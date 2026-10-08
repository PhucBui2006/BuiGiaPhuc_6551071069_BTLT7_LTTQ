using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Bai4.Models; // Đổi namespace này theo project thực tế của bạn

namespace Bai4
{
    public partial class Form1 : Form
    {
        private AnKhangClinicContext _context;
        private int selectedMaLich = -1;

        public Form1()
        {
            InitializeComponent();
            _context = new AnKhangClinicContext();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Cấu hình DateTimePicker
            dtpNgayKham.Format = DateTimePickerFormat.Custom;
            dtpNgayKham.CustomFormat = "dd/MM/yyyy HH:mm";

            dtpGioKham.Format = DateTimePickerFormat.Custom;
            dtpGioKham.CustomFormat = "HH:mm";
            dtpGioKham.ShowUpDown = true;

            dtpTuNgay.Format = DateTimePickerFormat.Short;
            dtpDenNgay.Format = DateTimePickerFormat.Short;

            LoadComboBoxBacSi();
            LoadComboBoxTrangThai();
            LoadDataToDataGridView();
        }

        private void LoadComboBoxBacSi()
        {
            var danhSachBacSi = _context.BacSis
                .Select(bs => new
                {
                    bs.MaBs,
                    HienThi = "BS. " + bs.HoTen + " - " + bs.ChuyenKhoa
                })
                .ToList();

            cboBacSi.DataSource = danhSachBacSi;
            cboBacSi.DisplayMember = "HienThi";
            cboBacSi.ValueMember = "MaBs";
            cboBacSi.SelectedIndex = -1;

            var dsTimKiem = _context.BacSis.ToList();
            dsTimKiem.Insert(0, new BacSi { MaBs = 0, HoTen = "--- Tất cả bác sĩ ---", ChuyenKhoa = "" });
            cboTimKiemBacSi.DataSource = dsTimKiem;
            cboTimKiemBacSi.DisplayMember = "HoTen";
            cboTimKiemBacSi.ValueMember = "MaBs";
            cboTimKiemBacSi.SelectedIndex = 0;
        }

        private void LoadComboBoxTrangThai()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Chờ khám");
            cboTrangThai.Items.Add("Đã khám");
            cboTrangThai.Items.Add("Đã hủy");
            cboTrangThai.SelectedIndex = 0;
        }

        private void LoadDataToDataGridView()
        {
            var query = _context.LichKhams
                .Include(l => l.MaBsNavigation)
                .Select(l => new
                {
                    l.MaLich,
                    l.TenBenhNhan,
                    l.Sdt,
                    l.NgayKham,
                    l.GioKham,
                    HoTenBacSi = l.MaBsNavigation != null ? "BS. " + l.MaBsNavigation.HoTen : "",
                    ChuyenKhoa = l.MaBsNavigation != null ? l.MaBsNavigation.ChuyenKhoa : "",
                    l.TrangThai
                })
                .ToList();

            dgvLichKham.DataSource = query;

            if (dgvLichKham.Columns["MaLich"] != null) dgvLichKham.Columns["MaLich"].HeaderText = "Mã lịch";
            if (dgvLichKham.Columns["TenBenhNhan"] != null) dgvLichKham.Columns["TenBenhNhan"].HeaderText = "Tên bệnh nhân";
            if (dgvLichKham.Columns["Sdt"] != null) dgvLichKham.Columns["Sdt"].HeaderText = "SĐT";
            if (dgvLichKham.Columns["NgayKham"] != null) dgvLichKham.Columns["NgayKham"].HeaderText = "Ngày khám";
            if (dgvLichKham.Columns["GioKham"] != null) dgvLichKham.Columns["GioKham"].HeaderText = "Giờ khám";
            if (dgvLichKham.Columns["HoTenBacSi"] != null) dgvLichKham.Columns["HoTenBacSi"].HeaderText = "Bác sĩ";
            if (dgvLichKham.Columns["ChuyenKhoa"] != null) dgvLichKham.Columns["ChuyenKhoa"].HeaderText = "Chuyên khoa";
            if (dgvLichKham.Columns["TrangThai"] != null) dgvLichKham.Columns["TrangThai"].HeaderText = "Trạng thái";
        }

        private bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(txtTenBenhNhan.Text))
            {
                MessageBox.Show("Tên bệnh nhân không được để trống!", "Lỗi Validate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenBenhNhan.Focus();
                return false;
            }

            if (cboBacSi.SelectedValue == null || (int)cboBacSi.SelectedValue <= 0)
            {
                MessageBox.Show("Vui lòng chọn bác sĩ khám bệnh!", "Lỗi Validate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboBacSi.Focus();
                return false;
            }

            if (dtpNgayKham.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Không thể đặt lịch khám vào ngày trong quá khứ!", "Lỗi Validate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgayKham.Focus();
                return false;
            }

            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;

            try
            {
                var lichKhamMoi = new LichKham
                {
                    TenBenhNhan = txtTenBenhNhan.Text.Trim(),
                    Sdt = txtSDT.Text.Trim(),
                    NgayKham = DateOnly.FromDateTime(dtpNgayKham.Value.Date),
                    GioKham = TimeOnly.FromDateTime(dtpGioKham.Value),
                    MaBs = (int)cboBacSi.SelectedValue,
                    TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Chờ khám"
                };

                _context.LichKhams.Add(lichKhamMoi);
                _context.SaveChanges();

                MessageBox.Show("Thêm lịch khám thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataToDataGridView();
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaLich == -1)
            {
                MessageBox.Show("Vui lòng chọn dòng lịch khám cần sửa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateData()) return;

            try
            {
                var lichKham = _context.LichKhams.Find(selectedMaLich);
                if (lichKham != null)
                {
                    lichKham.TenBenhNhan = txtTenBenhNhan.Text.Trim();
                    lichKham.Sdt = txtSDT.Text.Trim();
                    lichKham.NgayKham = DateOnly.FromDateTime(dtpNgayKham.Value.Date);
                    lichKham.GioKham = TimeOnly.FromDateTime(dtpGioKham.Value);
                    lichKham.MaBs = (int)cboBacSi.SelectedValue;
                    lichKham.TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Chờ khám";

                    _context.SaveChanges();

                    MessageBox.Show("Cập nhật lịch khám thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataToDataGridView();
                    btnLamMoi_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMaLich == -1)
            {
                MessageBox.Show("Vui lòng chọn dòng lịch khám cần xóa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa lịch khám này không?",
                                                 "Xác nhận xóa",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    var lichKham = _context.LichKhams.Find(selectedMaLich);
                    if (lichKham != null)
                    {
                        _context.LichKhams.Remove(lichKham);
                        _context.SaveChanges();

                        MessageBox.Show("Xóa lịch khám thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataToDataGridView();
                        btnLamMoi_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            selectedMaLich = -1;
            txtTenBenhNhan.Clear();
            txtSDT.Clear();
            dtpNgayKham.Value = DateTime.Now;
            dtpGioKham.Value = DateTime.Now;
            if (cboBacSi.Items.Count > 0) cboBacSi.SelectedIndex = -1;
            if (cboTrangThai.Items.Count > 0) cboTrangThai.SelectedIndex = 0;
            dgvLichKham.ClearSelection();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                DateOnly tuNgay = DateOnly.FromDateTime(dtpTuNgay.Value.Date);
                DateOnly denNgay = DateOnly.FromDateTime(dtpDenNgay.Value.Date);
                int maBsTimKiem = cboTimKiemBacSi.SelectedValue != null ? (int)cboTimKiemBacSi.SelectedValue : 0;

                var query = _context.LichKhams
                    .Include(l => l.MaBsNavigation)
                    .Where(l => l.NgayKham >= tuNgay && l.NgayKham <= denNgay);

                if (maBsTimKiem > 0)
                {
                    query = query.Where(l => l.MaBs == maBsTimKiem);
                }

                var ketQua = query.Select(l => new
                {
                    l.MaLich,
                    l.TenBenhNhan,
                    l.Sdt,
                    l.NgayKham,
                    l.GioKham,
                    HoTenBacSi = l.MaBsNavigation != null ? "BS. " + l.MaBsNavigation.HoTen : "",
                    ChuyenKhoa = l.MaBsNavigation != null ? l.MaBsNavigation.ChuyenKhoa : "",
                    l.TrangThai
                }).ToList();

                dgvLichKham.DataSource = ketQua;

                if (ketQua.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy lịch khám phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvLichKham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLichKham.Rows[e.RowIndex];
                if (row.Cells["MaLich"].Value != null)
                {
                    selectedMaLich = Convert.ToInt32(row.Cells["MaLich"].Value);
                    txtTenBenhNhan.Text = row.Cells["TenBenhNhan"].Value?.ToString() ?? "";
                    txtSDT.Text = row.Cells["Sdt"].Value?.ToString() ?? "";

                    if (row.Cells["NgayKham"].Value != null && DateOnly.TryParse(row.Cells["NgayKham"].Value.ToString(), out DateOnly ngay))
                    {
                        dtpNgayKham.Value = ngay.ToDateTime(TimeOnly.MinValue);
                    }

                    if (row.Cells["GioKham"].Value != null && TimeOnly.TryParse(row.Cells["GioKham"].Value.ToString(), out TimeOnly gio))
                    {
                        dtpGioKham.Value = DateTime.Today.Add(gio.ToTimeSpan());
                    }

                    var lichKhamObj = _context.LichKhams.Find(selectedMaLich);
                    if (lichKhamObj != null)
                    {
                        cboBacSi.SelectedValue = lichKhamObj.MaBs;
                        cboTrangThai.SelectedItem = lichKhamObj.TrangThai;
                    }
                }
            }
        }
    }
}