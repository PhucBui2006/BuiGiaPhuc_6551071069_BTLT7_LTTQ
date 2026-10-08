using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Bai3.Models;

namespace Bai3
{
    public partial class Form1 : Form
    {
        private SunriseHomestayDBContext _context = new SunriseHomestayDBContext();
        private string selectedFileName = "";
        private string imageFolderPath = Path.Combine(Application.StartupPath, "Images");
        private bool isInitialized = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            isInitialized = false;

            if (!Directory.Exists(imageFolderPath))
                Directory.CreateDirectory(imageFolderPath);

            SetupDataGridViewColumns();
            LoadComboBoxData();

            isInitialized = true;
            LoadDataGridViewData();
        }

        #region Khởi tạo giao diện

        private void SetupDataGridViewColumns()
        {
            if (dgvPhong.Columns.Count > 0) return;

            dgvPhong.AutoGenerateColumns = false;
            dgvPhong.AllowUserToAddRows = false;
            dgvPhong.RowTemplate.Height = 45;
            dgvPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvPhong.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaPhong", HeaderText = "Mã phòng" });
            dgvPhong.Columns.Add(new DataGridViewImageColumn
            {
                Name = "HinhAnh",
                HeaderText = "Ảnh",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            });
            dgvPhong.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoPhong", HeaderText = "Số phòng" });
            dgvPhong.Columns.Add(new DataGridViewTextBoxColumn { Name = "TangSo", HeaderText = "Tầng" });
            dgvPhong.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenLoai", HeaderText = "Loại phòng" });
            dgvPhong.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaMoiDem",
                HeaderText = "Giá/đêm",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
            dgvPhong.Columns.Add(new DataGridViewTextBoxColumn { Name = "TinhTrang", HeaderText = "Tình trạng" });
        }

        private void LoadComboBoxData()
        {
            // ComboBox nhập liệu
            cboLoaiPhong.DataSource = _context.LoaiPhongs.AsNoTracking().ToList();
            cboLoaiPhong.DisplayMember = "TenLoai";
            cboLoaiPhong.ValueMember = "MaLoai";

            cboTinhTrang.Items.Clear();
            cboTinhTrang.Items.AddRange(new object[] { "Trống", "Đang ở", "Đang dọn" });
            cboTinhTrang.SelectedIndex = 0;

            // ComboBox lọc
            var listLocLoai = _context.LoaiPhongs.AsNoTracking().ToList();
            listLocLoai.Insert(0, new LoaiPhong { MaLoai = 0, TenLoai = "lọc theo loại phòng" });
            cboLocLoaiPhong.DataSource = listLocLoai;
            cboLocLoaiPhong.DisplayMember = "TenLoai";
            cboLocLoaiPhong.ValueMember = "MaLoai";

            cboLocTinhTrang.DataSource = new string[] { "lọc theo tình trạng", "Trống", "Đang ở", "Đang dọn" };
            cboLocTinhTrang.SelectedIndex = 0;
        }

        #endregion

        #region Load + Tìm kiếm (dùng chung)

        // Load theo điều kiện lọc hiện tại (không chọn lọc thì hiện tất cả)
        private void LoadDataGridViewData()
        {
            int maLoai = cboLocLoaiPhong.SelectedValue is int id ? id : 0;
            string tinhTrang = cboLocTinhTrang.SelectedIndex > 0
                ? cboLocTinhTrang.SelectedItem?.ToString()
                : null;

            var query = _context.Phongs
                .AsNoTracking()
                .Include(p => p.MaLoaiNavigation)
                .AsQueryable();

            if (maLoai > 0)
                query = query.Where(p => p.MaLoai == maLoai);

            if (!string.IsNullOrEmpty(tinhTrang))
                query = query.Where(p => p.TinhTrang == tinhTrang);

            var data = query.Select(p => new
            {
                p.MaPhong,
                p.HinhAnh,
                p.SoPhong,
                p.TangSo,
                TenLoai = p.MaLoaiNavigation != null ? p.MaLoaiNavigation.TenLoai : "",
                GiaMoiDem = p.MaLoaiNavigation != null ? p.MaLoaiNavigation.GiaMoiDem : 0,
                p.TinhTrang
            }).ToList();

            dgvPhong.Rows.Clear();
            foreach (var item in data)
            {
                dgvPhong.Rows.Add(item.MaPhong, LoadThumbnail(item.HinhAnh),
                    item.SoPhong, item.TangSo, item.TenLoai, item.GiaMoiDem, item.TinhTrang);
            }
        }

        private Image LoadThumbnail(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            string fullPath = Path.Combine(imageFolderPath, fileName);
            if (!File.Exists(fullPath)) return null;

            using (var bmpTemp = new Bitmap(fullPath))
                return new Bitmap(bmpTemp, 60, 40);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDataGridViewData();
        }

        private void cboLocLoaiPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitialized) LoadDataGridViewData();
        }

        private void cboLocTinhTrang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitialized) LoadDataGridViewData();
        }

        #endregion

        #region Form phụ Loại phòng

        private void btnLoaiPhong_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmLoaiPhong())
            {
                frm.ShowDialog();
            }

            // Nạp lại dữ liệu sau khi form phụ thay đổi bảng LoaiPhong
            _context.Dispose();
            _context = new SunriseHomestayDBContext();

            isInitialized = false;
            LoadComboBoxData();
            isInitialized = true;

            ClearForm();
            LoadDataGridViewData();
        }

        #endregion

        #region Ảnh

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                string fileName = Path.GetFileName(ofd.FileName);
                string destPath = Path.Combine(imageFolderPath, fileName);

                if (!File.Exists(destPath))
                    File.Copy(ofd.FileName, destPath, true);

                selectedFileName = fileName; // chỉ lưu TÊN FILE vào CSDL
                ShowImage(destPath);
            }
        }

        // Đọc ảnh qua bản sao để không khóa file
        private void ShowImage(string fullPath)
        {
            picHinhAnh.Image?.Dispose();
            if (File.Exists(fullPath))
            {
                using (var bmpTemp = new Bitmap(fullPath))
                    picHinhAnh.Image = new Bitmap(bmpTemp);
                picHinhAnh.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                picHinhAnh.Image = null;
            }
        }

        #endregion

        #region CRUD

        private void dgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var cellValue = dgvPhong.Rows[e.RowIndex].Cells["MaPhong"].Value;
            if (cellValue == null) return;

            int maPhong = Convert.ToInt32(cellValue);
            var phong = _context.Phongs.AsNoTracking().FirstOrDefault(p => p.MaPhong == maPhong);
            if (phong == null) return;

            txtSoPhong.Text = phong.SoPhong;
            numTangSo.Value = phong.TangSo ?? 1;
            cboLoaiPhong.SelectedValue = phong.MaLoai;
            cboTinhTrang.SelectedItem = phong.TinhTrang;
            selectedFileName = phong.HinhAnh ?? "";

            if (!string.IsNullOrEmpty(selectedFileName))
                ShowImage(Path.Combine(imageFolderPath, selectedFileName));
            else
            {
                picHinhAnh.Image?.Dispose();
                picHinhAnh.Image = null;
            }
        }

        private bool ValidateInput(int maPhongHienTai = 0)
        {
            if (string.IsNullOrWhiteSpace(txtSoPhong.Text))
            {
                MessageBox.Show("Vui lòng nhập số phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoPhong.Focus();
                return false;
            }
            if (cboLoaiPhong.SelectedValue == null)
            {
                MessageBox.Show("Chưa có loại phòng để chọn! Hãy thêm loại phòng trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string soPhong = txtSoPhong.Text.Trim();
            bool trung = _context.Phongs.Any(p => p.SoPhong == soPhong && p.MaPhong != maPhongHienTai);
            if (trung)
            {
                MessageBox.Show("Số phòng này đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoPhong.Focus();
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            var newPhong = new Phong
            {
                SoPhong = txtSoPhong.Text.Trim(),
                TangSo = (int)numTangSo.Value,
                MaLoai = (int)cboLoaiPhong.SelectedValue,
                TinhTrang = cboTinhTrang.SelectedItem.ToString(),
                HinhAnh = selectedFileName
            };

            _context.Phongs.Add(newPhong);
            _context.SaveChanges();

            MessageBox.Show("Thêm phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDataGridViewData();
            ClearForm();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvPhong.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng cần sửa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maPhong = Convert.ToInt32(dgvPhong.SelectedRows[0].Cells["MaPhong"].Value);
            if (!ValidateInput(maPhong)) return;

            var phong = _context.Phongs.Find(maPhong);
            if (phong == null) return;

            phong.SoPhong = txtSoPhong.Text.Trim();
            phong.TangSo = (int)numTangSo.Value;
            phong.MaLoai = (int)cboLoaiPhong.SelectedValue;
            phong.TinhTrang = cboTinhTrang.SelectedItem.ToString();
            phong.HinhAnh = selectedFileName;

            _context.SaveChanges();
            MessageBox.Show("Cập nhật phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDataGridViewData();
            ClearForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvPhong.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng cần xóa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa phòng này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            int maPhong = Convert.ToInt32(dgvPhong.SelectedRows[0].Cells["MaPhong"].Value);
            var phong = _context.Phongs.Find(maPhong);
            if (phong == null) return;

            _context.Phongs.Remove(phong);
            _context.SaveChanges();

            MessageBox.Show("Xóa phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDataGridViewData();
            ClearForm();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            isInitialized = false; // tránh load lặp khi reset combobox lọc
            cboLocLoaiPhong.SelectedIndex = 0;
            cboLocTinhTrang.SelectedIndex = 0;
            isInitialized = true;

            ClearForm();
            LoadDataGridViewData();
        }

        private void ClearForm()
        {
            txtSoPhong.Clear();
            numTangSo.Value = 1;
            if (cboLoaiPhong.Items.Count > 0) cboLoaiPhong.SelectedIndex = 0;
            if (cboTinhTrang.Items.Count > 0) cboTinhTrang.SelectedIndex = 0;
            picHinhAnh.Image?.Dispose();
            picHinhAnh.Image = null;
            selectedFileName = "";
        }

        #endregion
    }
}