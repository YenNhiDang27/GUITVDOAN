# ============================================
# SCRIPT CHẠY FORM TEST EMAIL NHANH
# ============================================

Write-Host "🚀 HƯỚNG DẪN MỞ FORM TEST EMAIL" -ForegroundColor Cyan
Write-Host ""
Write-Host "Có 2 cách:" -ForegroundColor Yellow
Write-Host ""
Write-Host "📋 CÁCH 1: SỬA PROGRAM.CS (NHANH NHẤT)" -ForegroundColor Green
Write-Host "1. Mở file Program.cs" -ForegroundColor White
Write-Host "2. Tìm dòng: Application.Run(new frmDangNhap());" -ForegroundColor Gray
Write-Host "3. Thay bằng: Application.Run(new ThuVien.View.frmTestEmailNhanh());" -ForegroundColor Yellow
Write-Host "4. Nhấn F5 để chạy" -ForegroundColor White
Write-Host ""
Write-Host "📋 CÁCH 2: THÊM NÚT VÀO FORM CHÍNH" -ForegroundColor Green
Write-Host "Tôi đã tạo form test rồi, bạn chỉ cần mở từ menu hoặc nút" -ForegroundColor White
Write-Host ""
Write-Host "⚠️ QUAN TRỌNG: SAU KHI CHẠY XONG, REBUILD LẠI!" -ForegroundColor Red
Write-Host "Build → Rebuild Solution" -ForegroundColor Yellow
Write-Host ""
Read-Host "Nhấn Enter để tiếp tục"
