CREATE DATABASE SunriseHomestayDB;
GO

USE SunriseHomestayDB;
GO
CREATE TABLE LoaiPhong (
    MaLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(100) NOT NULL,
    GiaMoiDem DECIMAL(18,2),
    MoTa NVARCHAR(255)
);
GO
CREATE TABLE Phong (
    MaPhong INT IDENTITY(1,1) PRIMARY KEY,
    SoPhong VARCHAR(10) NOT NULL,
    TangSo INT,
    TinhTrang NVARCHAR(20), -- 'Trống', 'Đang ở', 'Đang dọn'
    HinhAnh NVARCHAR(255),  -- Lưu tên file ảnh
    MaLoai INT FOREIGN KEY REFERENCES LoaiPhong(MaLoai)
);
GO
INSERT INTO LoaiPhong (TenLoai, GiaMoiDem, MoTa) VALUES 
(N'Phòng Đơn', 350000, N'Phòng dành cho 1 người, đầy đủ tiện nghi'),
(N'Phòng Đôi', 500000, N'Phòng dành cho 2 người, view hướng biển'),
(N'Phòng VIP', 1000000, N'Phòng cao cấp, không gian rộng rãi');
GO