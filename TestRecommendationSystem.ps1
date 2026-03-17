# =============================================
# Script: Test Hệ Thống Gợi Ý Sách
# Mô tả: Chạy các test cases cho Recommendation System
# =============================================

Write-Host "╔══════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║    TEST HỆ THỐNG GỢI Ý SÁCH THÔNG MINH       ║" -ForegroundColor Cyan
Write-Host "╚══════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# Kiểm tra file Test có tồn tại không
$testFile = "Tests\RecommendationSystemTest.cs"
if (-not (Test-Path $testFile)) {
    Write-Host "❌ Không tìm thấy file test: $testFile" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Tìm thấy file test" -ForegroundColor Green
Write-Host ""

# Build project
Write-Host "🔨 Đang build project..." -ForegroundColor Yellow
dotnet build --configuration Debug

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build thất bại!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Build thành công!" -ForegroundColor Green
Write-Host ""

# Chạy application với test mode
Write-Host "🚀 Đang chạy tests..." -ForegroundColor Yellow
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray
Write-Host ""

# Note: Bạn cần tạo một entry point để chạy test
# Hoặc thêm test vào Program.cs
Write-Host "⚠️  Để chạy test, hãy thêm code sau vào Program.cs:" -ForegroundColor Cyan
Write-Host ""
Write-Host "// Uncomment để chạy test" -ForegroundColor Gray
Write-Host "var test = new ThuVien.Tests.RecommendationSystemTest();" -ForegroundColor White
Write-Host "test.RunAllTests();" -ForegroundColor White
Write-Host "return; // Comment dòng này để chạy app bình thường" -ForegroundColor Gray
Write-Host ""

# Thống kê file
Write-Host "📊 THỐNG KÊ FILES ĐÃ TẠO:" -ForegroundColor Cyan
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Gray

$files = @(
    "Services\RecommendationService.cs",
    "Controller\GoiYSachController.cs",
    "View\frmGoiYSach.cs",
    "View\frmGoiYSach.Designer.cs",
    "Database\CreateGoiYSachTable.sql",
    "Tests\RecommendationSystemTest.cs",
    "README_RECOMMENDATION_SYSTEM.md"
)

foreach ($file in $files) {
    if (Test-Path $file) {
        $size = (Get-Item $file).Length
        $sizeKB = [math]::Round($size / 1KB, 2)
        Write-Host "  ✅ $file ($sizeKB KB)" -ForegroundColor Green
    } else {
        Write-Host "  ❌ $file (Không tìm thấy)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "╔══════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║           HƯỚNG DẪN SỬ DỤNG                  ║" -ForegroundColor Cyan
Write-Host "╚══════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""
Write-Host "1️⃣  Chạy script SQL:" -ForegroundColor Yellow
Write-Host "    Database\CreateGoiYSachTable.sql" -ForegroundColor White
Write-Host ""
Write-Host "2️⃣  Thêm menu vào frmMain:" -ForegroundColor Yellow
Write-Host "    - Menu: Chức Năng > Gợi Ý Sách Cho Bạn" -ForegroundColor White
Write-Host "    - Event: new frmGoiYSach(maBanDoc).ShowDialog()" -ForegroundColor White
Write-Host ""
Write-Host "3️⃣  Test hệ thống:" -ForegroundColor Yellow
Write-Host "    - Uncomment code test trong Program.cs" -ForegroundColor White
Write-Host "    - Hoặc gọi từ menu Debug" -ForegroundColor White
Write-Host ""
Write-Host "4️⃣  Đọc tài liệu:" -ForegroundColor Yellow
Write-Host "    README_RECOMMENDATION_SYSTEM.md" -ForegroundColor White
Write-Host ""

Write-Host "🎉 Hoàn tất! Chúc bạn thành công!" -ForegroundColor Green
Write-Host ""
