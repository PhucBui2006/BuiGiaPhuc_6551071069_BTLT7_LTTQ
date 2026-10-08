using Bai1.Models;

namespace Bai1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (var db = new TriThucBooksDbContext())
            {
                dgvTheLoai.DataSource = db.TheLoaiSaches.ToList();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        //==========================================

        //===================================================

        //==============================

        //==========================================

        //=========================================================

        //========================================================


        private void btnThem_Click_1(object sender, EventArgs e)
        {
            // Validate: không được bỏ trống Tên thể loại
            if (string.IsNullOrWhiteSpace(txtTenTheLoai.Text))
            {
                MessageBox.Show("Tên thể loại không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new TriThucBooksDbContext())
            {
                // Validate: không cho phép trùng tên thể loại khi Thêm mới
                string tenMoi = txtTenTheLoai.Text.Trim();
                if (db.TheLoaiSaches.Any(t => t.TenTheLoai == tenMoi))
                {
                    MessageBox.Show("Tên thể loại này đã tồn tại trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var item = new TheLoaiSach
                {
                    TenTheLoai = tenMoi,
                    MoTa = txtMoTa.Text.Trim(),
                    SoLuongSach = 0,
                    NgayTao = DateTime.Now
                };

                db.TheLoaiSaches.Add(item);
                db.SaveChanges();

                MessageBox.Show("Thêm thể loại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                btnLamMoi.PerformClick();
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaTL.Text)) return;

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa thể loại này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                int id = int.Parse(txtMaTL.Text);
                using (var db = new TriThucBooksDbContext())
                {
                    try
                    {
                        var item = db.TheLoaiSaches.Find(id);
                        if (item != null)
                        {
                            db.TheLoaiSaches.Remove(item);
                            db.SaveChanges();
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            btnLamMoi.PerformClick();
                        }
                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                    {
                        // Bắt lỗi DbUpdateException khi thể loại đang được tham chiếu bởi bảng khác
                        MessageBox.Show("Không thể xóa vì thể loại này đang được sách khác tham chiếu!", "Lỗi ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaTL.Text)) return;

            int id = int.Parse(txtMaTL.Text);
            using (var db = new TriThucBooksDbContext())
            {
                var item = db.TheLoaiSaches.Find(id);
                if (item != null)
                {
                    item.TenTheLoai = txtTenTheLoai.Text.Trim();
                    item.MoTa = txtMoTa.Text.Trim();

                    db.SaveChanges();
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTL.Clear();
            txtTenTheLoai.Clear();
            txtMoTa.Clear();
            lblNgayTao.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            txtTenTheLoai.Focus();
        }

        private void dgvTheLoai_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTheLoai.CurrentRow != null && dgvTheLoai.CurrentRow.DataBoundItem is TheLoaiSach selected)
            {
                txtMaTL.Text = selected.MaTl.ToString();
                txtTenTheLoai.Text = selected.TenTheLoai;
                txtMoTa.Text = selected.MoTa;
                lblNgayTao.Text = selected.NgayTao?.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            using (var db = new TriThucBooksDbContext())
            {
                var result = db.TheLoaiSaches
                               .Where(t => t.TenTheLoai.Contains(keyword))
                               .ToList();
                dgvTheLoai.DataSource = result;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();

            using (var db = new TriThucBooksDbContext())
            {
                var result = db.TheLoaiSaches
                               .Where(t => t.TenTheLoai.Contains(keyword) || (t.MoTa != null && t.MoTa.Contains(keyword)))
                               .ToList();

                dgvTheLoai.DataSource = result;
                if (result.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy thể loại nào phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
