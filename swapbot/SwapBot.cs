using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace swapbot
{
    public partial class SwapBot : Form
    {
        public SwapBot()
        {
            InitializeComponent();
        }

        bool flag = false;
        string nl = Environment.NewLine;
        int timer = 0;
        string konf = "konfig.txt";

        static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
                sbinary += buff[i].ToString("x2"); // małe hexy
            return sbinary;
        }
        private static readonly Encoding encoding = Encoding.UTF8;
        private string podpis(string message, string secret)
        {
            var keyByte = encoding.GetBytes(secret);
            using (var hmacsha512 = new HMACSHA512(keyByte))
            {
                hmacsha512.ComputeHash(encoding.GetBytes(message));
                return ByteToString(hmacsha512.Hash);
            }
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach (string s in nvc)
            {
                queryString.Add(s, nvc[s]);
            }
            return queryString.ToString();
        }

        private string postuj(string metoda, NameValueCollection nvc)
        {
            if (nvc == null) { nvc = new NameValueCollection(); } //dla info
            string resp = "";
            string key = tbJawny.Text;
            string secret = tbTajny.Text;
            string tonce = ((Int32)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            //wygeneruj zapytanie w json
            nvc.Add("method", metoda);
            nvc.Add("tonce", tonce);
            string post = ToQueryString(nvc);
            //podpisz
            string hasz = podpis(post, secret);
            //wyślij
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers["API-Key"] = key;
                    client.Headers["API-Hash"] = hasz;
                    resp = client.UploadString("https://www.bitmarket.pl/api2/", post);
                }
            }
            catch (Exception e)
            {
                tbLog.Text += nl + "Błąd transmisji! " + nl + e.Message.ToString() + nl;
            }
            tbLog.Text += nl + metoda + nl + resp;//responsy po kolei do okienka
            return resp;
        }

        private void btCheck_click(object sender, EventArgs e)
        {
            string metoda = "info";
            string resp = postuj(metoda, null);

            var ok = new { success = "" };
            var json = JsonConvert.DeserializeAnonymousType(resp, ok);
            if (json.success == "true")
            {
                MessageBox.Show("Ok, możemy działać!");
            }
            else
            {
                MessageBox.Show("Coś nie halo!");
            }
        }

        void ticker()
        {
            tbLog.Text = "";//czyścimy loga
            string jstring = "";
            CultureInfo cult = new CultureInfo("en-US");
            try
            {
                using (WebClient wc = new WebClient())
                {
                    jstring = wc.DownloadString("http://bitmarket.pl/json/swapBTC/swap.json");
                }
            }
            catch (Exception e)
            {
                tbLog.Text += nl + "Błąd pobierania danych!" + nl + e.Message.ToString() + nl;
            }
            var cut = new { cutoff = "" };
            var json = JsonConvert.DeserializeAnonymousType(jstring, cut);
            decimal ileJest = Convert.ToDecimal(json.cutoff, cult);
            tbLog.Text += nl + "Obecny cutoff: " + ileJest + nl;
            decimal ustaw = rbProc.Checked ? ileJest * ((100 - nudPerc.Value) / 100) : ileJest - nudPerc.Value; //teraz to jest "ile ma być"
            ustaw = Math.Truncate(ustaw * 1000) / 1000;//zaokrąglam do 3 miejsc.
            string newRate = ustaw.ToString(cult);

            //swaplist currency  BTC ->id
            NameValueCollection par = new NameValueCollection();
            par.Add("currency", "BTC");
            string resp = postuj("swapList", par);

            dynamic jresp = JsonConvert.DeserializeObject(resp);
            string id = "";
            string rate = "0";
            if (jresp.data.Count > 0)
            {
                id = jresp.data[0].id;
                rate = jresp.data[0].rate;
            }

            decimal diff = ileJest - Convert.ToDecimal(rate, cult);
            tbLog.Text += nl + "Obliczona różnica: " + diff + nl;
            if (diff <= 0 || diff > nudPerc.Value)//jeżeli nie zarabiamy lub różnica jest większa niż %
            {
                string stan = "";
                if (id != "") //jak mam swapa to go zamykam
                {
                    //swapClose id currency -> balances
                    par.Clear();
                    par.Add("id", id);
                    par.Add("currency", "BTC");
                    resp = postuj("swapClose", par); //zamknij swapa
                    jresp = JsonConvert.DeserializeObject(resp);
                    stan = jresp.data.balances.available.BTC;//ile masz BTC
                }
                else //jak nie to sprawdzam stan konta
                {
                    par.Clear();
                    resp = postuj("info", null);
                    jresp = JsonConvert.DeserializeObject(resp);
                    stan = jresp.data.balances.available.BTC;
                }

                //swapOpen currency amount rate -> id balances
                par.Clear();
                par.Add("currency", "BTC");
                par.Add("amount", stan);
                par.Add("rate", newRate);
                resp = postuj("swapOpen", par);//OPEN SESAME!
            }
        }

        private void btGOGOGO_Click(object sender, EventArgs e)
        {
            btGOGOGO.Enabled = false;
            btStop.Enabled = true;
            ticker();//pierwsze wywołanie od razu
            timer = (int)nudTimer.Value;
            timer1.Enabled = true;
            timer1.Start();
            flag = false;
        }

        private void btStop_click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
            flag = true;
            btGOGOGO.Enabled = true;
            btStop.Enabled = false;
            MessageBox.Show("Zatrzymane.");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer--;
            lblLicz.Text = timer.ToString();
            if (flag == false)
            {
                if (timer == 1) //nie odliczam do zera :P
                {
                    timer = (int)nudTimer.Value;
                    flag = true; //zablokować ew. kolejne wywyołania
                    ticker();
                    flag = false; //koniec, może zacząć od nowa
                }
            }
            else
            {
                if (timer < 0)
                {
                    tbLog.Text = "Wygląda na to, że poprzednie wywyołanie się nie skończyło. Trochę za długo, nieprawda-ż?";
                }
            }
        }

        private void SwapBot_Load(object sender, EventArgs e)
        {
            if (File.Exists(konf))
            {
                using (var sr = File.OpenText(konf))
                {
                    string k = sr.ReadLine(); tbJawny.Text = k;
                    k = sr.ReadLine(); tbTajny.Text = k;
                    k = sr.ReadLine(); nudPerc.Value = Convert.ToDecimal(k);
                    k = sr.ReadLine(); nudTimer.Value = Convert.ToDecimal(k);
                    k = sr.ReadLine(); rbArb.Checked = (k == "arb");
                    k = sr.ReadLine(); cbAuto.Checked = (k == "True");
                }
                if (cbAuto.Checked)
                {
                    btGOGOGO_Click(null, null);
                }
            }
        }

        private void btZapisz_Click(object sender, EventArgs e)
        {

            if (File.Exists(konf))
            {
                File.Delete(konf);
            }
            using (var sw = File.CreateText(konf))
            {
                sw.WriteLine(tbJawny.Text);
                sw.WriteLine(tbTajny.Text);
                sw.WriteLine(nudPerc.Value.ToString());
                sw.WriteLine(nudTimer.Value.ToString());
                sw.WriteLine(rbArb.Checked ? "arb" : "proc");
                sw.WriteLine(cbAuto.Checked.ToString());
                sw.Close();
            }
            tbLog.Text = "Konfiguracja zapisana w pliku konf.txt";
        }
    }
}

