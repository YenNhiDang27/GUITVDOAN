# 🧠 GIẢI THÍCH THUẬT TOÁN MACHINE LEARNING

## 📚 Tổng Quan Hệ Thống Recommendation

Hệ thống gợi ý sách sử dụng kết hợp 2 phương pháp ML chính:
1. **Content-Based Filtering** (Lọc dựa trên nội dung)
2. **Collaborative Filtering** (Lọc cộng tác)

---

## 🎯 1. CONTENT-BASED FILTERING

### Nguyên lý:
> "Người dùng thích những gì họ đã từng thích trong quá khứ"

### Cách hoạt động:

#### Bước 1: Thu thập lịch sử
```
Bạn đọc A đã mượn:
├─ "Nhà Giả Kim" (Tiểu thuyết, Paulo Coelho)
├─ "Tuổi Trẻ Đáng Giá Bao Nhiêu" (Kỹ năng sống, Rosie Nguyễn)  
└─ "Đắc Nhân Tâm" (Kỹ năng sống, Dale Carnegie)
```

#### Bước 2: Phân tích sở thích
```
Thống kê thể loại:
├─ Kỹ năng sống: 2 lần (66.7%)
└─ Tiểu thuyết: 1 lần (33.3%)

Thống kê tác giả:
├─ Paulo Coelho: 1 lần
├─ Rosie Nguyễn: 1 lần
└─ Dale Carnegie: 1 lần
```

#### Bước 3: Tính điểm phù hợp

**Công thức:**
```
DiemPhuHop = (TiLeTheLoai × 0.6) + (TiLeTacGia × 0.4)
```

**Ví dụ tính toán:**

Sách gợi ý: **"7 Thói Quen Hiệu Quả"** (Kỹ năng sống, Stephen Covey)

```
Điểm thể loại:
- Kỹ năng sống = 66.7%
- Điểm = 0.667 × 0.6 = 0.400

Điểm tác giả:
- Stephen Covey chưa mượn bao giờ = 0%
- Điểm = 0.0 × 0.4 = 0.000

Tổng điểm = 0.400 + 0.000 = 0.400 (40%)
```

Sách gợi ý: **"Veronika Quyết Định Chết"** (Tiểu thuyết, Paulo Coelho)

```
Điểm thể loại:
- Tiểu thuyết = 33.3%
- Điểm = 0.333 × 0.6 = 0.200

Điểm tác giả:
- Paulo Coelho đã mượn 1/3 lần = 33.3%
- Điểm = 0.333 × 0.4 = 0.133

Tổng điểm = 0.200 + 0.133 = 0.333 (33%)
```

### Ưu điểm:
✅ Không cần dữ liệu người dùng khác  
✅ Giải thích được lý do gợi ý  
✅ Hoạt động tốt với người dùng mới có ít lịch sử

### Nhược điểm:
❌ Thiếu tính đa dạng (chỉ gợi ý sách tương tự)  
❌ Không khám phá được sở thích mới

---

## 👥 2. COLLABORATIVE FILTERING

### Nguyên lý:
> "Những người có sở thích giống nhau thường thích những thứ giống nhau"

### Cách hoạt động:

#### Bước 1: Tìm người dùng tương tự

**Sử dụng Jaccard Similarity:**

```
Công thức:
DoTuongDong = |A ∩ B| / |A ∪ B|

Trong đó:
- A: Tập sách bạn đọc A đã mượn
- B: Tập sách bạn đọc B đã mượn
- ∩: Giao (sách chung)
- ∪: Hợp (tổng sách không trùng)
```

**Ví dụ thực tế:**

```
Bạn đọc A đã mượn: {1, 2, 3, 4, 5}
Bạn đọc B đã mượn: {3, 4, 5, 6, 7}

Giao: {3, 4, 5} → 3 cuốn
Hợp: {1, 2, 3, 4, 5, 6, 7} → 7 cuốn

Độ tương đồng = 3/7 = 0.428 (42.8%)
```

#### Bước 2: Lấy top độc giả tương tự

```
Kết quả phân tích:
├─ Bạn đọc B: 42.8% tương đồng
├─ Bạn đọc C: 35.7% tương đồng
├─ Bạn đọc D: 28.5% tương đồng
└─ Bạn đọc E: 21.4% tương đồng

→ Chọn top 5 độc giả tương tự
```

#### Bước 3: Gợi ý từ những người tương tự

```
Bạn đọc B mượn: {3, 4, 5, 6, 7}
Bạn đọc A đã có: {1, 2, 3, 4, 5}

→ Gợi ý cho A: {6, 7}
```

#### Bước 4: Tính điểm gợi ý

**Công thức:**
```
DiemGoiY = TrungBinhCong(DoTuongDong)
```

**Ví dụ:**

Sách ID = 6 được gợi ý bởi:
```
├─ Bạn đọc B (độ tương đồng 42.8%)
├─ Bạn đọc C (độ tương đồng 35.7%)
└─ Bạn đọc E (độ tương đồng 21.4%)

Điểm = (0.428 + 0.357 + 0.214) / 3 = 0.333 (33.3%)
```

### Ưu điểm:
✅ Khám phá được sách mới, đa dạng  
✅ Không cần phân tích nội dung sách  
✅ Chất lượng gợi ý tăng theo thời gian

### Nhược điểm:
❌ Yêu cầu nhiều dữ liệu người dùng  
❌ Cold start problem (người dùng mới)  
❌ Khó giải thích lý do gợi ý

---

## 🔄 3. HYBRID APPROACH (KẾT HỢP)

### Chiến lược kết hợp:

```
┌─────────────────────────────────────────┐
│  TẤT CẢ GỢI Ý                           │
├─────────────────────────────────────────┤
│  Content-Based: 60%                     │
│  ├─ Sách A: 0.85                        │
│  ├─ Sách B: 0.75                        │
│  └─ Sách C: 0.65                        │
├─────────────────────────────────────────┤
│  Collaborative: 40%                     │
│  ├─ Sách C: 0.70                        │
│  ├─ Sách D: 0.60                        │
│  └─ Sách E: 0.50                        │
└─────────────────────────────────────────┘
         ↓ KẾT HỢP
┌─────────────────────────────────────────┐
│  KẾT QUẢ CUỐI                           │
├─────────────────────────────────────────┤
│  Sách A: 0.85 (Content)                 │
│  Sách B: 0.75 (Content)                 │
│  Sách C: (0.65 + 0.70)/2 = 0.675 (Both) │
│  Sách D: 0.60 (Collaborative)           │
│  Sách E: 0.50 (Collaborative)           │
└─────────────────────────────────────────┘
```

### Lý do kết hợp:
✅ Bù trừ nhược điểm của nhau  
✅ Tăng độ chính xác  
✅ Cân bằng giữa an toàn và khám phá

---

## 🎲 4. COLD START PROBLEM (VẤN ĐỀ KHỞI ĐẦU)

### Trường hợp: Người dùng mới không có lịch sử

**Giải pháp: Popularity-Based Recommendation**

```sql
-- Lấy top sách phổ biến nhất
SELECT TOP 10 
    s.MaSach,
    s.TenSach,
    COUNT(ct.MaSach) AS SoLuotMuon
FROM Sach s
INNER JOIN ChiTietPhieuMuon ct ON s.MaSach = ct.MaSach
GROUP BY s.MaSach, s.TenSach
ORDER BY COUNT(ct.MaSach) DESC
```

**Logic:**
```
IF (Không có lịch sử) THEN
    Gợi ý sách phổ biến
ELSE IF (Có ít lịch sử < 3 cuốn) THEN
    70% Content-Based
    30% Popularity
ELSE
    60% Content-Based
    40% Collaborative
END IF
```

---

## 📊 5. ĐÁNH GIÁ HIỆU QUẢ

### Metrics quan trọng:

#### 5.1. Precision (Độ chính xác)
```
Precision = SoSachGoiYDuocMuon / TongSoSachGoiY

Ví dụ:
- Gợi ý 10 sách
- Người dùng mượn 6 sách
- Precision = 6/10 = 60%
```

#### 5.2. Recall (Độ bao phủ)
```
Recall = SoSachGoiYDuocMuon / TongSoSachNguoiDungMuon

Ví dụ:
- Người dùng mượn 8 sách
- 6 sách nằm trong gợi ý
- Recall = 6/8 = 75%
```

#### 5.3. Click-Through Rate (CTR)
```
CTR = SoLuotClick / SoLanHienThi × 100%
```

#### 5.4. Conversion Rate
```
ConversionRate = SoSachDaMuon / SoSachDaXem × 100%
```

### Query thống kê:

```sql
-- Hiệu quả tổng thể
SELECT 
    COUNT(*) AS TongGoiY,
    SUM(CASE WHEN DaXem = 1 THEN 1 ELSE 0 END) AS SoLuotXem,
    SUM(CASE WHEN DaMuon = 1 THEN 1 ELSE 0 END) AS SoLuotMuon,
    
    -- CTR
    CAST(SUM(CASE WHEN DaXem = 1 THEN 1 ELSE 0 END) * 100.0 / COUNT(*) 
        AS DECIMAL(5,2)) AS CTR,
    
    -- Conversion Rate
    CAST(SUM(CASE WHEN DaMuon = 1 THEN 1 ELSE 0 END) * 100.0 / 
        NULLIF(SUM(CASE WHEN DaXem = 1 THEN 1 ELSE 0 END), 0) 
        AS DECIMAL(5,2)) AS ConversionRate
FROM GoiYSach
WHERE NgayGoiY >= DATEADD(MONTH, -1, GETDATE())
```

---

## 🔧 6. TỐI ƯU HÓA THUẬT TOÁN

### 6.1. Điều chỉnh trọng số

**Hiện tại:**
```csharp
DiemPhuHop = (TiLeTheLoai × 0.6) + (TiLeTacGia × 0.4)
```

**Tùy chỉnh theo feedback:**
```csharp
// Nếu người dùng thích tác giả hơn thể loại
DiemPhuHop = (TiLeTheLoai × 0.4) + (TiLeTacGia × 0.6)

// Thêm yếu tố thời gian (sách mới)
DiemPhuHop = (TiLeTheLoai × 0.5) + (TiLeTacGia × 0.3) + (NhaCungCap × 0.2)
```

### 6.2. Time Decay (Giảm dần theo thời gian)

```csharp
// Ưu tiên sở thích gần đây
decimal timeWeight = 1.0m;
TimeSpan timeSince = DateTime.Now - lichSu.NgayMuon;

if (timeSince.TotalDays > 180)
    timeWeight = 0.5m; // Giảm 50% sau 6 tháng
else if (timeSince.TotalDays > 90)
    timeWeight = 0.7m; // Giảm 30% sau 3 tháng

diem *= timeWeight;
```

### 6.3. Diversity (Đa dạng hóa)

```csharp
// Đảm bảo không gợi ý toàn sách cùng thể loại
var topGoiY = ketQua
    .GroupBy(g => g.MaLoai)
    .SelectMany(group => group.Take(3)) // Tối đa 3 sách/thể loại
    .OrderByDescending(g => g.DiemGoiY)
    .Take(10)
    .ToList();
```

---

## 🚀 7. NÂNG CẤP TƯƠNG LAI

### 7.1. Matrix Factorization
Sử dụng SVD (Singular Value Decomposition) để tối ưu:
```
User-Item Matrix → Decompose → Predict Rating
```

### 7.2. Deep Learning
Neural Network với TensorFlow.NET:
```
Input: [UserFeatures, BookFeatures]
  ↓
Hidden Layers
  ↓
Output: PredictedRating
```

### 7.3. Context-Aware Recommendation
Xem xét ngữ cảnh:
- Thời gian (mùa, ngày trong tuần)
- Thiết bị (mobile, desktop)
- Vị trí (thư viện, nhà)

### 7.4. Reinforcement Learning
Học từ feedback và tự động điều chỉnh:
```
Action → Recommend Book
  ↓
Reward → User Borrows
  ↓
Update → Improve Algorithm
```

---

## 📖 8. TÀI LIỆU THAM KHẢO

### Papers:
1. "Collaborative Filtering for Implicit Feedback Datasets" - Hu et al.
2. "Matrix Factorization Techniques for Recommender Systems" - Koren et al.
3. "Deep Learning based Recommender System: A Survey" - Zhang et al.

### Online Courses:
- Coursera: Recommender Systems Specialization
- edX: Data Science: Machine Learning
- YouTube: StatQuest, 3Blue1Brown

### Books:
- "Programming Collective Intelligence" - Toby Segaran
- "Recommender Systems Handbook" - Ricci et al.

---

## 💡 KẾT LUẬN

Hệ thống gợi ý sách kết hợp:
- ✅ **Content-Based**: An toàn, giải thích được
- ✅ **Collaborative**: Đa dạng, khám phá mới
- ✅ **Popularity**: Giải quyết cold start

**Kết quả:**
> Một hệ thống gợi ý thông minh, chính xác và thân thiện với người dùng!

---

**"The best recommendation is one that feels like magic but is built on solid math."** ✨

