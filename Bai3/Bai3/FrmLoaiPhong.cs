using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Bai3.Models;

namespace Bai3
{
    public partial class FrmLoaiPhong : Form
    {
        private SunriseHomestayDBContext _context = new SunriseHomestayDBContext();
        private int selectedMaLoai = 0;

        public FrmLoaiPhong()
        {
            InitializeComponent();
        }

        private void FrmLoaiPhong_Load(object sender, EventArgs e)
        {
            dgvLoaiPhong.AutoGenerateColumns = false;
            dgvLoaiPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvLoaiPhong.Columns.Clear();
            dgvLoaiPhong.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaLoai",
                HeaderText = "Mã loại",
                DataPropertyName = "MaLoai",
                FillWeight = 20
            });
            dgvLoaiPhong.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenLoai",
                HeaderText = "Tên loại",
                DataPropertyName = "TenLoai",
                FillWeight = 40
            });
            dgvLoaiPhong.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaMoiDem",
                HeaderText = "Giá/đêm",
                DataPropertyName = "GiaMoiDem",
                FillWeight = 30,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
            dgvLoaiPhong.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MoTa",
                HeaderText = "Mô tả",
                DataPropertyName = "MoTa",
                FillWeight = 60
            });

            LoadData();
        }

        private void LoadData()
        {
            dgvLoaiPhong.DataSource = _context.LoaiPhongs
                .AsNoTracking()
                .OrderBy(l => l.MaLoai)
                .ToList();
            dgvLoaiPhong.ClearSelection();
        }

        private void dgvLoaiPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var lp = dgvLoaiPhong.Rows[e.RowIndex].DataBoundItem as LoaiPhong;
            if (lp == null) return;

            selectedMaLoai = lp.MaLoai;
            txtTenLoai.Text = lp.TenLoai;
            numGia.Value = Convert.ToDecimal(lp.GiaMoiDem);
            txtMoTa.Text = lp.MoTa;
        }

        private bool ValidateInput(int maLoaiHienTai = 0)
        {
            if (string.IsNullOrWhiteSpace(txtTenLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập tên loại phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenLoai.Focus();
                return false;
            }

            string ten = txtTenLoai.Text.Trim();
            if (_context.LoaiPhongs.Any(l => l.TenLoai == ten && l.MaLoai != maLoaiHienTai))
            {
                MessageBox.Show("Tên loại phòng này đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenLoai.Focus();
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            var lp = new LoaiPhong
            {
                TenLoai = txtTenLoai.Text.Trim(),
                GiaMoiDem = numGia.Value,
                MoTa = txtMoTa.Text.Trim()
            };

            _context.LoaiPhongs.Add(lp);
            _context.SaveChanges();

            MessageBox.Show("Thêm loại phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
            ClearForm();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedMaLoai == 0)
            {
                MessageBox.Show("Vui lòng chọn loại phòng cần sửa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInput(selectedMaLoai)) return;

            var lp = _context.LoaiPhongs.Find(selectedMaLoai);
            if (lp == null) return;

            lp.TenLoai = txtTenLoai.Text.Trim();
            lp.GiaMoiDem = numGia.Value;
            lp.MoTa = txtMoTa.Text.Trim();

            _context.SaveChanges();

            MessageBox.Show("Cập nhật loại phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
            ClearForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedMaLoai == 0)
            {
                MessageBox.Show("Vui lòng chọn loại phòng cần xóa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ràng buộc khóa ngoại: không xóa loại đang có phòng
            if (_context.Phongs.Any(p => p.MaLoai == selectedMaLoai))
            {
                MessageBox.Show("Không thể xóa: loại phòng này đang có phòng sử dụng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa loại phòng này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            var lp = _context.LoaiPhongs.Find(selectedMaLoai);
            if (lp == null) return;

            _context.LoaiPhongs.Remove(lp);
            _context.SaveChanges();

            MessageBox.Show("Xóa loại phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
            ClearForm();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadData();
        }

        private void ClearForm()
        {
            selectedMaLoai = 0;
            txtTenLoai.Clear();
            numGia.Value = 0;
            txtMoTa.Clear();
            dgvLoaiPhong.ClearSelection();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _context.Dispose();
            base.OnFormClosed(e);
        }
    }
}