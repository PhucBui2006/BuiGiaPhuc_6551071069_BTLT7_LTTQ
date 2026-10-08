CREATE DATABASE FitZone;
GO
USE FitZone;
GO
CREATE TABLE HoiVien (
    MaHV INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh BIT DEFAULT 1,               
    NgaySinh DATE,
    SDT VARCHAR(15),
    Email VARCHAR(100),
    HangThanhVien NVARCHAR(20),           
    NgayDangKy DATETIME DEFAULT GETDATE(),
    TrangThai BIT DEFAULT 1               
);
GO


SELECT * FROM HoiVien;