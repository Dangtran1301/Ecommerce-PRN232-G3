# 🚀 BẮT ĐẦU XEM LOG - HƯỚNG DẪN NHANH

## CÁCH 1: Sử dụng Script PowerShell (Dễ nhất) ⭐

1. **Mở PowerShell trong thư mục project:**
   ```powershell
   cd D:\KY_8\PRN232\GitNew
   ```

2. **Chạy script:**
   ```powershell
   .\xem-log.ps1
   ```

3. **Chọn option 1** để chạy ứng dụng và xem log real-time

---

## CÁCH 2: Xem log từ Visual Studio

1. **Mở Visual Studio:**
   - Mở solution: `src/CatalogService/CatalogService.sln`

2. **Mở Output Window:**
   - Menu: `View` → `Output` (hoặc `Ctrl + Alt + O`)

3. **Chọn nguồn log:**
   - Dropdown "Show output from:" → Chọn **"Debug"** hoặc **"CatalogService.API"**

4. **Chạy ứng dụng:**
   - Nhấn `F5`

5. **Xem log:**
   - Log sẽ hiển thị trong Output window
   - Tìm các dòng có `error`, `fail`, `exception`

---

## CÁCH 3: Xem log từ Terminal

1. **Mở PowerShell:**
   ```powershell
   cd D:\KY_8\PRN232\GitNew\src\CatalogService\CatalogService.API
   ```

2. **Chạy ứng dụng:**
   ```powershell
   dotnet run --launch-profile https
   ```

3. **Quan sát log:**
   - Tất cả log sẽ hiển thị trong terminal
   - Tìm các dòng có `error`, `fail`, `exception`

---

## CÁCH 4: Xem lỗi từ Browser (Khi Swagger bị lỗi)

1. **Mở Swagger UI:**
   - URL: `https://localhost:7080/swagger`

2. **Mở Developer Tools:**
   - Nhấn `F12` hoặc `Ctrl + Shift + I`

3. **Xem Console tab:**
   - Click tab **"Console"**
   - Tìm error message

4. **Xem Network tab:**
   - Click tab **"Network"**
   - Refresh trang (`F5`)
   - Tìm request `swagger.json`
   - Click vào request → xem tab **"Response"** để xem chi tiết lỗi

---

## CÁCH 5: Test API trực tiếp (Bỏ qua Swagger)

Mở trực tiếp trong browser:
```
https://localhost:7080/api/v1/catalog/categories
https://localhost:7080/api/v1/catalog/brands
https://localhost:7080/api/v1/catalog/products
```

Hoặc dùng PowerShell:
```powershell
Invoke-WebRequest -Uri "https://localhost:7080/api/v1/catalog/categories" -SkipCertificateCheck
```

---

## 📝 CHECKLIST KHI GẶP LỖI 500 SWAGGER

- [ ] 1. Dừng ứng dụng đang chạy (nếu có)
- [ ] 2. Mở Output window (Visual Studio) hoặc terminal
- [ ] 3. Chạy ứng dụng lại
- [ ] 4. Copy toàn bộ log có chứa `error`, `fail`, `exception`
- [ ] 5. Mở Swagger UI trong browser
- [ ] 6. Mở Developer Tools (F12)
- [ ] 7. Xem Console tab → tìm error
- [ ] 8. Xem Network tab → click vào request `swagger.json` → xem Response
- [ ] 9. Copy error message từ Response
- [ ] 10. Gửi tất cả thông tin để phân tích

---

## 🔍 CÁC THÔNG TIN CẦN THU THẬP

Khi gặp lỗi, hãy copy các thông tin sau:

1. **Log từ Output window/terminal:**
   - Tất cả dòng có `error`, `fail`, `exception`
   - Stack trace đầy đủ

2. **Error từ Browser:**
   - Console error message
   - Network tab → Response của request `swagger.json`

3. **Thông tin môi trường:**
   - .NET version: `dotnet --version`
   - SQL Server đang chạy: `Get-Service -Name "*SQL*"`

---

## ⚡ QUICK FIX THỬ NGAY

Nếu Swagger bị lỗi 500, thử các bước sau:

1. **Dừng ứng dụng:**
   - Visual Studio: `Shift + F5`
   - Hoặc kill process: `Get-Process | Where-Object {$_.ProcessName -like "*dotnet*"} | Stop-Process -Force`

2. **Kiểm tra SQL Server:**
   ```powershell
   Get-Service -Name "*SQL*" | Select-Object Name, Status
   ```

3. **Chạy lại và xem log:**
   ```powershell
   cd src\CatalogService\CatalogService.API
   dotnet run --launch-profile https
   ```

4. **Xem log chi tiết trong Output window**

---

## 📚 TÀI LIỆU ĐẦY ĐỦ

Xem file `HUONG_DAN_XEM_LOG.md` để có hướng dẫn chi tiết đầy đủ.

