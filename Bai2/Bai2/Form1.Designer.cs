namespace Bai2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblHoTen = new Label();
            txtHoTen = new TextBox();
            lblSDT = new Label();
            txtSDT = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblNgaySinh = new Label();
            dtpNgaySinh = new DateTimePicker();
            lblHang = new Label();
            cboHangThanhVien = new ComboBox();
            chkTrangThai = new CheckBox();
            groupBoxGioiTinh = new GroupBox();
            rdoNu = new RadioButton();
            rdoNam = new RadioButton();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            txtTimKiemHoTen = new TextBox();
            cboTimKiemHang = new ComboBox();
            btnTimKiem = new Button();
            dgvHoiVien = new DataGridView();
            groupBoxGioiTinh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(dgvHoiVien)).BeginInit();
            SuspendLayout();

            // lblHoTen
            lblHoTen.AutoSize = true;
            lblHoTen.Location = new Point(25, 30);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(46, 15);
            lblHoTen.Text = "Họ tên";

            // txtHoTen
            txtHoTen.Location = new Point(140, 27);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(200, 23);

            // lblSDT
            lblSDT.AutoSize = true;
            lblSDT.Location = new Point(25, 70);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new Size(76, 15);
            lblSDT.Text = "Số điện thoại";

            // txtSDT
            txtSDT.Location = new Point(140, 67);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(200, 23);

            // lblEmail
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(25, 110);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(36, 15);
            lblEmail.Text = "Email";

            // txtEmail
            txtEmail.Location = new Point(140, 107);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(200, 23);

            // lblNgaySinh
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Location = new Point(25, 150);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Size = new Size(60, 15);
            lblNgaySinh.Text = "Ngày sinh";

            // dtpNgaySinh
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.Location = new Point(140, 147);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(200, 23);

            // lblHang
            lblHang.AutoSize = true;
            lblHang.Location = new Point(25, 190);
            lblHang.Name = "lblHang";
            lblHang.Size = new Size(95, 15);
            lblHang.Text = "Hạng thành viên";

            // cboHangThanhVien
            cboHangThanhVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHangThanhVien.FormattingEnabled = true;
            cboHangThanhVien.Location = new Point(140, 187);
            cboHangThanhVien.Name = "cboHangThanhVien";
            cboHangThanhVien.Size = new Size(200, 23);

            // chkTrangThai
            chkTrangThai.AutoSize = true;
            chkTrangThai.Location = new Point(140, 227);
            chkTrangThai.Name = "chkTrangThai";
            chkTrangThai.Size = new Size(106, 19);
            chkTrangThai.Text = "Đang hoạt động";
            chkTrangThai.UseVisualStyleBackColor = true;

            // groupBoxGioiTinh
            groupBoxGioiTinh.Controls.Add(rdoNu);
            groupBoxGioiTinh.Controls.Add(rdoNam);
            groupBoxGioiTinh.Location = new Point(360, 27);
            groupBoxGioiTinh.Name = "groupBoxGioiTinh";
            groupBoxGioiTinh.Size = new Size(200, 80);
            groupBoxGioiTinh.TabStop = false;
            groupBoxGioiTinh.Text = "Giới tính";

            // rdoNam
            rdoNam.AutoSize = true;
            rdoNam.Checked = true;
            rdoNam.Location = new Point(20, 35);
            rdoNam.Name = "rdoNam";
            rdoNam.Size = new Size(51, 19);
            rdoNam.TabStop = true;
            rdoNam.Text = "Nam";
            rdoNam.UseVisualStyleBackColor = true;

            // rdoNu
            rdoNu.AutoSize = true;
            rdoNu.Location = new Point(110, 35);
            rdoNu.Name = "rdoNu";
            rdoNu.Size = new Size(41, 19);
            rdoNu.Text = "Nữ";
            rdoNu.UseVisualStyleBackColor = true;

            // btnThem
            btnThem.Location = new Point(580, 27);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(110, 30);
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;

            // btnSua
            btnSua.Location = new Point(580, 67);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(110, 30);
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;

            // btnXoa
            btnXoa.Location = new Point(580, 107);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(110, 30);
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;

            // btnLamMoi
            btnLamMoi.Location = new Point(580, 147);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(110, 30);
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;

            // txtTimKiemHoTen
            txtTimKiemHoTen.Location = new Point(25, 270);
            txtTimKiemHoTen.Name = "txtTimKiemHoTen";
            txtTimKiemHoTen.Size = new Size(250, 23);

            // cboTimKiemHang
            cboTimKiemHang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTimKiemHang.FormattingEnabled = true;
            cboTimKiemHang.Location = new Point(390, 270);
            cboTimKiemHang.Name = "cboTimKiemHang";
            cboTimKiemHang.Size = new Size(150, 23);

            // btnTimKiem
            btnTimKiem.Location = new Point(580, 267);
            btnTimKiem.Name = "btnTimKiem";
            btnTimKiem.Size = new Size(110, 28);
            btnTimKiem.Text = "Tìm kiếm";
            btnTimKiem.UseVisualStyleBackColor = true;

            // dgvHoiVien
            dgvHoiVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHoiVien.Location = new Point(25, 310);
            dgvHoiVien.Name = "dgvHoiVien";
            dgvHoiVien.RowTemplate.Height = 25;
            dgvHoiVien.Size = new Size(735, 180);

            // Form1
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 510);
            Controls.Add(dgvHoiVien);
            Controls.Add(btnTimKiem);
            Controls.Add(cboTimKiemHang);
            Controls.Add(txtTimKiemHoTen);
            Controls.Add(btnLamMoi);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(groupBoxGioiTinh);
            Controls.Add(chkTrangThai);
            Controls.Add(cboHangThanhVien);
            Controls.Add(lblHang);
            Controls.Add(dtpNgaySinh);
            Controls.Add(lblNgaySinh);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(txtSDT);
            Controls.Add(lblSDT);
            Controls.Add(txtHoTen);
            Controls.Add(lblHoTen);
            Name = "Form1";
            Text = "Quản lý Hội viên Phòng tập Gym FitZone";
            groupBoxGioiTinh.ResumeLayout(false);
            groupBoxGioiTinh.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(dgvHoiVien)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblHoTen;
        private TextBox txtHoTen;
        private Label lblSDT;
        private TextBox txtSDT;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblNgaySinh;
        private DateTimePicker dtpNgaySinh;
        private Label lblHang;
        private ComboBox cboHangThanhVien;
        private CheckBox chkTrangThai;
        private GroupBox groupBoxGioiTinh;
        private RadioButton rdoNu;
        private RadioButton rdoNam;
        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private TextBox txtTimKiemHoTen;
        private ComboBox cboTimKiemHang;
        private Button btnTimKiem;
        private DataGridView dgvHoiVien;
    }
}