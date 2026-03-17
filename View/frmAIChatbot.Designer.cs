namespace ThuVien.View
{
    partial class frmAIChatbot
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
            panelHeader = new Panel();
            lblTitle = new Label();
            btnClose = new Button();
            panelChat = new Panel();
            rtbChat = new RichTextBox();
            panelInput = new Panel();
            txtInput = new TextBox();
            btnSend = new Button();
            btnGoiY = new Button();
            btnXuHuong = new Button();
            panelLoading = new Panel();
            lblLoading = new Label();
            panelHeader.SuspendLayout();
            panelChat.SuspendLayout();
            panelInput.SuspendLayout();
            panelLoading.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(0, 122, 204);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(btnClose);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(3, 4, 3, 4);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(914, 80);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(23, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(290, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🤖 Trợ lý AI Thư Viện";
            lblTitle.Click += lblTitle_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.FromArgb(192, 0, 0);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(846, 13);
            btnClose.Margin = new Padding(3, 4, 3, 4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(57, 53);
            btnClose.TabIndex = 1;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // panelChat
            // 
            panelChat.Controls.Add(rtbChat);
            panelChat.Dock = DockStyle.Fill;
            panelChat.Location = new Point(0, 80);
            panelChat.Margin = new Padding(3, 4, 3, 4);
            panelChat.Name = "panelChat";
            panelChat.Padding = new Padding(11, 13, 11, 13);
            panelChat.Size = new Size(914, 587);
            panelChat.TabIndex = 1;
            // 
            // rtbChat
            // 
            rtbChat.BackColor = Color.White;
            rtbChat.BorderStyle = BorderStyle.None;
            rtbChat.Dock = DockStyle.Fill;
            rtbChat.Font = new Font("Segoe UI", 10F);
            rtbChat.Location = new Point(11, 13);
            rtbChat.Margin = new Padding(3, 4, 3, 4);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.Size = new Size(892, 561);
            rtbChat.TabIndex = 0;
            rtbChat.Text = "";
            // 
            // panelInput
            // 
            panelInput.Controls.Add(txtInput);
            panelInput.Controls.Add(btnSend);
            panelInput.Controls.Add(btnGoiY);
            panelInput.Controls.Add(btnXuHuong);
            panelInput.Dock = DockStyle.Bottom;
            panelInput.Location = new Point(0, 667);
            panelInput.Margin = new Padding(3, 4, 3, 4);
            panelInput.Name = "panelInput";
            panelInput.Padding = new Padding(11, 13, 11, 13);
            panelInput.Size = new Size(914, 133);
            panelInput.TabIndex = 2;
            // 
            // txtInput
            // 
            txtInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtInput.Font = new Font("Segoe UI", 11F);
            txtInput.Location = new Point(15, 73);
            txtInput.Margin = new Padding(3, 4, 3, 4);
            txtInput.Multiline = true;
            txtInput.Name = "txtInput";
            txtInput.PlaceholderText = "Nhập câu hỏi của bạn... (VD: Gợi ý sách trinh thám hay)";
            txtInput.Size = new Size(747, 45);
            txtInput.TabIndex = 0;
            txtInput.KeyDown += txtInput_KeyDown;
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSend.BackColor = Color.FromArgb(0, 122, 204);
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(769, 73);
            btnSend.Margin = new Padding(3, 4, 3, 4);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(131, 47);
            btnSend.TabIndex = 1;
            btnSend.Text = "📤 Gửi";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // btnGoiY
            // 
            btnGoiY.BackColor = Color.FromArgb(46, 204, 113);
            btnGoiY.FlatStyle = FlatStyle.Flat;
            btnGoiY.Font = new Font("Segoe UI", 9F);
            btnGoiY.ForeColor = Color.White;
            btnGoiY.Location = new Point(15, 17);
            btnGoiY.Margin = new Padding(3, 4, 3, 4);
            btnGoiY.Name = "btnGoiY";
            btnGoiY.Size = new Size(171, 47);
            btnGoiY.TabIndex = 2;
            btnGoiY.Text = "💡 Gợi ý cho tôi";
            btnGoiY.UseVisualStyleBackColor = false;
            btnGoiY.Click += btnGoiY_Click;
            // 
            // btnXuHuong
            // 
            btnXuHuong.BackColor = Color.FromArgb(230, 126, 34);
            btnXuHuong.FlatStyle = FlatStyle.Flat;
            btnXuHuong.Font = new Font("Segoe UI", 9F);
            btnXuHuong.ForeColor = Color.White;
            btnXuHuong.Location = new Point(193, 17);
            btnXuHuong.Margin = new Padding(3, 4, 3, 4);
            btnXuHuong.Name = "btnXuHuong";
            btnXuHuong.Size = new Size(171, 47);
            btnXuHuong.TabIndex = 3;
            btnXuHuong.Text = "📊 Xu hướng";
            btnXuHuong.UseVisualStyleBackColor = false;
            btnXuHuong.Click += btnXuHuong_Click;
            // 
            // panelLoading
            // 
            panelLoading.BackColor = Color.FromArgb(240, 240, 240);
            panelLoading.Controls.Add(lblLoading);
            panelLoading.Location = new Point(286, 333);
            panelLoading.Margin = new Padding(3, 4, 3, 4);
            panelLoading.Name = "panelLoading";
            panelLoading.Size = new Size(343, 133);
            panelLoading.TabIndex = 3;
            panelLoading.Visible = false;
            // 
            // lblLoading
            // 
            lblLoading.Dock = DockStyle.Fill;
            lblLoading.Font = new Font("Segoe UI", 12F);
            lblLoading.Location = new Point(0, 0);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(343, 133);
            lblLoading.TabIndex = 0;
            lblLoading.Text = "⏳ Đang suy nghĩ...";
            lblLoading.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmAIChatbot
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(914, 800);
            Controls.Add(panelLoading);
            Controls.Add(panelChat);
            Controls.Add(panelInput);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmAIChatbot";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AI Chatbot - Thư Viện";
            Load += frmAIChatbot_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelChat.ResumeLayout(false);
            panelInput.ResumeLayout(false);
            panelInput.PerformLayout();
            panelLoading.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelChat;
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnGoiY;
        private System.Windows.Forms.Button btnXuHuong;
        private System.Windows.Forms.Panel panelLoading;
        private System.Windows.Forms.Label lblLoading;
    }
}
