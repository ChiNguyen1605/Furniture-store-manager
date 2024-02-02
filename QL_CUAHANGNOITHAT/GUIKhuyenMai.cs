using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
namespace QL_CUAHANGNOITHAT
{
    public partial class GUIKhuyenMai : UserControl
    {
        DB_CuaHangNoiThatDataContext db = new DB_CuaHangNoiThatDataContext();

        BLL_KhuyenMai km = new BLL_KhuyenMai();
        NhanVien UserAccout { get; set; }
        public GUIKhuyenMai(NhanVien UserAccout)
        {
            this.UserAccout = UserAccout;

            InitializeComponent();
        }
        private void setActionForm()
        {
            if (UserAccout.MaNhom == "User")
            {
                btnadd.Visible = false;
                btnedit.Visible = false;
                btndel.Visible = false;
                btnsave.Visible = false;

            }
        }

        private void GUIKhuyenMai_Load(object sender, EventArgs e)
        {
            // Thiết lập các sự kiện và load dữ liệu
            setActionForm();
            dtKM.SelectionChanged += dtKM_SelectionChanged;

            // Khởi tạo đối tượng BLL_KhuyenMai
            BLL_KhuyenMai BLL_KhuyenMai = new BLL_KhuyenMai();

            // Lấy danh sách khuyến mãi và gán vào DataGridView
            dtKM.DataSource = BLL_KhuyenMai.GetKhuyenMai(0);

            if (dtKM.Rows.Count > 0)
            {
                // Gán giá trị từ dòng đầu tiên cho các TextBox tương ứng
                txtMaKM.Text = dtKM.Rows[0].Cells["MaKM"].Value.ToString();
                txtTenKM.Text = dtKM.Rows[0].Cells["TenKM"].Value.ToString();
                txtSoLuongToiThieu.Text = dtKM.Rows[0].Cells["SoLuongToiThieu"].Value.ToString();
                txtSoLuongToiDa.Text = dtKM.Rows[0].Cells["SoLuongToiDa"].Value.ToString();
                dtNgayBatDau.Value = Convert.ToDateTime(dtKM.Rows[0].Cells["NgayBatDau"].Value);
                dtNgayKetThuc.Value = Convert.ToDateTime(dtKM.Rows[0].Cells["NgayKetThuc"].Value);
                txtPhanTramGiam.Text = dtKM.Rows[0].Cells["GiamGiaPhanTram"].Value.ToString();

                // Thiết lập ReadOnly cho các TextBox
                txtMaKM.ReadOnly = true;
                txtTenKM.ReadOnly = true;
                txtSoLuongToiThieu.ReadOnly = true;
                txtSoLuongToiDa.ReadOnly = true;
                dtNgayBatDau.Enabled = false; // Ngày bắt đầu không cho sửa
                dtNgayKetThuc.Enabled = false; // Ngày kết thúc không cho sửa
                txtPhanTramGiam.ReadOnly = true;
            }

            // Gán sự kiện cho nút Edit
           btnedit.Click += btnedit_Click;
        }

        private void dtKM_SelectionChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng được chọn không
            if (dtKM.SelectedRows.Count > 0)
            {
                // Lấy dòng được chọn
                DataGridViewRow selectedRow = dtKM.SelectedRows[0];

                // Hiển thị thông tin của dòng được chọn trong các TextBox tương ứng

                txtMaKM.Text = selectedRow.Cells["MaKM"].Value?.ToString();
                txtTenKM.Text = selectedRow.Cells["TenKM"].Value?.ToString();
                txtSoLuongToiThieu.Text = selectedRow.Cells["SoLuongToiThieu"].Value?.ToString();

                // Kiểm tra giá trị trước khi gán vào TextBox
                object soLuongToiDaValue = selectedRow.Cells["SoLuongToiDa"].Value;
                txtSoLuongToiDa.Text = soLuongToiDaValue != null ? soLuongToiDaValue.ToString() : "";

                // Tương tự, kiểm tra giá trị trước khi gán vào DateTimePicker
                object ngayBatDauValue = selectedRow.Cells["NgayBatDau"].Value;
                dtNgayBatDau.Value = ngayBatDauValue != null ? Convert.ToDateTime(ngayBatDauValue) : DateTime.Now;

                object ngayKetThucValue = selectedRow.Cells["NgayKetThuc"].Value;
                dtNgayKetThuc.Value = ngayKetThucValue != null ? Convert.ToDateTime(ngayKetThucValue) : DateTime.Now;

                object phanTramGiamValue = selectedRow.Cells["GiamGiaPhanTram"].Value;
                txtPhanTramGiam.Text = phanTramGiamValue != null ? phanTramGiamValue.ToString() : "";
            }
        }
        private void LoadData()
        {
            
            dtKM.DataSource = GetKhuyenMai();
        }

        private object GetKhuyenMai()
        {
            return db.KhuyenMais.Select(r => r);
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn nhà cung cấp nào chưa
            if (dtKM.SelectedRows.Count > 0)
            {
                // Kích hoạt chế độ chỉnh sửa
                EnableEditMode();
            }
            else
            {
                MessageBox.Show("Chọn chương trình khuyến mãi cần sửa");
            }
        }

        private void EnableEditMode()
        {
            // Cho phép chỉnh sửa các TextBox
            txtMaKM.ReadOnly = false;
            txtTenKM.ReadOnly = false;
            txtTenKM.ReadOnly = false;
            txtSoLuongToiDa.ReadOnly = false;
            txtSoLuongToiDa.ReadOnly = false;
            dtNgayBatDau.Enabled = false;
            dtNgayKetThuc.Enabled = false;
            txtPhanTramGiam.ReadOnly = false;

            // Thay đổi trạng thái các nút
            btnedit.Enabled = false; // Chỉ cho phép chỉnh sửa một lần
            btnsave.Enabled = true; // Cho phép lưu sau khi chỉnh sửa
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            ThemKhuyenMai fr = new ThemKhuyenMai();
            fr.FormClosed += new FormClosedEventHandler(fr_formclosed);
            fr.ADD = true;
            fr.EDIT = false;
            fr.Show();
        }
        private void fr_formclosed(object sender, FormClosedEventArgs e)
        {
            dtKM.DataSource = km.GetKhuyenMai(0);
        }
        private void dtKM_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem người dùng có nhấp vào dòng (không phải tiêu đề cột) không
            if (e.RowIndex >= 0)
            {
                // Lấy dòng được chọn
                DataGridViewRow selectedRow = dtKM.Rows[e.RowIndex];

                // Hiển thị thông tin của dòng được chọn trong các TextBox tương ứng
                txtMaKM.Text = selectedRow.Cells["MaKM"].Value?.ToString();
                txtTenKM.Text = selectedRow.Cells["TenKM"].Value?.ToString();
                txtSoLuongToiThieu.Text = selectedRow.Cells["SoLuongToiThieu"].Value?.ToString();

                // Kiểm tra giá trị trước khi gán vào TextBox
                object soLuongToiDaValue = selectedRow.Cells["SoLuongToiDa"].Value;
                txtSoLuongToiDa.Text = soLuongToiDaValue != null ? soLuongToiDaValue.ToString() : "";

                // Tương tự, kiểm tra giá trị trước khi gán vào DateTimePicker
                object ngayBatDauValue = selectedRow.Cells["NgayBatDau"].Value;
                dtNgayBatDau.Value = ngayBatDauValue != null && ngayBatDauValue != DBNull.Value
                    ? Convert.ToDateTime(ngayBatDauValue)
                    : dtNgayBatDau.MinDate;

                object ngayKetThucValue = selectedRow.Cells["NgayKetThuc"].Value;
                dtNgayKetThuc.Value = ngayKetThucValue != null && ngayKetThucValue != DBNull.Value
                    ? Convert.ToDateTime(ngayKetThucValue)
                    : dtNgayKetThuc.MinDate;

                object phanTramGiamValue = selectedRow.Cells["GiamGiaPhanTram"].Value;
                txtPhanTramGiam.Text = phanTramGiamValue != null ? phanTramGiamValue.ToString() : "";
            }
        }
        private void EnableEditModeKM()
        {
            // Cho phép chỉnh sửa các TextBox và DateTimePicker
            txtMaKM.ReadOnly = false;
            txtTenKM.ReadOnly = false;
            txtSoLuongToiThieu.ReadOnly = false;
            txtSoLuongToiDa.ReadOnly = false;
            dtNgayBatDau.Enabled = true;
            dtNgayKetThuc.Enabled = true;
            txtPhanTramGiam.ReadOnly = false;

            // Thay đổi trạng thái các nút
            btnedit.Enabled = false; // Chỉ cho phép chỉnh sửa một lần
            btnsave.Enabled = true; // Cho phép lưu sau khi chỉnh sửa
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            // Lưu các thay đổi vào cơ sở dữ liệu hoặc nơi bạn cần
            SaveChanges();

            // Tắt chế độ chỉnh sửa
            DisableEditMode();
        }

        private void DisableEditMode()
        {
            // Không cho phép chỉnh sửa các TextBox và DateTimePicker
            txtMaKM.ReadOnly = true;
            txtTenKM.ReadOnly = true;
            txtSoLuongToiThieu.ReadOnly = true;
            txtSoLuongToiDa.ReadOnly = true;
            dtNgayBatDau.Enabled = false;
            dtNgayKetThuc.Enabled = false;
            txtPhanTramGiam.ReadOnly = true;

            // Thay đổi trạng thái các nút
            btnedit.Enabled = true; // Cho phép chỉnh sửa sau khi đã lưu
            btnsave.Enabled = false; // Vô hiệu hóa nút lưu
        }

        private void SaveChanges()
        {
            // Lấy thông tin từ giao diện người dùng
            int maKM = int.Parse(txtMaKM.Text);
            string tenKM = txtTenKM.Text;
            int soLuongToiThieu = int.Parse(txtSoLuongToiThieu.Text);
            int soLuongToiDa = int.Parse(txtSoLuongToiDa.Text);
            DateTime ngayBatDau = dtNgayBatDau.Value;
            DateTime ngayKetThuc = dtNgayKetThuc.Value;
            decimal phanTramGiam = decimal.Parse(txtPhanTramGiam.Text);

            // Tạo một đối tượng BLL_KhuyenMai
            BLL_KhuyenMai khuyenMaiBLL = new BLL_KhuyenMai();

            // Gọi phương thức cập nhật từ đối tượng BLL_KhuyenMai
            if (khuyenMaiBLL.UpdateKhuyenMai(maKM, tenKM, soLuongToiThieu, soLuongToiDa, ngayBatDau, ngayKetThuc, phanTramGiam))
            {
                MessageBox.Show("Lưu thành công");
            }
            else
            {
                MessageBox.Show("Lưu thất bại");
            }
        }
        private bool CheckDuplicate(int soLuongToiThieu, int soLuongToiDa)
        {
            // Lấy danh sách khuyến mãi từ cơ sở dữ liệu
            List<KhuyenMai> khuyenMais = (List<KhuyenMai>)km.GetKhuyenMai(0);

            // Kiểm tra xem có khuyến mãi nào có số lượng tối thiểu và tối đa giống với dữ liệu nhập vào không
            return khuyenMais.Any(km => km.SoLuongToiThieu == soLuongToiThieu && km.SoLuongToiDa == soLuongToiDa);
        }

        private void btndel_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn chương trình khuyến mãi nào để xóa chưa
            if (dtKM.SelectedRows.Count > 0)
            {
                // Lấy mã khuyến mãi từ dòng được chọn
                int maKM = int.Parse(dtKM.SelectedRows[0].Cells["MaKM"].Value.ToString());

                // Hiển thị hộp thoại xác nhận xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa chương trình khuyến mãi này?", "Xác nhận xóa", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // Gọi phương thức xóa từ đối tượng BLL_KhuyenMai
                    bool success = km.DeleteKhuyenMai(maKM);

                    if (success)
                    {
                        MessageBox.Show("Xóa khuyến mãi thành công");
                        LoadData(); // Reload dữ liệu sau khi xóa
                    }
                    else
                    {
                        MessageBox.Show("Xóa khuyến mãi thất bại");
                    }
                }
            }
            else
            {
                MessageBox.Show("Chọn chương trình khuyến mãi cần xóa");
            }
        }
    }
}

      
