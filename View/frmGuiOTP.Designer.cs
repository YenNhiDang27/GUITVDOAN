namespace GUITV
{
    partial class frmGuiOTP
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGuiOTP = new System.Windows.Forms.TabPage();
            this.btnBoChonTatCa = new System.Windows.Forms.Button();
            this.btnChonTatCa = new System.Windows.Forms.Button();
            this.btnGuiOTP = new System.Windows.Forms.Button();
            this.clbBanDoc = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabXacThuc = new System.Windows.Forms.TabPage();
            this.btnXacThuc = new System.Windows.Forms.Button();
            this.txtOTP = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabGuiOTP.SuspendLayout();
            this.tabXacThuc.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGuiOTP);
            this.tabControl1.Controls.Add(this.tabXacThuc);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(700, 550);
            this.tabControl1.TabIndex = 0;
            // 
            // tabGuiOTP
            // 
            this.tabGuiOTP.Controls.Add(this.btnBoChonTatCa);
            this.tabGuiOTP.Controls.Add(this.btnChonTatCa);
            this.tabGuiOTP.Controls.Add(this.btnGuiOTP);
            this.tabGuiOTP.Controls.Add(this.clbBanDoc);
            this.tabGuiOTP.Controls.Add(this.label1);
            this.tabGuiOTP.Location = new System.Drawing.Point(4, 32);
            this.tabGuiOTP.Name = "tabGuiOTP";
            this.tabGuiOTP.Padding = new System.Windows.Forms.Padding(3);
            this.tabGuiOTP.Size = new System.Drawing.Size(692, 514);
            this.tabGuiOTP.TabIndex = 0;
            this.tabGuiOTP.Text = "Gửi OTP";
            this.tabGuiOTP.UseVisualStyleBackColor = true;
            // 
            // btnBoChonTatCa
            // 
            this.btnBoChonTatCa.Location = new System.Drawing.Point(360, 460);
            this.btnBoChonTatCa.Name = "btnBoChonTatCa";
            this.btnBoChonTatCa.Size = new System.Drawing.Size(150, 40);
            this.btnBoChonTatCa.TabIndex = 4;
            this.btnBoChonTatCa.Text = "Bỏ Chọn Tất Cả";
            this.btnBoChonTatCa.UseVisualStyleBackColor = true;
            this.btnBoChonTatCa.Click += new System.EventHandler(this.btnBoChonTatCa_Click);
            // 
            // btnChonTatCa
            // 
            this.btnChonTatCa.Location = new System.Drawing.Point(180, 460);
            this.btnChonTatCa.Name = "btnChonTatCa";
            this.btnChonTatCa.Size = new System.Drawing.Size(150, 40);
            this.btnChonTatCa.TabIndex = 3;
            this.btnChonTatCa.Text = "Chọn Tất Cả";
            this.btnChonTatCa.UseVisualStyleBackColor = true;
            this.btnChonTatCa.Click += new System.EventHandler(this.btnChonTatCa_Click);
            // 
            // btnGuiOTP
            // 
            this.btnGuiOTP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnGuiOTP.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnGuiOTP.ForeColor = System.Drawing.Color.White;
            this.btnGuiOTP.Location = new System.Drawing.Point(540, 460);
            this.btnGuiOTP.Name = "btnGuiOTP";
            this.btnGuiOTP.Size = new System.Drawing.Size(130, 40);
            this.btnGuiOTP.TabIndex = 2;
            this.btnGuiOTP.Text = "Gửi OTP";
            this.btnGuiOTP.UseVisualStyleBackColor = false;
            this.btnGuiOTP.Click += new System.EventHandler(this.btnGuiOTP_Click);
            // 
            // clbBanDoc
            // 
            this.clbBanDoc.CheckOnClick = true;
            this.clbBanDoc.FormattingEnabled = true;
            this.clbBanDoc.Location = new System.Drawing.Point(20, 60);
            this.clbBanDoc.Name = "clbBanDoc";
            this.clbBanDoc.Size = new System.Drawing.Size(650, 380);
            this.clbBanDoc.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(308, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chọn Bạn Đọc Để Gửi Mã OTP";
            // 
            // tabXacThuc
            // 
            this.tabXacThuc.Controls.Add(this.btnXacThuc);
            this.tabXacThuc.Controls.Add(this.txtOTP);
            this.tabXacThuc.Controls.Add(this.txtEmail);
            this.tabXacThuc.Controls.Add(this.label3);
            this.tabXacThuc.Controls.Add(this.label2);
            this.tabXacThuc.Location = new System.Drawing.Point(4, 32);
            this.tabXacThuc.Name = "tabXacThuc";
            this.tabXacThuc.Padding = new System.Windows.Forms.Padding(3);
            this.tabXacThuc.Size = new System.Drawing.Size(692, 514);
            this.tabXacThuc.TabIndex = 1;
            this.tabXacThuc.Text = "Xác Thực OTP";
            this.tabXacThuc.UseVisualStyleBackColor = true;
            // 
            // btnXacThuc
            // 
            this.btnXacThuc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnXacThuc.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnXacThuc.ForeColor = System.Drawing.Color.White;
            this.btnXacThuc.Location = new System.Drawing.Point(250, 250);
            this.btnXacThuc.Name = "btnXacThuc";
            this.btnXacThuc.Size = new System.Drawing.Size(200, 45);
            this.btnXacThuc.TabIndex = 4;
            this.btnXacThuc.Text = "Xác Thực";
            this.btnXacThuc.UseVisualStyleBackColor = false;
            this.btnXacThuc.Click += new System.EventHandler(this.btnXacThuc_Click);
            // 
            // txtOTP
            // 
            this.txtOTP.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtOTP.Location = new System.Drawing.Point(250, 180);
            this.txtOTP.MaxLength = 6;
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.Size = new System.Drawing.Size(300, 34);
            this.txtOTP.TabIndex = 3;
            this.txtOTP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtEmail.Location = new System.Drawing.Point(250, 100);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(300, 34);
            this.txtEmail.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label3.Location = new System.Drawing.Point(140, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "Mã OTP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label2.Location = new System.Drawing.Point(140, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Email:";
            // 
            // frmGuiOTP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 550);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmGuiOTP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Gửi OTP";
            this.Load += new System.EventHandler(this.frmGuiOTP_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabGuiOTP.ResumeLayout(false);
            this.tabGuiOTP.PerformLayout();
            this.tabXacThuc.ResumeLayout(false);
            this.tabXacThuc.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGuiOTP;
        private System.Windows.Forms.TabPage tabXacThuc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox clbBanDoc;
        private System.Windows.Forms.Button btnGuiOTP;
        private System.Windows.Forms.Button btnChonTatCa;
        private System.Windows.Forms.Button btnBoChonTatCa;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtOTP;
        private System.Windows.Forms.Button btnXacThuc;
    }
}