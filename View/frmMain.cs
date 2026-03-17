using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ThuVien.Utilies;
using ThuVien.Models;
using GUITV;

namespace ThuVien.View
{
    public partial class frmMain : Form
    {
        private string loaiNguoiDung;
        public frmMain(string loaiNguoiDung)
        {
            InitializeComponent();
            this.loaiNguoiDung = loaiNguoiDung;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (loaiNguoiDung == "Admin")
            {
                thôngTinTàiKhoảnToolStripMenuItem.Visible = false;
                tìmKiếmSáchToolStripMenuItem.Visible = false;
                lịchSửMượnSáchToolStripMenuItem.Visible = false;
                tàiKhoảntoolStripMenuItem2.Visible = true;
                sáchtoolStripMenuItem2.Visible = true;
                độcGiảtoolStripMenuItem2.Visible = true;
                phiếuMượntoolStripMenuItem2.Visible = true;
                tìnhTrạngMượnTrảSáchtoolStripMenuItem3.Visible = true;
                giớiThiệutoolStripMenuItem2.Visible = true;
                goiYSachChobanToolStripMenuItem.Visible = false;
                goiYTheoEmocaoToolStripMenuItem.Visible = false;  // Menu mới - chỉ cho bạn đọc
            }
            else // Bạn đọc
            {
                tàiKhoảntoolStripMenuItem2.Visible = false;
                sáchtoolStripMenuItem2.Visible = false;
                độcGiảtoolStripMenuItem2.Visible = false;
                phiếuMượntoolStripMenuItem2.Visible = false;
                thôngTinTàiKhoảnToolStripMenuItem.Visible = true;
                tìmKiếmSáchToolStripMenuItem.Visible = true;
                lịchSửMượnSáchToolStripMenuItem.Visible = true;
                tìnhTrạngMượnTrảSáchtoolStripMenuItem3.Visible = false;
                giớiThiệutoolStripMenuItem2.Visible = true;
                goiYSachChobanToolStripMenuItem.Visible = true;
                goiYTheoEmocaoToolStripMenuItem.Visible = true;   // Menu mới - hiển thị cho bạn đọc
            }
        }
        
                
          
      
        private void đăngXuấtoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Auth.DangXuat();

                this.Hide();
                frmDangNhap loginForm = new frmDangNhap();
                loginForm.ShowDialog(); // Mở lại form đăng nhập
                this.Close(); // Đóng frmMain sau khi đăng xuất
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AppState.DongFormTuFormDoiMatKhau)
            {

                Auth.DangXuat();
            }
            else
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?",
          "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    Auth.DangXuat();
                    Application.Exit();

                }
            }
        }

        private void pnlHienThi_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void ShowFormInPanel(Form frm)
        {
            pnlHienThi.Controls.Clear();
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            pnlHienThi.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void tàiKhoảntoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new frmQuanLyTaiKhoan());
        }

        private void sáchtoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new frmQuanLySach());
        }

        private void phiếuMượntoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new frmQuanLyPhieuMuon());
        }

        private void độcGiảtoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new frmQuanLyBanDoc());
        }

        private void tìnhTrạngMượnTrảSáchtoolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new frmThongKe());
        }

        private void đổiMậtKhâuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new frmDoiMatKhau(this));
        }

        private void tìmKiếmSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new frmTimKiemSach());
        }

        private void giớiThiệutoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new frmGioiThieu());
        }

        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new frmXemThongTinTaiKhoan());
        }

        public void lịchSửMượnTrảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new frmLichSuMuonSach());
        }

        private void thoáttoolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void goiYSachChobanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra người dùng đã đăng nhập
                if (TaiKhoan.DangNhapHienTai == null)
                {
                    MessageBox.Show("Vui lòng đăng nhập trước!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra loại người dùng (có thể là "BanDoc", "Bạn Đọc", "bandoc", v.v.)
                string loaiNguoiDung = TaiKhoan.DangNhapHienTai.LoaiNguoiDung?.ToLower() ?? "";
                bool laBanDoc = loaiNguoiDung.Contains("ban") || 
                               loaiNguoiDung.Contains("đọc") || 
                               loaiNguoiDung.Contains("doc") ||
                               loaiNguoiDung == "bandoc";

                // Debug: Hiển thị thông tin để kiểm tra
                MessageBox.Show($"LoaiNguoiDung: '{TaiKhoan.DangNhapHienTai.LoaiNguoiDung}'\nMaBanDoc: {TaiKhoan.DangNhapHienTai.MaBanDoc}", 
                    "DEBUG INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (!laBanDoc)
                {
                    MessageBox.Show("Chức năng này chỉ dành cho bạn đọc!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Kiểm tra MaBanDoc
                if (!TaiKhoan.DangNhapHienTai.MaBanDoc.HasValue)
                {
                    MessageBox.Show("Tài khoản của bạn chưa được liên kết với bạn đọc!\nVui lòng liên hệ quản trị viên.", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Mở form gợi ý sách
                int maBanDoc = TaiKhoan.DangNhapHienTai.MaBanDoc.Value;
                frmGoiYSach frmGoiY = new frmGoiYSach(maBanDoc);
                ShowFormInPanel(frmGoiY);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form gợi ý sách:\n{ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Mở form gợi ý sách theo cảm xúc (tính năng mới)
        /// </summary>
        private void btnGoiYSachTheoEmocao_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra người dùng đã đăng nhập
                if (TaiKhoan.DangNhapHienTai == null)
                {
                    MessageBox.Show("Vui lòng đăng nhập trước!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra MaBanDoc
                if (!TaiKhoan.DangNhapHienTai.MaBanDoc.HasValue)
                {
                    MessageBox.Show("Tài khoản của bạn chưa được liên kết với bạn đọc!\nVui lòng liên hệ quản trị viên.", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Mở form gợi ý sách theo cảm xúc
                int maBanDoc = TaiKhoan.DangNhapHienTai.MaBanDoc.Value;
                frmGoiYSachTheoEmocao frmEmocao = new frmGoiYSachTheoEmocao(maBanDoc);
                ShowFormInPanel(frmEmocao);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form gợi ý sách theo cảm xúc:\n{ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

