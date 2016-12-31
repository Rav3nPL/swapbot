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

        bool flag = false; //blokowanie wykonania tickera podczas jego pracy
        string nl = Environment.NewLine; //nowa linia
        int timer = 0; //licznik czasu
        string konf = "konfig.txt"; // plik konfiguracji
        bool error = false; //jeżeli wystapi wyjątek w transmisji przerywamy

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
                log(e);
                tbLog.Text += nl + "Błąd transmisji! " + nl + e.Message.ToString() + nl;
                error = true;
            }
            tbLog.Text += nl + metoda + nl + resp;//responsy po kolei do okienka
            if (resp == "") //odpowiedź nigdy nie powinna być pusta
            {
                error = true;
            }
            else //zawsze powinno być success:true
            {
                var ok = new { success = "" };
                var json = JsonConvert.DeserializeAnonymousType(resp, ok);
                if (json.success != "true") 
                {
                    Exception e = new Exception(resp);
                    log(e);
                    error = true;
                }
            }

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
            error = false;
            tbLog.Text = "";//czyścimy loga
            Application.DoEvents();
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
                log(e);
                tbLog.Text += nl + "Błąd pobierania danych!" + nl + e.Message.ToString() + nl;
                error = true;
            }
            if (error || jstring == "") return; //kończymy jak błąd

            var cut = new { cutoff = "" };
            var json = JsonConvert.DeserializeAnonymousType(jstring, cut);
            decimal currCutoff = Convert.ToDecimal(json.cutoff, cult);
            tbLog.Text += nl + "Obecny cutoff: " + currCutoff + nl;
            decimal ustaw = rbProc.Checked ? currCutoff * ((100 - nudPerc.Value) / 100) : currCutoff - nudPerc.Value; //teraz to jest "ile ma być"
            ustaw = Math.Truncate(ustaw * 10000) / 10000;//zaokrąglam do 4 miejsc.
            string newRate = ustaw.ToString(cult);

            //swaplist currency  BTC ->id
            NameValueCollection par = new NameValueCollection();
            par.Add("currency", "BTC");
            string resp = postuj("swapList", par);
            if (error) return; //kończymy jak błąd
            dynamic jresp = JsonConvert.DeserializeObject(resp);
            string id = "";
            string rate = "0";
            if (jresp.data.Count > 0)
            {
                id = jresp.data[0].id;
                rate = jresp.data[0].rate;
            }

            decimal diff = currCutoff - Convert.ToDecimal(rate, cult);
            //potrzebujemy dopuszcalnej odchyłki "w górę" zanim przestawimy swapa - 105% ustawianej różnicy wydaje mi się sensowne
            decimal maxDiff = (decimal)((double)(rbProc.Checked ? currCutoff - currCutoff * ((100 - nudPerc.Value) / 100) : nudPerc.Value) * 1.05);
            tbLog.Text += nl + "Obliczona różnica: " + diff + nl;
            if (diff <= 0 || diff >= currCutoff - ustaw + maxDiff || currCutoff == diff)//jeżeli jesteśmy za wysoko, lub kurs wzrósł, lub nic nie mamy
            {
                string stan = "";
                if (id != "") //jak mam swapa to go zamykam
                {
                    //swapClose id currency -> balances
                    par.Clear();
                    par.Add("id", id);
                    par.Add("currency", "BTC");
                    resp = postuj("swapClose", par); //zamknij swapa
                    if (error) return; //kończymy jak błąd
                    jresp = JsonConvert.DeserializeObject(resp);
                    stan = jresp.data.balances.available.BTC;//ile masz BTC
                }
                else //jak nie to sprawdzam stan konta
                {
                    par.Clear();
                    resp = postuj("info", null);
                    if (error) return; //kończymy jak błąd
                    jresp = JsonConvert.DeserializeObject(resp);
                    stan = jresp.data.balances.available.BTC;
                }
                if (Convert.ToDouble(stan, cult) > 0.1) //jezeli mamy powyżej 0.1BTC wolnego
                {
                    //swapOpen currency amount rate -> id balances
                    par.Clear();
                    par.Add("currency", "BTC");
                    par.Add("amount", stan);
                    par.Add("rate", newRate);
                    resp = postuj("swapOpen", par);//OPEN SESAME!
                    Application.DoEvents();//haxx żeby log mógł się odświerzyć
                    ticker();//sprawdź po ustawieniu czy jest ok, ew od razu popraw.
                }
                
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
            try //ye olde ultimate bughunter
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
                        if (error)
                        {
                            tbLog.Text += nl + "Wystąpił bład transmisji!" + nl;
                            error = false;
                        }
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
            catch (Exception ex)
            {
                log(ex);
                Application.Restart(); //bez marudzenia od nowa :P
            }
        }

        void log(Exception e)
        {
            StringBuilder s=new StringBuilder();
            s.Append(DateTime.Now.ToString());
            s.Append(nl);
            s.Append(e.Message.ToString());
            s.Append(nl);
            if (e.Source != null)
            {
                s.Append(e.Source.ToString());
                s.Append(nl);
            }
            if (e.StackTrace != null)
            {
                s.Append(e.StackTrace.ToString());
                s.Append(nl);
            }
            File.AppendAllText("log.txt", s.ToString());
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

        private void SwapBot_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)//przy minimalizacji
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1500);
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }
    }
}