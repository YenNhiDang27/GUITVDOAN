namespace ThuVien.View
{
    partial class frmGoiYSach
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvGoiY = new System.Windows.Forms.DataGridView();
            this.btnTaoGoiYMoi = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblTongSo = new System.Windows.Forms.Label();
            this.lblThongBao = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoiY)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvGoiY
            // 
            this.dgvGoiY.AllowUserToAddRows = false;
            this.dgvGoiY.AllowUserToDeleteRows = false;
            this.dgvGoiY.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGoiY.BackgroundColor = System.Drawing.Color.White;
            this.dgvGoiY.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGoiY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGoiY.Location = new System.Drawing.Point(0, 120);
            this.dgvGoiY.MultiSelect = false;
            this.dgvGoiY.Name = "dgvGoiY";
            this.dgvGoiY.ReadOnly = true;
            this.dgvGoiY.RowHeadersWidth = 51;
            this.dgvGoiY.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGoiY.Size = new System.Drawing.Size(1200, 530);
            this.dgvGoiY.TabIndex = 0;
            this.dgvGoiY.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGoiY_CellDoubleClick);
            this.dgvGoiY.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvGoiY_CellFormatting);
            // 
            // btnTaoGoiYMoi
            // 
            this.btnTaoGoiYMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnTaoGoiYMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTaoGoiYMoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTaoGoiYMoi.ForeColor = System.Drawing.Color.White;
            this.btnTaoGoiYMoi.Location = new System.Drawing.Point(20, 15);
            this.btnTaoGoiYMoi.Name = "btnTaoGoiYMoi";
            this.btnTaoGoiYMoi.Size = new System.Drawing.Size(180, 45);
            this.btnTaoGoiYMoi.TabIndex = 1;
            this.btnTaoGoiYMoi.Text = "🔄 Tạo Gợi Ý Mới";
            this.btnTaoGoiYMoi.UseVisualStyleBackColor = false;
            this.btnTaoGoiYMoi.Click += new System.EventHandler(this.btnTaoGoiYMoi_Click);
            // 
            // btnDong
            // 
            this.btnDong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDong.ForeColor = System.Drawing.Color.White;
            this.btnDong.Location = new System.Drawing.Point(1000, 15);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(180, 45);
            this.btnDong.TabIndex = 2;
            this.btnDong.Text = "❌ Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.lblTieuDe.Location = new System.Drawing.Point(20, 20);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(428, 41);
            this.lblTieuDe.TabIndex = 3;
            this.lblTieuDe.Text = "📚 Gợi Ý Sách Cho Bạn";
            // 
            // lblTongSo
            // 
            this.lblTongSo.AutoSize = true;
            this.lblTongSo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTongSo.Location = new System.Drawing.Point(20, 70);
            this.lblTongSo.Name = "lblTongSo";
            this.lblTongSo.Size = new System.Drawing.Size(133, 23);
            this.lblTongSo.TabIndex = 4;
            this.lblTongSo.Text = "Tổng số gợi ý: 0";
            // 
            // lblThongBao
            // 
            this.lblThongBao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblThongBao.AutoSize = true;
            this.lblThongBao.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblThongBao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.lblThongBao.Location = new System.Drawing.Point(900, 70);
            this.lblThongBao.Name = "lblThongBao";
            this.lblThongBao.Size = new System.Drawing.Size(280, 23);
            this.lblThongBao.TabIndex = 5;
            this.lblThongBao.Text = "Đang tải dữ liệu, vui lòng đợi...";
            this.lblThongBao.Visible = false;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelTop.Controls.Add(this.lblTieuDe);
            this.panelTop.Controls.Add(this.lblTongSo);
            this.panelTop.Controls.Add(this.lblThongBao);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1200, 120);
            this.panelTop.TabIndex = 6;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelBottom.Controls.Add(this.btnTaoGoiYMoi);
            this.panelBottom.Controls.Add(this.btnDong);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 650);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1200, 75);
            this.panelBottom.TabIndex = 7;
            // 
            // frmGoiYSach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 725);
            this.Controls.Add(this.dgvGoiY);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "frmGoiYSach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Gợi Ý Sách Thông Minh - AI Recommendation";
            this.Load += new System.EventHandler(this.frmGoiYSach_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoiY)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvGoiY;
        private System.Windows.Forms.Button btnTaoGoiYMoi;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblTongSo;
        private System.Windows.Forms.Label lblThongBao;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
    }
}
