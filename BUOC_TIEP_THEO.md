# 🎯 HƯỚNG DẪN THÊM MENU "GỢI Ý THEO CẢM XÚC"

## ✅ ĐÃ HOÀN THÀNH
- ✅ Đã thêm method `btnGoiYSachTheoEmocao_Click` vào `frmMain.cs`

## 📝 CẦN LÀM TIẾP (Chọn 1 trong 3 cách)

---

## CÁCH 1: Thêm Menu bằng Visual Studio Designer (Dễ nhất - Khuyến nghị)

### Bước 1: Mở frmMain trong Design Mode
1. Trong **Solution Explorer**, tìm `View/frmMain.cs`
2. **Click phải** → Chọn **"View Designer"** (hoặc nhấn **Shift+F7**)

### Bước 2: Tìm MenuStrip
1. Trong form designer, tìm thanh menu ở phía trên
2. Click vào menu cha (có thể là "Chức năng", "Dịch vụ", hoặc tương tự)

### Bước 3: Thêm Menu Item Mới
1. Click vào vị trí muốn thêm menu (bên dưới "Gợi Ý Sách Cho Bạn" hiện có)
2. Gõ text: **💙 Gợi Ý Theo Cảm Xúc**
3. Nhấn **Enter**

### Bước 4: Đặt tên cho Menu Item
1. Menu vừa tạo đang được chọn
2. Nhìn sang **Properties** (bên phải màn hình)
3. Tìm thuộc tính **(Name)**
4. Đổi thành: **goiYTheoEmocaoToolStripMenuItem**

### Bước 5: Gắn Event Handler
1. Menu item vẫn đang được chọn
2. Trong **Properties**, click vào icon ⚡ (Events)
3. Tìm event **Click**
4. Trong dropdown, chọn **btnGoiYSachTheoEmocao_Click** (method vừa tạo)

### Bước 6: Lưu và Test
1. Nhấn **Ctrl+S** để lưu
2. Nhấn **F5** để chạy
3. Đăng nhập với tài khoản **Bạn đọc**
4. Tìm menu **"💙 Gợi Ý Theo Cảm Xúc"**
5. Click và test!

---

## CÁCH 2: Thêm Button Riêng (Nếu không muốn dùng menu)

### Bước 1: Mở frmMain trong Design Mode
1. **Solution Explorer** → `View/frmMain.cs`
2. **Click phải** → **"View Designer"**

### Bước 2: Kéo Button vào Form
1. Trong **Toolbox** (bên trái), tìm **Button**
2. Kéo thả vào vị trí muốn đặt trên form

### Bước 3: Cấu hình Button
Trong **Properties**:
- **(Name)**: `btnGoiYTheoEmocao`
- **Text**: `💙 Gợi Ý Sách Theo Cảm Xúc`
- **Font**: Segoe UI, 11pt, Bold
- **BackColor**: 0, 122, 204 (màu xanh đẹp)
- **ForeColor**: White
- **Size**: 200, 50

### Bước 4: Gắn Event
1. **Double-click** vào button
2. Nó sẽ tạo method `btnGoiYTheoEmocao_Click` tự động
3. **XOÁ** method vừa tạo (vì đã có rồi)
4. Hoặc gắn event thủ công như Cách 1

### Bước 5: Kiểm soát hiển thị
Trong `frmMain_Load()`, thêm:

```csharp
if (loaiNguoiDung == "Admin")
{
    btnGoiYTheoEmocao.Visible = false; // Admin không thấy
}
else
{
    btnGoiYTheoEmocao.Visible = true;  // Bạn đọc thấy
}
```

---

## CÁCH 3: Code thủ công trong Designer.cs (Nâng cao)

Nếu 2 cách trên không được, làm thủ công:

### Bước 1: Mở frmMain.Designer.cs
**Solution Explorer** → `frmMain.cs` → Click mũi tên ▶ → `frmMain.Designer.cs`

### Bước 2: Tìm phần khai báo components
Tìm đoạn code:
```csharp
private System.Windows.Forms.ToolStripMenuItem goiYSachChobanToolStripMenuItem;
```

Thêm ngay sau:
```csharp
private System.Windows.Forms.ToolStripMenuItem goiYTheoEmocaoToolStripMenuItem;
```

### Bước 3: Tìm InitializeComponent()
Tìm dòng:
```csharp
this.goiYSachChobanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
```

Thêm ngay sau:
```csharp
this.goiYTheoEmocaoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
```

### Bước 4: Cấu hình menu item
Tìm đoạn cấu hình `goiYSachChobanToolStripMenuItem`, thêm sau đó:

```csharp
// 
// goiYTheoEmocaoToolStripMenuItem
// 
this.goiYTheoEmocaoToolStripMenuItem.Name = "goiYTheoEmocaoToolStripMenuItem";
this.goiYTheoEmocaoToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
this.goiYTheoEmocaoToolStripMenuItem.Text = "💙 Gợi Ý Theo Cảm Xúc";
this.goiYTheoEmocaoToolStripMenuItem.Click += new System.EventHandler(this.btnGoiYSachTheoEmocao_Click);
```

### Bước 5: Thêm vào menu cha
Tìm đoạn code thêm items vào menu (có thể là `chứcNăngToolStripMenuItem` hoặc tương tự):

```csharp
this.chứcNăngToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
    // ... các items khác ...
    this.goiYSachChobanToolStripMenuItem,
    this.goiYTheoEmocaoToolStripMenuItem  // ← THÊM DÒNG NÀY
});
```

### Bước 6: Kiểm soát hiển thị trong frmMain_Load
Trong file `frmMain.cs`, tìm `frmMain_Load()`, thêm:

```csharp
if (loaiNguoiDung == "Admin")
{
    goiYTheoEmocaoToolStripMenuItem.Visible = false;
}
else
{
    goiYTheoEmocaoToolStripMenuItem.Visible = true;
}
```

---

## ✅ KIỂM TRA SAU KHI HOÀN THÀNH

1. **Build project**: `Ctrl+Shift+B`
2. Không có lỗi compile ✅
3. Chạy ứng dụng: `F5`
4. Đăng nhập với tài khoản **Bạn Đọc**
5. Tìm menu hoặc button **"💙 Gợi Ý Theo Cảm Xúc"**
6. Click vào
7. Form gợi ý sách theo cảm xúc hiện ra ✅
8. Nhập trạng thái: "Hôm nay tôi buồn"
9. Nhấn "Phân Tích"
10. Kết quả hiển thị đúng ✅

---

## 🐛 NẾU GẶP LỖI

### Lỗi: "The name 'frmGoiYSachTheoEmocao' does not exist"
→ **Build lại project**: `Ctrl+Shift+B`

### Lỗi: "Object reference not set to an instance"
→ Kiểm tra `TaiKhoan.DangNhapHienTai` có null không

### Menu không hiện
→ Kiểm tra `Visible = true` trong `frmMain_Load()`

### Lỗi: "ShowFormInPanel does not exist"
→ Thay `ShowFormInPanel(frm)` bằng `frm.ShowDialog()`

---

## 💡 LỰA CHỌN CỦA TÔI

**Khuyến nghị dùng CÁCH 1** (Visual Studio Designer) vì:
- ✅ Dễ nhất, trực quan
- ✅ Không phải viết code thủ công
- ✅ Ít lỗi hơn
- ✅ Visual Studio tự động generate code

---

**Hãy chọn cách phù hợp với bạn và bắt đầu! 🚀**
