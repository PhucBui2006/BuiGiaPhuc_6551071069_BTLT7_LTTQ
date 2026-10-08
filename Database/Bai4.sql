CREATE DATABASE AnKhangClinic;
GO
USE AnKhangClinic;
GO
CREATE TABLE BacSi(
    MaBS INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BacSi PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    ChuyenKhoa NVARCHAR(100) NULL,
    SDT VARCHAR(15)   NULL
);
GO

CREATE TABLE LichKham(
    MaLich INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_LichKham PRIMARY KEY,
    TenBenhNhan NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15) NULL,
    NgayKham DATE   NOT NULL,
    GioKham TIME(0) NOT NULL,
    MaBS INT  NOT NULL,
    TrangThai   NVARCHAR(20)  NOT NULL CONSTRAINT DF_LichKham_TrangThai DEFAULT (N'Chờ khám'),
    CONSTRAINT FK_LichKham_BacSi FOREIGN KEY (MaBS) REFERENCES dbo.BacSi(MaBS),
    CONSTRAINT CK_LichKham_TrangThai CHECK (TrangThai IN (N'Chờ khám', N'Đã khám', N'Đã hủy'))
);
GO

INSERT INTO dbo.BacSi (HoTen, ChuyenKhoa, SDT) VALUES
(N'Nguyễn Văn A', N'Nội tổng quát','0901000001'),
(N'Nguyễn Văn B', N'Nhi khoa','0901000002'),
(N'Nguyễn Văn C', N'Tai Mũi Họng', '0901000003'),
(N'Nguyễn Văn D', N'Da liễu','0901000004');
GO
INSERT INTO dbo.LichKham (TenBenhNhan, SDT, NgayKham, GioKham, MaBS, TrangThai) VALUES
(N'Nguyễn Văn Bình','09725667894', '2026-12-13', '09:00', 1, N'Chờ khám'),
(N'Nguyễn Xinh','09725667897', '2022-12-29', '13:00', 1, N'Đã khám'),
(N'Nguyễn Hạm', '09725667899', '2022-12-23', '16:00', 2, N'Đã hủy'),
(N'Nguyễn Tình','09725667830', '2022-12-23', '16:00', 3, N'Đã hủy'),
(N'Nguyễn Hoan Anh','09725667899', '2022-12-29', '18:00', 4, N'Đã hủy');
GO
SELECT l.MaLich, l.TenBenhNhan, l.NgayKham, l.GioKham, b.HoTen, b.ChuyenKhoa, l.TrangThai
FROM dbo.LichKham l JOIN dbo.BacSi b ON b.MaBS = l.MaBS;
GO