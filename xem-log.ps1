# Script hỗ trợ xem log Catalog Service API
# Sử dụng: .\xem-log.ps1

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  CATALOG SERVICE API - LOG VIEWER" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Kiểm tra xem có process đang chạy không
$runningProcesses = Get-Process | Where-Object {
    $_.ProcessName -like "*dotnet*" -or 
    $_.ProcessName -like "*CatalogService*"
}

if ($runningProcesses) {
    Write-Host "⚠️  Có process đang chạy:" -ForegroundColor Yellow
    $runningProcesses | Format-Table Id, ProcessName, Path -AutoSize
    Write-Host ""
    $stop = Read-Host "Bạn có muốn dừng các process này không? (Y/N)"
    if ($stop -eq "Y" -or $stop -eq "y") {
        $runningProcesses | Stop-Process -Force
        Write-Host "✅ Đã dừng tất cả processes" -ForegroundColor Green
        Start-Sleep -Seconds 2
    }
}

Write-Host ""
Write-Host "Chọn cách xem log:" -ForegroundColor Cyan
Write-Host "1. Chạy ứng dụng và xem log real-time (HTTPS)" -ForegroundColor White
Write-Host "2. Chạy ứng dụng và xem log real-time (HTTP)" -ForegroundColor White
Write-Host "3. Chạy ứng dụng và lưu log vào file" -ForegroundColor White
Write-Host "4. Test API endpoints trực tiếp" -ForegroundColor White
Write-Host "5. Kiểm tra SQL Server connection" -ForegroundColor White
Write-Host "6. Xem cấu hình hiện tại" -ForegroundColor White
Write-Host "0. Thoát" -ForegroundColor White
Write-Host ""

$choice = Read-Host "Nhập lựa chọn (0-6)"

switch ($choice) {
    "1" {
        Write-Host ""
        Write-Host "🚀 Đang chạy ứng dụng với HTTPS..." -ForegroundColor Green
        Write-Host "📝 Log sẽ hiển thị bên dưới. Nhấn Ctrl+C để dừng." -ForegroundColor Yellow
        Write-Host ""
        Write-Host "URL: https://localhost:7080/swagger" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "========================================" -ForegroundColor Gray
        Set-Location "src\CatalogService\CatalogService.API"
        dotnet run --launch-profile https
    }
    "2" {
        Write-Host ""
        Write-Host "🚀 Đang chạy ứng dụng với HTTP..." -ForegroundColor Green
        Write-Host "📝 Log sẽ hiển thị bên dưới. Nhấn Ctrl+C để dừng." -ForegroundColor Yellow
        Write-Host ""
        Write-Host "URL: http://localhost:5173/swagger" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "========================================" -ForegroundColor Gray
        Set-Location "src\CatalogService\CatalogService.API"
        dotnet run --launch-profile http
    }
    "3" {
        Write-Host ""
        $logFile = "app_log_$(Get-Date -Format 'yyyyMMdd_HHmmss').txt"
        Write-Host "📝 Đang chạy và lưu log vào: $logFile" -ForegroundColor Green
        Write-Host ""
        Set-Location "src\CatalogService\CatalogService.API"
        dotnet run --launch-profile https 2>&1 | Tee-Object -FilePath "..\..\..\$logFile"
        Write-Host ""
        Write-Host "✅ Log đã được lưu vào: $logFile" -ForegroundColor Green
    }
    "4" {
        Write-Host ""
        Write-Host "🧪 Test API Endpoints" -ForegroundColor Cyan
        Write-Host ""
        
        $baseUrl = Read-Host "Nhập base URL (mặc định: https://localhost:7080)"
        if ([string]::IsNullOrWhiteSpace($baseUrl)) {
            $baseUrl = "https://localhost:7080"
        }
        
        Write-Host ""
        Write-Host "Đang test các endpoints..." -ForegroundColor Yellow
        Write-Host ""
        
        $endpoints = @(
            "/api/v1/catalog/categories",
            "/api/v1/catalog/brands",
            "/api/v1/catalog/products"
        )
        
        foreach ($endpoint in $endpoints) {
            $url = "$baseUrl$endpoint"
            Write-Host "Testing: $url" -ForegroundColor Cyan
            try {
                $response = Invoke-WebRequest -Uri $url -SkipCertificateCheck -ErrorAction Stop
                Write-Host "  ✅ Success: $($response.StatusCode)" -ForegroundColor Green
                Write-Host "  📄 Content length: $($response.Content.Length) bytes" -ForegroundColor Gray
            } catch {
                Write-Host "  ❌ Error: $($_.Exception.Message)" -ForegroundColor Red
                if ($_.Exception.Response) {
                    $statusCode = $_.Exception.Response.StatusCode.value__
                    Write-Host "  📊 Status Code: $statusCode" -ForegroundColor Yellow
                }
            }
            Write-Host ""
        }
        
        Write-Host "Hoàn thành!" -ForegroundColor Green
    }
    "5" {
        Write-Host ""
        Write-Host "🔍 Kiểm tra SQL Server Connection" -ForegroundColor Cyan
        Write-Host ""
        
        # Đọc connection string từ appsettings.Development.json
        $appSettingsPath = "src\CatalogService\CatalogService.API\appsettings.Development.json"
        
        if (Test-Path $appSettingsPath) {
            $appSettings = Get-Content $appSettingsPath | ConvertFrom-Json
            $connectionString = $appSettings.ConnectionStrings.CatalogDb
            
            Write-Host "Connection String:" -ForegroundColor Yellow
            Write-Host $connectionString -ForegroundColor Gray
            Write-Host ""
            
            # Extract server name
            if ($connectionString -match "Server=([^;]+)") {
                $server = $matches[1]
                Write-Host "Server: $server" -ForegroundColor Cyan
                Write-Host ""
                Write-Host "⚠️  Để test connection, bạn cần:" -ForegroundColor Yellow
                Write-Host "   1. Mở SQL Server Management Studio (SSMS)" -ForegroundColor White
                Write-Host "   2. Kết nối với thông tin trên" -ForegroundColor White
                Write-Host "   3. Hoặc chạy ứng dụng và xem log database" -ForegroundColor White
            }
        } else {
            Write-Host "❌ Không tìm thấy file appsettings.Development.json" -ForegroundColor Red
        }
        
        Write-Host ""
    }
    "6" {
        Write-Host ""
        Write-Host "📋 Cấu hình hiện tại" -ForegroundColor Cyan
        Write-Host ""
        
        Write-Host "✅ .NET Version:" -ForegroundColor Green
        dotnet --version
        Write-Host ""
        
        Write-Host "✅ .NET SDKs đã cài:" -ForegroundColor Green
        dotnet --list-sdks
        Write-Host ""
        
        Write-Host "✅ SQL Server Services:" -ForegroundColor Green
        Get-Service -Name "*SQL*" | Format-Table Name, Status, DisplayName -AutoSize
        Write-Host ""
        
        $appSettingsPath = "src\CatalogService\CatalogService.API\appsettings.Development.json"
        if (Test-Path $appSettingsPath) {
            Write-Host "✅ Connection String:" -ForegroundColor Green
            $appSettings = Get-Content $appSettingsPath | ConvertFrom-Json
            Write-Host $appSettings.ConnectionStrings.CatalogDb -ForegroundColor Gray
        }
        Write-Host ""
    }
    "0" {
        Write-Host "👋 Tạm biệt!" -ForegroundColor Cyan
        exit
    }
    default {
        Write-Host "❌ Lựa chọn không hợp lệ!" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Gray

