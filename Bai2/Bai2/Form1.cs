using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Bai2.Models;

namespace Bai2
{
    public partial class Form1 : Form
    {
        private int selectedMaHV = 0;

        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;
            dgvHoiVien.CellClick += dgvHoiVien_CellClick;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnTimKiem.Click += btnTimKiem_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cboHangThanhVien.Items.Clear();
            cboHangThanhVien.Items.AddRange(new string[] { "Basic", "VIP", "Premium" });
            cboHangThanhVien.SelectedIndex = 0;

            cboTimKiemHang.Items.Clear();
            cboTimKiemHang.Items.Add("Tất cả");
            cboTimKiemHang.Items.AddRange(new string[] { "Basic", "VIP", "Premium" });
            cboTimKiemHang.SelectedIndex = 0;

            LoadData();
        }

        private void LoadData()
        {
            using (var db = new FitZoneContext())
            {
                var list = db.HoiViens.Select(h => new
                {
                    h.MaHv, // Đã đổi thành Mahv theo model EF Core
                    h.HoTen,
                    GioiTinh = h.GioiTinh == true ? "Nam" : "Nữ",
                    NgaySinh = h.NgaySinh.HasValue ? h.NgaySinh.Value.ToString("dd/MM/yyyy") : "",
                    h.Sdt,
                    h.Email,
                    h.HangThanhVien,
                    TrangThai = h.TrangThai == true ? "Đang hoạt động" : "Tạm ngưng"
                }).ToList();

                dgvHoiVien.DataSource = list;
            }
        }

        private void dgvHoiVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvHoiVien.Rows[e.RowIndex];
                selectedMaHV = Convert.ToInt32(row.Cells["Mahv"].Value); // Đọc theo cột Mahv

                using (var db = new FitZoneContext())
                {
                    var hv = db.HoiViens.Find(selectedMaHV);
                    if (hv != null)
                    {
                        txtHoTen.Text = hv.HoTen;
                        if (hv.GioiTinh == true) rdoNam.Checked = true; else rdoNu.Checked = true;

                        // Chuyển đổi DateOnly sang DateTime cho DateTimePicker
                        if (hv.NgaySinh.HasValue)
                            dtpNgaySinh.Value = hv.NgaySinh.Value.ToDateTime(TimeOnly.MinValue);

                        txtSDT.Text = hv.Sdt;
                        txtEmail.Text = hv.Email;
                        cboHangThanhVien.SelectedItem = hv.HangThanhVien;
                        chkTrangThai.Checked = hv.TrangThai ?? false;
                    }
                }
            }
        }

        private bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHoTen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSDT.Text) || !txtSDT.Text.All(char.IsDigit) || txtSDT.Text.Length < 9 || txtSDT.Text.Length > 11)
            {
                MessageBox.Show("Số điện thoại phải chỉ chứa chữ số và có độ dài từ 9 đến 11 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDT.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Email không hợp lệ (phải chứa ký tự '@')!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return false;
            }

            int age = DateTime.Now.Year - dtpNgaySinh.Value.Year;
            if (dtpNgaySinh.Value.Date > DateTime.Now.AddYears(-age)) age--;
            if (age < 15)
            {
                MessageBox.Show("Hội viên phải từ 15 tuổi trở lên mới được đăng ký!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpNgaySinh.Focus();
                return false;
            }

            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;

            using (var db = new FitZoneContext())
            {
                var hv = new HoiVien
                {
                    HoTen = txtHoTen.Text.Trim(),
                    GioiTinh = rdoNam.Checked,
                    NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value), // Chuyển DateTime sang DateOnly
                    Sdt = txtSDT.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    HangThanhVien = cboHangThanhVien.SelectedItem.ToString(),
                    TrangThai = chkTrangThai.Checked
                };

                db.HoiViens.Add(hv);
                db.SaveChanges();
            }

            MessageBox.Show("Thêm hội viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
            ClearForm();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaHV == 0)
            {
                MessageBox.Show("Vui lòng chọn hội viên cần sửa trên lưới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateData()) return;

            using (var db = new FitZoneContext())
            {
                var hv = db.HoiViens.Find(selectedMaHV);
                if (hv != null)
                {
                    hv.HoTen = txtHoTen.Text.Trim();
                    hv.GioiTinh = rdoNam.Checked;
                    hv.NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value); // Chuyển DateTime sang DateOnly
                    hv.Sdt = txtSDT.Text.Trim();
                    hv.Email = txtEmail.Text.Trim();
                    hv.HangThanhVien = cboHangThanhVien.SelectedItem.ToString();
                    hv.TrangThai = chkTrangThai.Checked;

                    db.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin hội viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            LoadData();
            ClearForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMaHV == 0)
            {
                MessageBox.Show("Vui lòng chọn hội viên cần xóa trên lưới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa hội viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                using (var db = new FitZoneContext())
                {
                    var hv = db.HoiViens.Find(selectedMaHV);
                    if (hv != null)
                    {
                        db.HoiViens.Remove(hv);
                        db.SaveChanges();
                        MessageBox.Show("Xóa hội viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                LoadData();
                ClearForm();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiemHoTen.Text.Trim();
            string selectedHang = cboTimKiemHang.SelectedItem?.ToString();

            using (var db = new FitZoneContext())
            {
                var query = db.HoiViens.AsQueryable();

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(h => h.HoTen.Contains(keyword));
                }

                if (!string.IsNullOrEmpty(selectedHang) && selectedHang != "Tất cả")
                {
                    query = query.Where(h => h.HangThanhVien == selectedHang);
                }

                var result = query.Select(h => new
                {
                    h.MaHv,
                    h.HoTen,
                    GioiTinh = h.GioiTinh == true ? "Nam" : "Nữ",
                    NgaySinh = h.NgaySinh.HasValue ? h.NgaySinh.Value.ToString("dd/MM/yyyy") : "",
                    h.Sdt,
                    h.Email,
                    h.HangThanhVien,
                    TrangThai = h.TrangThai == true ? "Đang hoạt động" : "Tạm ngưng"
                }).ToList();

                dgvHoiVien.DataSource = result;
            }
        }

        private void ClearForm()
        {
            selectedMaHV = 0;
            txtHoTen.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            rdoNam.Checked = true;
            dtpNgaySinh.Value = DateTime.Now;
            cboHangThanhVien.SelectedIndex = 0;
            chkTrangThai.Checked = false;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadData();
        }
    }
}