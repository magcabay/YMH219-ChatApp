namespace ChatServer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblPort = new Label();
            txtPort = new TextBox();
            btnBaslat = new Button();
            btnDurdur = new Button();
            lblDurum = new Label();
            lblIstemciler = new Label();
            lstIstemciler = new ListBox();
            lblLog = new Label();
            lstLog = new ListBox();
            txtDuyuru = new TextBox();
            btnDuyuru = new Button();
            SuspendLayout();
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Location = new Point(16, 20);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(32, 15);
            lblPort.TabIndex = 0;
            lblPort.Text = "Port:";
            //
            // txtPort
            //
            txtPort.Location = new Point(54, 16);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(80, 23);
            txtPort.TabIndex = 1;
            txtPort.Text = "5000";
            //
            // btnBaslat
            //
            btnBaslat.Location = new Point(146, 12);
            btnBaslat.Name = "btnBaslat";
            btnBaslat.Size = new Size(120, 30);
            btnBaslat.TabIndex = 2;
            btnBaslat.Text = "Sunucuyu Başlat";
            btnBaslat.UseVisualStyleBackColor = true;
            btnBaslat.Click += btnBaslat_Click;
            //
            // btnDurdur
            //
            btnDurdur.Enabled = false;
            btnDurdur.Location = new Point(272, 12);
            btnDurdur.Name = "btnDurdur";
            btnDurdur.Size = new Size(120, 30);
            btnDurdur.TabIndex = 3;
            btnDurdur.Text = "Sunucuyu Durdur";
            btnDurdur.UseVisualStyleBackColor = true;
            btnDurdur.Click += btnDurdur_Click;
            //
            // lblDurum
            //
            lblDurum.AutoSize = true;
            lblDurum.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDurum.ForeColor = Color.IndianRed;
            lblDurum.Location = new Point(410, 18);
            lblDurum.Name = "lblDurum";
            lblDurum.Size = new Size(126, 19);
            lblDurum.TabIndex = 4;
            lblDurum.Text = "Durum: Kapalı";
            //
            // lblIstemciler
            //
            lblIstemciler.AutoSize = true;
            lblIstemciler.Location = new Point(16, 56);
            lblIstemciler.Name = "lblIstemciler";
            lblIstemciler.Size = new Size(230, 15);
            lblIstemciler.TabIndex = 5;
            lblIstemciler.Text = "Bağlı istemciler (ad | IP:port | bağlanma):";
            //
            // lstIstemciler
            //
            lstIstemciler.FormattingEnabled = true;
            lstIstemciler.IntegralHeight = false;
            lstIstemciler.Location = new Point(16, 76);
            lstIstemciler.Name = "lstIstemciler";
            lstIstemciler.Size = new Size(280, 360);
            lstIstemciler.TabIndex = 6;
            //
            // lblLog
            //
            lblLog.AutoSize = true;
            lblLog.Location = new Point(310, 56);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(90, 15);
            lblLog.TabIndex = 7;
            lblLog.Text = "Sunucu kayıtları:";
            //
            // lstLog
            //
            lstLog.FormattingEnabled = true;
            lstLog.HorizontalScrollbar = true;
            lstLog.IntegralHeight = false;
            lstLog.Location = new Point(310, 76);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(400, 360);
            lstLog.TabIndex = 8;
            //
            // txtDuyuru
            //
            txtDuyuru.Location = new Point(16, 448);
            txtDuyuru.Name = "txtDuyuru";
            txtDuyuru.PlaceholderText = "Tüm istemcilere duyuru gönder...";
            txtDuyuru.Size = new Size(568, 23);
            txtDuyuru.TabIndex = 9;
            txtDuyuru.KeyDown += txtDuyuru_KeyDown;
            //
            // btnDuyuru
            //
            btnDuyuru.Enabled = false;
            btnDuyuru.Location = new Point(590, 445);
            btnDuyuru.Name = "btnDuyuru";
            btnDuyuru.Size = new Size(120, 29);
            btnDuyuru.TabIndex = 10;
            btnDuyuru.Text = "Duyuru Gönder";
            btnDuyuru.UseVisualStyleBackColor = true;
            btnDuyuru.Click += btnDuyuru_Click;
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(726, 486);
            Controls.Add(btnDuyuru);
            Controls.Add(txtDuyuru);
            Controls.Add(lstLog);
            Controls.Add(lblLog);
            Controls.Add(lstIstemciler);
            Controls.Add(lblIstemciler);
            Controls.Add(lblDurum);
            Controls.Add(btnDurdur);
            Controls.Add(btnBaslat);
            Controls.Add(txtPort);
            Controls.Add(lblPort);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chat Sunucu - YMH219";
            FormClosing += Form1_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPort;
        private TextBox txtPort;
        private Button btnBaslat;
        private Button btnDurdur;
        private Label lblDurum;
        private Label lblIstemciler;
        private ListBox lstIstemciler;
        private Label lblLog;
        private ListBox lstLog;
        private TextBox txtDuyuru;
        private Button btnDuyuru;
    }
}
