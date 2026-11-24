namespace ChatClient
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
            grpBaglanti = new GroupBox();
            lblDurum = new Label();
            btnKes = new Button();
            btnBaglan = new Button();
            txtKullanici = new TextBox();
            lblKullanici = new Label();
            txtPort = new TextBox();
            lblPort = new Label();
            txtSunucu = new TextBox();
            lblSunucu = new Label();
            lstMesajlar = new ListBox();
            lblCevrimici = new Label();
            lstKullanicilar = new ListBox();
            chkOzel = new CheckBox();
            txtMesaj = new TextBox();
            btnGonder = new Button();
            btnGecmisKaydet = new Button();
            grpBaglanti.SuspendLayout();
            SuspendLayout();
            //
            // grpBaglanti
            //
            grpBaglanti.Controls.Add(lblDurum);
            grpBaglanti.Controls.Add(btnKes);
            grpBaglanti.Controls.Add(btnBaglan);
            grpBaglanti.Controls.Add(txtKullanici);
            grpBaglanti.Controls.Add(lblKullanici);
            grpBaglanti.Controls.Add(txtPort);
            grpBaglanti.Controls.Add(lblPort);
            grpBaglanti.Controls.Add(txtSunucu);
            grpBaglanti.Controls.Add(lblSunucu);
            grpBaglanti.Location = new Point(12, 8);
            grpBaglanti.Name = "grpBaglanti";
            grpBaglanti.Size = new Size(700, 88);
            grpBaglanti.TabIndex = 0;
            grpBaglanti.TabStop = false;
            grpBaglanti.Text = "Bağlantı";
            //
            // lblDurum
            //
            lblDurum.AutoSize = true;
            lblDurum.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblDurum.ForeColor = Color.IndianRed;
            lblDurum.Location = new Point(12, 60);
            lblDurum.Name = "lblDurum";
            lblDurum.Size = new Size(124, 17);
            lblDurum.TabIndex = 8;
            lblDurum.Text = "Durum: Bağlı değil";
            //
            // btnKes
            //
            btnKes.Enabled = false;
            btnKes.Location = new Point(574, 22);
            btnKes.Name = "btnKes";
            btnKes.Size = new Size(114, 28);
            btnKes.TabIndex = 7;
            btnKes.Text = "Bağlantıyı Kes";
            btnKes.UseVisualStyleBackColor = true;
            btnKes.Click += btnKes_Click;
            //
            // btnBaglan
            //
            btnBaglan.Location = new Point(478, 22);
            btnBaglan.Name = "btnBaglan";
            btnBaglan.Size = new Size(90, 28);
            btnBaglan.TabIndex = 6;
            btnBaglan.Text = "Bağlan";
            btnBaglan.UseVisualStyleBackColor = true;
            btnBaglan.Click += btnBaglan_Click;
            //
            // txtKullanici
            //
            txtKullanici.Location = new Point(352, 25);
            txtKullanici.MaxLength = 20;
            txtKullanici.Name = "txtKullanici";
            txtKullanici.Size = new Size(116, 23);
            txtKullanici.TabIndex = 5;
            //
            // lblKullanici
            //
            lblKullanici.AutoSize = true;
            lblKullanici.Location = new Point(276, 28);
            lblKullanici.Name = "lblKullanici";
            lblKullanici.Size = new Size(73, 15);
            lblKullanici.TabIndex = 4;
            lblKullanici.Text = "Kullanıcı adı:";
            //
            // txtPort
            //
            txtPort.Location = new Point(208, 25);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(58, 23);
            txtPort.TabIndex = 3;
            txtPort.Text = "5000";
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Location = new Point(174, 28);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(32, 15);
            lblPort.TabIndex = 2;
            lblPort.Text = "Port:";
            //
            // txtSunucu
            //
            txtSunucu.Location = new Point(64, 25);
            txtSunucu.Name = "txtSunucu";
            txtSunucu.Size = new Size(100, 23);
            txtSunucu.TabIndex = 1;
            txtSunucu.Text = "127.0.0.1";
            //
            // lblSunucu
            //
            lblSunucu.AutoSize = true;
            lblSunucu.Location = new Point(12, 28);
            lblSunucu.Name = "lblSunucu";
            lblSunucu.Size = new Size(49, 15);
            lblSunucu.TabIndex = 0;
            lblSunucu.Text = "Sunucu:";
            //
            // lstMesajlar
            //
            lstMesajlar.FormattingEnabled = true;
            lstMesajlar.HorizontalScrollbar = true;
            lstMesajlar.IntegralHeight = false;
            lstMesajlar.Location = new Point(12, 106);
            lstMesajlar.Name = "lstMesajlar";
            lstMesajlar.Size = new Size(520, 330);
            lstMesajlar.TabIndex = 1;
            //
            // lblCevrimici
            //
            lblCevrimici.AutoSize = true;
            lblCevrimici.Location = new Point(544, 106);
            lblCevrimici.Name = "lblCevrimici";
            lblCevrimici.Size = new Size(118, 15);
            lblCevrimici.TabIndex = 2;
            lblCevrimici.Text = "Çevrimiçi kullanıcılar:";
            //
            // lstKullanicilar
            //
            lstKullanicilar.FormattingEnabled = true;
            lstKullanicilar.IntegralHeight = false;
            lstKullanicilar.Location = new Point(544, 126);
            lstKullanicilar.Name = "lstKullanicilar";
            lstKullanicilar.Size = new Size(168, 280);
            lstKullanicilar.TabIndex = 3;
            //
            // chkOzel
            //
            chkOzel.Location = new Point(544, 410);
            chkOzel.Name = "chkOzel";
            chkOzel.Size = new Size(168, 34);
            chkOzel.TabIndex = 4;
            chkOzel.Text = "Seçili kullanıcıya özel mesaj";
            chkOzel.UseVisualStyleBackColor = true;
            //
            // txtMesaj
            //
            txtMesaj.Enabled = false;
            txtMesaj.Location = new Point(12, 448);
            txtMesaj.MaxLength = 500;
            txtMesaj.Name = "txtMesaj";
            txtMesaj.PlaceholderText = "Mesajınızı yazın ve Enter'a basın...";
            txtMesaj.Size = new Size(420, 23);
            txtMesaj.TabIndex = 5;
            txtMesaj.KeyDown += txtMesaj_KeyDown;
            //
            // btnGonder
            //
            btnGonder.Enabled = false;
            btnGonder.Location = new Point(438, 445);
            btnGonder.Name = "btnGonder";
            btnGonder.Size = new Size(94, 29);
            btnGonder.TabIndex = 6;
            btnGonder.Text = "Gönder";
            btnGonder.UseVisualStyleBackColor = true;
            btnGonder.Click += btnGonder_Click;
            //
            // btnGecmisKaydet
            //
            btnGecmisKaydet.Location = new Point(544, 445);
            btnGecmisKaydet.Name = "btnGecmisKaydet";
            btnGecmisKaydet.Size = new Size(168, 29);
            btnGecmisKaydet.TabIndex = 7;
            btnGecmisKaydet.Text = "Geçmişi Kaydet (.txt)";
            btnGecmisKaydet.UseVisualStyleBackColor = true;
            btnGecmisKaydet.Click += btnGecmisKaydet_Click;
            //
            // Form1
            //
            AcceptButton = btnBaglan;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(724, 486);
            Controls.Add(btnGecmisKaydet);
            Controls.Add(btnGonder);
            Controls.Add(txtMesaj);
            Controls.Add(chkOzel);
            Controls.Add(lstKullanicilar);
            Controls.Add(lblCevrimici);
            Controls.Add(lstMesajlar);
            Controls.Add(grpBaglanti);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chat İstemci - YMH219";
            FormClosing += Form1_FormClosing;
            grpBaglanti.ResumeLayout(false);
            grpBaglanti.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpBaglanti;
        private Label lblDurum;
        private Button btnKes;
        private Button btnBaglan;
        private TextBox txtKullanici;
        private Label lblKullanici;
        private TextBox txtPort;
        private Label lblPort;
        private TextBox txtSunucu;
        private Label lblSunucu;
        private ListBox lstMesajlar;
        private Label lblCevrimici;
        private ListBox lstKullanicilar;
        private CheckBox chkOzel;
        private TextBox txtMesaj;
        private Button btnGonder;
        private Button btnGecmisKaydet;
    }
}
