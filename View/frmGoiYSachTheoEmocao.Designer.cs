namespace ThuVien.View
{
    partial class frmGoiYSachTheoEmocao
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
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblHuongDan = new System.Windows.Forms.Label();
            this.txtTrangThai = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnPhanTich = new Guna.UI2.WinForms.Guna2Button();
            this.pnlKetQua = new Guna.UI2.WinForms.Guna2Panel();
            this.lblCamXuc = new System.Windows.Forms.Label();
            this.lblDoTinCay = new System.Windows.Forms.Label();
            this.lblLyDo = new System.Windows.Forms.Label();
            this.lblDongVien = new System.Windows.Forms.Label();
            this.dgvGoiY = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnXemChiTiet = new Guna.UI2.WinForms.Guna2Button();
            this.btnLichSu = new Guna.UI2.WinForms.Guna2Button();
            this.lblDangXuLy = new System.Windows.Forms.Label();
            this.progressBar = new Guna.UI2.WinForms.Guna2ProgressBar();
            this.pnlVidu = new System.Windows.Forms.Panel();
            this.lblViDu = new System.Windows.Forms.Label();
            this.btnViDu1 = new Guna.UI2.WinForms.Guna2Button();
            this.btnViDu2 = new Guna.UI2.WinForms.Guna2Button();
            this.btnViDu3 = new Guna.UI2.WinForms.Guna2Button();
            this.pnlKetQua.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoiY)).BeginInit();
            this.pnlVidu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblTieuDe.Location = new System.Drawing.Point(20, 20);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(416, 32);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "📚 Gợi Ý Sách Theo Cảm Xúc";
            // 
            // lblHuongDan
            // 
            this.lblHuongDan.AutoSize = true;
            this.lblHuongDan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblHuongDan.Location = new System.Drawing.Point(23, 60);
            this.lblHuongDan.Name = "lblHuongDan";
            this.lblHuongDan.Size = new System.Drawing.Size(550, 19);
            this.lblHuongDan.TabIndex = 1;
            this.lblHuongDan.Text = "Hãy chia sẻ cảm xúc hoặc trạng thái của bạn hôm nay, chúng tôi sẽ gợi ý sách phù hợp! 💙";
            // 
            // txtTrangThai
            // 
            this.txtTrangThai.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtTrangThai.Location = new System.Drawing.Point(27, 90);
            this.txtTrangThai.Multiline = true;
            this.txtTrangThai.Name = "txtTrangThai";
            this.txtTrangThai.PlaceholderText = "Ví dụ: Hôm nay tôi buồn quá, cảm thấy mệt mỏi và cô đơn...";
            this.txtTrangThai.Size = new System.Drawing.Size(880, 80);
            this.txtTrangThai.TabIndex = 2;
            this.txtTrangThai.BorderRadius = 10;
            this.txtTrangThai.BorderThickness = 1;
            this.txtTrangThai.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.txtTrangThai.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            // 
            // btnPhanTich
            // 
            this.btnPhanTich.BackColor = System.Drawing.Color.Transparent;
            this.btnPhanTich.BorderRadius = 10;
            this.btnPhanTich.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnPhanTich.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPhanTich.ForeColor = System.Drawing.Color.White;
            this.btnPhanTich.Location = new System.Drawing.Point(927, 90);
            this.btnPhanTich.Name = "btnPhanTich";
            this.btnPhanTich.Size = new System.Drawing.Size(150, 80);
            this.btnPhanTich.TabIndex = 3;
            this.btnPhanTich.Text = "🔍 Phân Tích\r\n&& Gợi Ý";
            this.btnPhanTich.Click += new System.EventHandler(this.btnPhanTich_Click);
            // 
            // pnlKetQua
            // 
            this.pnlKetQua.Controls.Add(this.lblDongVien);
            this.pnlKetQua.Controls.Add(this.lblLyDo);
            this.pnlKetQua.Controls.Add(this.lblDoTinCay);
            this.pnlKetQua.Controls.Add(this.lblCamXuc);
            this.pnlKetQua.Location = new System.Drawing.Point(27, 270);
            this.pnlKetQua.Name = "pnlKetQua";
            this.pnlKetQua.Size = new System.Drawing.Size(1050, 120);
            this.pnlKetQua.TabIndex = 4;
            this.pnlKetQua.Visible = false;
            this.pnlKetQua.BorderRadius = 15;
            this.pnlKetQua.BorderThickness = 1;
            this.pnlKetQua.BorderColor = System.Drawing.Color.LightGray;
            this.pnlKetQua.FillColor = System.Drawing.Color.MintCream;
            // 
            // lblCamXuc
            // 
            this.lblCamXuc.AutoSize = true;
            this.lblCamXuc.BackColor = System.Drawing.Color.Transparent;
            this.lblCamXuc.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCamXuc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblCamXuc.Location = new System.Drawing.Point(10, 10);
            this.lblCamXuc.Name = "lblCamXuc";
            this.lblCamXuc.Size = new System.Drawing.Size(200, 21);
            this.lblCamXuc.TabIndex = 0;
            this.lblCamXuc.Text = "😊 Cảm xúc: Đang phân tích...";
            // 
            // lblDoTinCay
            // 
            this.lblDoTinCay.AutoSize = true;
            this.lblDoTinCay.BackColor = System.Drawing.Color.Transparent;
            this.lblDoTinCay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDoTinCay.ForeColor = System.Drawing.Color.Gray;
            this.lblDoTinCay.Location = new System.Drawing.Point(11, 35);
            this.lblDoTinCay.Name = "lblDoTinCay";
            this.lblDoTinCay.Size = new System.Drawing.Size(100, 19);
            this.lblDoTinCay.TabIndex = 1;
            this.lblDoTinCay.Text = "Độ tin cậy: 0%";
            // 
            // lblLyDo
            // 
            this.lblLyDo.BackColor = System.Drawing.Color.Transparent;
            this.lblLyDo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblLyDo.Location = new System.Drawing.Point(11, 58);
            this.lblLyDo.Name = "lblLyDo";
            this.lblLyDo.Size = new System.Drawing.Size(1025, 25);
            this.lblLyDo.TabIndex = 2;
            this.lblLyDo.Text = "Lý do: ...";
            // 
            // lblDongVien
            // 
            this.lblDongVien.BackColor = System.Drawing.Color.Transparent;
            this.lblDongVien.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDongVien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblDongVien.Location = new System.Drawing.Point(11, 86);
            this.lblDongVien.Name = "lblDongVien";
            this.lblDongVien.Size = new System.Drawing.Size(1025, 25);
            this.lblDongVien.TabIndex = 3;
            this.lblDongVien.Text = "💪 Câu động viên...";
            // 
            // dgvGoiY
            // 
            this.dgvGoiY.AllowUserToAddRows = false;
            this.dgvGoiY.AllowUserToDeleteRows = false;
            this.dgvGoiY.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGoiY.BackgroundColor = System.Drawing.Color.White;
            this.dgvGoiY.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGoiY.Location = new System.Drawing.Point(27, 400);
            this.dgvGoiY.Name = "dgvGoiY";
            this.dgvGoiY.ReadOnly = true;
            this.dgvGoiY.RowHeadersVisible = false;
            this.dgvGoiY.RowHeadersWidth = 51;
            this.dgvGoiY.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGoiY.Size = new System.Drawing.Size(1050, 300);
            this.dgvGoiY.TabIndex = 5;
            this.dgvGoiY.Visible = false;
            this.dgvGoiY.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvGoiY.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvGoiY.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvGoiY.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvGoiY.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvGoiY.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvGoiY.ThemeStyle.GridColor = System.Drawing.Color.White;
            this.dgvGoiY.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.dgvGoiY.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvGoiY.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dgvGoiY.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvGoiY.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGoiY.ThemeStyle.HeaderStyle.Height = 35;
            this.dgvGoiY.ThemeStyle.ReadOnly = true;
            this.dgvGoiY.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvGoiY.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvGoiY.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dgvGoiY.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvGoiY.ThemeStyle.RowsStyle.Height = 35;
            this.dgvGoiY.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvGoiY.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvGoiY.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGoiY_CellDoubleClick);
            // 
            // btnXemChiTiet
            // 
            this.btnXemChiTiet.BackColor = System.Drawing.Color.Transparent;
            this.btnXemChiTiet.BorderRadius = 8;
            this.btnXemChiTiet.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnXemChiTiet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXemChiTiet.ForeColor = System.Drawing.Color.White;
            this.btnXemChiTiet.Location = new System.Drawing.Point(27, 710);
            this.btnXemChiTiet.Name = "btnXemChiTiet";
            this.btnXemChiTiet.Size = new System.Drawing.Size(180, 40);
            this.btnXemChiTiet.TabIndex = 6;
            this.btnXemChiTiet.Text = "📖 Xem Chi Tiết Sách";
            this.btnXemChiTiet.Visible = false;
            this.btnXemChiTiet.Click += new System.EventHandler(this.btnXemChiTiet_Click);
            // 
            // btnLichSu
            // 
            this.btnLichSu.BackColor = System.Drawing.Color.Transparent;
            this.btnLichSu.BorderRadius = 8;
            this.btnLichSu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnLichSu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLichSu.ForeColor = System.Drawing.Color.White;
            this.btnLichSu.Location = new System.Drawing.Point(220, 710);
            this.btnLichSu.Name = "btnLichSu";
            this.btnLichSu.Size = new System.Drawing.Size(180, 40);
            this.btnLichSu.TabIndex = 7;
            this.btnLichSu.Text = "📊 Lịch Sử Cảm Xúc";
            this.btnLichSu.Click += new System.EventHandler(this.btnLichSu_Click);
            // 
            // lblDangXuLy
            // 
            this.lblDangXuLy.AutoSize = true;
            this.lblDangXuLy.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Italic);
            this.lblDangXuLy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblDangXuLy.Location = new System.Drawing.Point(400, 180);
            this.lblDangXuLy.Name = "lblDangXuLy";
            this.lblDangXuLy.Size = new System.Drawing.Size(300, 20);
            this.lblDangXuLy.TabIndex = 8;
            this.lblDangXuLy.Text = "⏳ Đang phân tích cảm xúc và tìm sách phù hợp...";
            this.lblDangXuLy.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(400, 210);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(300, 23);
            this.progressBar.TabIndex = 9;
            this.progressBar.Visible = false;
            this.progressBar.BorderRadius = 10;
            this.progressBar.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.progressBar.ProgressColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // pnlVidu
            // 
            this.pnlVidu.Controls.Add(this.btnViDu3);
            this.pnlVidu.Controls.Add(this.btnViDu2);
            this.pnlVidu.Controls.Add(this.btnViDu1);
            this.pnlVidu.Controls.Add(this.lblViDu);
            this.pnlVidu.Location = new System.Drawing.Point(27, 180);
            this.pnlVidu.Name = "pnlVidu";
            this.pnlVidu.Size = new System.Drawing.Size(1050, 80);
            this.pnlVidu.TabIndex = 10;
            // 
            // lblViDu
            // 
            this.lblViDu.AutoSize = true;
            this.lblViDu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblViDu.ForeColor = System.Drawing.Color.Gray;
            this.lblViDu.Location = new System.Drawing.Point(5, 5);
            this.lblViDu.Name = "lblViDu";
            this.lblViDu.Size = new System.Drawing.Size(160, 15);
            this.lblViDu.TabIndex = 0;
            this.lblViDu.Text = "💡 Hoặc thử các ví dụ sau:";
            // 
            // btnViDu1
            // 
            this.btnViDu1.BackColor = System.Drawing.Color.Transparent;
            this.btnViDu1.BorderRadius = 8;
            this.btnViDu1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnViDu1.ForeColor = System.Drawing.Color.Black;
            this.btnViDu1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnViDu1.Location = new System.Drawing.Point(8, 30);
            this.btnViDu1.Name = "btnViDu1";
            this.btnViDu1.Size = new System.Drawing.Size(330, 40);
            this.btnViDu1.TabIndex = 1;
            this.btnViDu1.Text = "😢 \"Hôm nay tôi buồn quá, muốn tìm sách để vơi đi\"";
            this.btnViDu1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnViDu1.Click += new System.EventHandler(this.btnViDu1_Click);
            // 
            // btnViDu2
            // 
            this.btnViDu2.BackColor = System.Drawing.Color.Transparent;
            this.btnViDu2.BorderRadius = 8;
            this.btnViDu2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnViDu2.ForeColor = System.Drawing.Color.Black;
            this.btnViDu2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnViDu2.Location = new System.Drawing.Point(350, 30);
            this.btnViDu2.Name = "btnViDu2";
            this.btnViDu2.Size = new System.Drawing.Size(330, 40);
            this.btnViDu2.TabIndex = 2;
            this.btnViDu2.Text = "😊 \"Tôi đang rất vui và muốn đọc gì đó tích cực\"";
            this.btnViDu2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnViDu2.Click += new System.EventHandler(this.btnViDu2_Click);
            // 
            // btnViDu3
            // 
            this.btnViDu3.BackColor = System.Drawing.Color.Transparent;
            this.btnViDu3.BorderRadius = 8;
            this.btnViDu3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnViDu3.ForeColor = System.Drawing.Color.Black;
            this.btnViDu3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnViDu3.Location = new System.Drawing.Point(692, 30);
            this.btnViDu3.Name = "btnViDu3";
            this.btnViDu3.Size = new System.Drawing.Size(350, 40);
            this.btnViDu3.TabIndex = 3;
            this.btnViDu3.Text = "😰 \"Lo lắng về tương lai, không biết phải làm gì\"";
            this.btnViDu3.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnViDu3.Click += new System.EventHandler(this.btnViDu3_Click);
            // 
            // frmGoiYSachTheoEmocao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1100, 770);
            this.Controls.Add(this.pnlVidu);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblDangXuLy);
            this.Controls.Add(this.btnLichSu);
            this.Controls.Add(this.btnXemChiTiet);
            this.Controls.Add(this.dgvGoiY);
            this.Controls.Add(this.pnlKetQua);
            this.Controls.Add(this.btnPhanTich);
            this.Controls.Add(this.txtTrangThai);
            this.Controls.Add(this.lblHuongDan);
            this.Controls.Add(this.lblTieuDe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmGoiYSachTheoEmocao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gợi Ý Sách Theo Cảm Xúc - Thư Viện";
            this.pnlKetQua.ResumeLayout(false);
            this.pnlKetQua.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoiY)).EndInit();
            this.pnlVidu.ResumeLayout(false);
            this.pnlVidu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblHuongDan;
        private Guna.UI2.WinForms.Guna2TextBox txtTrangThai;
        private Guna.UI2.WinForms.Guna2Button btnPhanTich;
        private Guna.UI2.WinForms.Guna2Panel pnlKetQua;
        private System.Windows.Forms.Label lblDongVien;
        private System.Windows.Forms.Label lblLyDo;
        private System.Windows.Forms.Label lblDoTinCay;
        private System.Windows.Forms.Label lblCamXuc;
        private Guna.UI2.WinForms.Guna2DataGridView dgvGoiY;
        private Guna.UI2.WinForms.Guna2Button btnXemChiTiet;
        private Guna.UI2.WinForms.Guna2Button btnLichSu;
        private System.Windows.Forms.Label lblDangXuLy;
        private Guna.UI2.WinForms.Guna2ProgressBar progressBar;
        private System.Windows.Forms.Panel pnlVidu;
        private Guna.UI2.WinForms.Guna2Button btnViDu3;
        private Guna.UI2.WinForms.Guna2Button btnViDu2;
        private Guna.UI2.WinForms.Guna2Button btnViDu1;
        private System.Windows.Forms.Label lblViDu;
    }
}
