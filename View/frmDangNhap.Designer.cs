using ThuVien;

namespace ThuVien.View
{
    partial class frmDangNhap
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
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDangNhap));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(components);
            pictureBox1 = new PictureBox();
            txtMatKhau = new Guna.UI2.WinForms.Guna2TextBox();
            txtTenDangNhap = new Guna.UI2.WinForms.Guna2TextBox();
            btnThoat = new Guna.UI2.WinForms.Guna2Button();
            lblVuiLongNhapMatKhau = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblQuenMatKhau = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnHienMatKhau = new Guna.UI2.WinForms.Guna2Button();
            lblVuiLongNhapTDN = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lblDangNhap = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnDangNhap = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources._2;
            pictureBox1.Location = new Point(-1, 0);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(740, 735);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            pictureBox1.Click += btnDangNhap_Click;
            // 
            // txtMatKhau
            // 
            txtMatKhau.BorderRadius = 10;
            txtMatKhau.CustomizableEdges = customizableEdges1;
            txtMatKhau.DefaultText = "";
            txtMatKhau.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtMatKhau.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtMatKhau.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtMatKhau.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtMatKhau.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtMatKhau.Font = new Font("Times New Roman", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMatKhau.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtMatKhau.IconLeft = Properties.Resources._3;
            txtMatKhau.IconLeftSize = new Size(30, 30);
            txtMatKhau.Location = new Point(802, 367);
            txtMatKhau.Margin = new Padding(3, 4, 3, 4);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.PlaceholderForeColor = Color.Teal;
            txtMatKhau.PlaceholderText = "Mật Khẩu";
            txtMatKhau.SelectedText = "";
            txtMatKhau.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtMatKhau.Size = new Size(433, 42);
            txtMatKhau.TabIndex = 8;
            txtMatKhau.UseSystemPasswordChar = true;
            txtMatKhau.TextChanged += txtMatKhau_TextChanged;
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.BorderRadius = 10;
            txtTenDangNhap.CustomizableEdges = customizableEdges3;
            txtTenDangNhap.DefaultText = "";
            txtTenDangNhap.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtTenDangNhap.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtTenDangNhap.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtTenDangNhap.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtTenDangNhap.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTenDangNhap.Font = new Font("Times New Roman", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTenDangNhap.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTenDangNhap.IconLeft = (Image)resources.GetObject("txtTenDangNhap.IconLeft");
            txtTenDangNhap.IconLeftSize = new Size(30, 30);
            txtTenDangNhap.Location = new Point(801, 282);
            txtTenDangNhap.Margin = new Padding(3, 4, 3, 4);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.PlaceholderForeColor = Color.Teal;
            txtTenDangNhap.PlaceholderText = "Tên Đăng Nhập";
            txtTenDangNhap.SelectedText = "";
            txtTenDangNhap.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtTenDangNhap.Size = new Size(433, 43);
            txtTenDangNhap.TabIndex = 7;
            txtTenDangNhap.TextChanged += txtTenDangNhap_TextChanged;
            // 
            // btnThoat
            // 
            btnThoat.BorderRadius = 10;
            btnThoat.CustomizableEdges = customizableEdges5;
            btnThoat.DisabledState.BorderColor = Color.DarkGray;
            btnThoat.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThoat.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThoat.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThoat.FillColor = Color.Blue;
            btnThoat.Font = new Font("Times New Roman", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnThoat.ForeColor = Color.White;
            btnThoat.Location = new Point(1163, 501);
            btnThoat.Margin = new Padding(2);
            btnThoat.Name = "btnThoat";
            btnThoat.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnThoat.Size = new Size(71, 42);
            btnThoat.TabIndex = 10;
            btnThoat.Text = "Thoát";
            btnThoat.Click += btnThoat_Click;
            // 
            // lblVuiLongNhapMatKhau
            // 
            lblVuiLongNhapMatKhau.BackColor = Color.Transparent;
            lblVuiLongNhapMatKhau.Font = new Font("Times New Roman", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblVuiLongNhapMatKhau.ForeColor = Color.Red;
            lblVuiLongNhapMatKhau.Location = new Point(802, 415);
            lblVuiLongNhapMatKhau.Margin = new Padding(2);
            lblVuiLongNhapMatKhau.Name = "lblVuiLongNhapMatKhau";
            lblVuiLongNhapMatKhau.Size = new Size(154, 19);
            lblVuiLongNhapMatKhau.TabIndex = 12;
            lblVuiLongNhapMatKhau.Text = "Vui lòng nhập mật khẩu";
            lblVuiLongNhapMatKhau.Visible = false;
            // 
            // lblQuenMatKhau
            // 
            lblQuenMatKhau.BackColor = Color.Transparent;
            lblQuenMatKhau.Font = new Font("Times New Roman", 9F, FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point, 0);
            lblQuenMatKhau.ForeColor = Color.Red;
            lblQuenMatKhau.Location = new Point(1135, 415);
            lblQuenMatKhau.Margin = new Padding(2);
            lblQuenMatKhau.Name = "lblQuenMatKhau";
            lblQuenMatKhau.Size = new Size(99, 19);
            lblQuenMatKhau.TabIndex = 13;
            lblQuenMatKhau.Text = "Quên mật khẩu";
            lblQuenMatKhau.Click += lblQuenMatKhau_Click;
            // 
            // btnHienMatKhau
            // 
            btnHienMatKhau.BorderColor = Color.FromArgb(213, 218, 223);
            btnHienMatKhau.BorderRadius = 10;
            btnHienMatKhau.BorderThickness = 1;
            btnHienMatKhau.CustomizableEdges = customizableEdges7;
            btnHienMatKhau.DisabledState.BorderColor = Color.DarkGray;
            btnHienMatKhau.DisabledState.CustomBorderColor = Color.DarkGray;
            btnHienMatKhau.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnHienMatKhau.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnHienMatKhau.FillColor = Color.White;
            btnHienMatKhau.Font = new Font("Times New Roman", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnHienMatKhau.ForeColor = Color.White;
            btnHienMatKhau.Image = (Image)resources.GetObject("btnHienMatKhau.Image");
            btnHienMatKhau.ImageSize = new Size(30, 30);
            btnHienMatKhau.Location = new Point(1244, 367);
            btnHienMatKhau.Margin = new Padding(2);
            btnHienMatKhau.Name = "btnHienMatKhau";
            btnHienMatKhau.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnHienMatKhau.Size = new Size(53, 42);
            btnHienMatKhau.TabIndex = 14;
            btnHienMatKhau.Click += btnHienMatKhau_Click;
            // 
            // lblVuiLongNhapTDN
            // 
            lblVuiLongNhapTDN.BackColor = Color.Transparent;
            lblVuiLongNhapTDN.Font = new Font("Times New Roman", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblVuiLongNhapTDN.ForeColor = Color.Red;
            lblVuiLongNhapTDN.Location = new Point(801, 331);
            lblVuiLongNhapTDN.Margin = new Padding(2);
            lblVuiLongNhapTDN.Name = "lblVuiLongNhapTDN";
            lblVuiLongNhapTDN.Size = new Size(187, 19);
            lblVuiLongNhapTDN.TabIndex = 15;
            lblVuiLongNhapTDN.Text = "Vui lòng nhập tên đăng nhập";
            lblVuiLongNhapTDN.Visible = false;
            // 
            // lblDangNhap
            // 
            lblDangNhap.BackColor = Color.Transparent;
            lblDangNhap.Font = new Font("Tahoma", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDangNhap.ForeColor = SystemColors.ActiveCaptionText;
            lblDangNhap.Location = new Point(883, 166);
            lblDangNhap.Margin = new Padding(2, 0, 2, 0);
            lblDangNhap.Name = "lblDangNhap";
            lblDangNhap.Size = new Size(249, 50);
            lblDangNhap.TabIndex = 5;
            lblDangNhap.Text = "ĐĂNG NHẬP";
            // 
            // btnDangNhap
            // 
            btnDangNhap.BorderRadius = 10;
            btnDangNhap.CustomizableEdges = customizableEdges9;
            btnDangNhap.DisabledState.BorderColor = Color.DarkGray;
            btnDangNhap.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDangNhap.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDangNhap.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDangNhap.FillColor = Color.Blue;
            btnDangNhap.Font = new Font("Times New Roman", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.Location = new Point(802, 501);
            btnDangNhap.Margin = new Padding(2);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnDangNhap.Size = new Size(347, 42);
            btnDangNhap.TabIndex = 9;
            btnDangNhap.Text = "Đăng Nhập";
            btnDangNhap.Click += btnDangNhap_Click;
            // 
            // frmDangNhap
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1342, 733);
            Controls.Add(lblDangNhap);
            Controls.Add(lblVuiLongNhapTDN);
            Controls.Add(btnHienMatKhau);
            Controls.Add(lblQuenMatKhau);
            Controls.Add(lblVuiLongNhapMatKhau);
            Controls.Add(btnThoat);
            Controls.Add(btnDangNhap);
            Controls.Add(txtMatKhau);
            Controls.Add(txtTenDangNhap);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "frmDangNhap";
            Text = "ĐĂNG NHẬP";
            Load += frmDangNhap_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2TextBox txtMatKhau;
        private Guna.UI2.WinForms.Guna2TextBox txtTenDangNhap;
        private Guna.UI2.WinForms.Guna2Button btnThoat;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblVuiLongNhapMatKhau;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblQuenMatKhau;
        private Guna.UI2.WinForms.Guna2Button btnHienMatKhau;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblVuiLongNhapTDN;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDangNhap;
        private Guna.UI2.WinForms.Guna2Button btnDangNhap;
    }


}
