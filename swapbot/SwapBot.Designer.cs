namespace swapbot
{
    partial class SwapBot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btCheck = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbJawny = new System.Windows.Forms.TextBox();
            this.tbTajny = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudPerc = new System.Windows.Forms.NumericUpDown();
            this.nudTimer = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btGOGOGO = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btStop = new System.Windows.Forms.Button();
            this.lblLicz = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudPerc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimer)).BeginInit();
            this.SuspendLayout();
            // 
            // btCheck
            // 
            this.btCheck.Location = new System.Drawing.Point(187, 30);
            this.btCheck.Name = "btCheck";
            this.btCheck.Size = new System.Drawing.Size(75, 23);
            this.btCheck.TabIndex = 0;
            this.btCheck.Text = "Sprawdź";
            this.btCheck.UseVisualStyleBackColor = true;
            this.btCheck.Click += new System.EventHandler(this.btCheck_click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Klucz jawny";
            // 
            // tbJawny
            // 
            this.tbJawny.Location = new System.Drawing.Point(81, 6);
            this.tbJawny.MaxLength = 32;
            this.tbJawny.Name = "tbJawny";
            this.tbJawny.Size = new System.Drawing.Size(100, 20);
            this.tbJawny.TabIndex = 2;
            // 
            // tbTajny
            // 
            this.tbTajny.Location = new System.Drawing.Point(81, 32);
            this.tbTajny.MaxLength = 32;
            this.tbTajny.Name = "tbTajny";
            this.tbTajny.Size = new System.Drawing.Size(100, 20);
            this.tbTajny.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Klucz tajny";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Ustawiaj";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "% poniżej \"cutoff\"";
            // 
            // nudPerc
            // 
            this.nudPerc.DecimalPlaces = 2;
            this.nudPerc.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudPerc.Location = new System.Drawing.Point(81, 107);
            this.nudPerc.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudPerc.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nudPerc.Name = "nudPerc";
            this.nudPerc.Size = new System.Drawing.Size(120, 20);
            this.nudPerc.TabIndex = 7;
            this.nudPerc.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // nudTimer
            // 
            this.nudTimer.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudTimer.Location = new System.Drawing.Point(81, 133);
            this.nudTimer.Maximum = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.nudTimer.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudTimer.Name = "nudTimer";
            this.nudTimer.Size = new System.Drawing.Size(120, 20);
            this.nudTimer.TabIndex = 10;
            this.nudTimer.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(207, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "sekund";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Sprawdzaj co";
            // 
            // btGOGOGO
            // 
            this.btGOGOGO.Location = new System.Drawing.Point(187, 159);
            this.btGOGOGO.Name = "btGOGOGO";
            this.btGOGOGO.Size = new System.Drawing.Size(75, 23);
            this.btGOGOGO.TabIndex = 11;
            this.btGOGOGO.Text = "Lecimy!";
            this.btGOGOGO.UseVisualStyleBackColor = true;
            this.btGOGOGO.Click += new System.EventHandler(this.btGOGOGO_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(12, 188);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(335, 208);
            this.tbLog.TabIndex = 12;
            this.tbLog.Text = "Tu będzie wpadać odpowiedź z serwera";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btStop
            // 
            this.btStop.Enabled = false;
            this.btStop.Location = new System.Drawing.Point(272, 159);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 23);
            this.btStop.TabIndex = 13;
            this.btStop.Text = "STOP";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_click);
            // 
            // lblLicz
            // 
            this.lblLicz.AutoSize = true;
            this.lblLicz.Location = new System.Drawing.Point(146, 169);
            this.lblLicz.Name = "lblLicz";
            this.lblLicz.Size = new System.Drawing.Size(36, 13);
            this.lblLicz.TabIndex = 14;
            this.lblLicz.Text = "licznik";
            // 
            // SwapBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 408);
            this.Controls.Add(this.lblLicz);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.btGOGOGO);
            this.Controls.Add(this.nudTimer);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudPerc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbTajny);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbJawny);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btCheck);
            this.Name = "SwapBot";
            this.Text = "Bitmarket SwapBot by rav3n_pl";
            ((System.ComponentModel.ISupportInitialize)(this.nudPerc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbJawny;
        private System.Windows.Forms.TextBox tbTajny;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudPerc;
        private System.Windows.Forms.NumericUpDown nudTimer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btGOGOGO;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Label lblLicz;
    }
}

