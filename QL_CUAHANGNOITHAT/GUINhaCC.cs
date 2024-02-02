using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace QL_CUAHANGNOITHAT
{
    public partial class GUINhaCC : UserControl
    {
        DB_CuaHangNoiThatDataContext db = new DB_CuaHangNoiThatDataContext();

        BLL_NhaCungCap ncc = new BLL_NhaCungCap();
        NhanVien UserAccout { get; set; }

        public GUINhaCC(NhanVien UserAccout)
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
       
        private void GUINhaCC_Load(object sender, EventArgs e)
        {
            setActionForm();
            dtNCC.SelectionChanged += dtNCC_SelectionChanged;
            dtNCC.DataSource = ncc.GetNhaCungCap(0);

            if (dtNCC.Rows.Count > 0)
            {
                txtMaNCC.Text = dtNCC.Rows[0].Cells[0].Value.ToString();
                txtTenNCC.Text = dtNCC.Rows[0].Cells[1].Value.ToString();
                txtDiaChi.Text = dtNCC.Rows[0].Cells[2].Value.ToString();
                txtDienThoai.Text = dtNCC.Rows[0].Cells[3].Value.ToString();

                // Thiết lập ReadOnly cho TextBox
                txtMaNCC.ReadOnly = true;
                txtTenNCC.ReadOnly = true;
                txtDiaChi.ReadOnly = true;
                txtDienThoai.ReadOnly = true;
            }
            btnedit.Click += btnedit_Click;
        }
        private void fr_formclosed(object sender, FormClosedEventArgs e)
        {
            dtNCC.DataSource = ncc.GetNhaCungCap(0);
        }
        private void btnadd_Click(object sender, EventArgs e)
        {
            ThemNCC fr = new ThemNCC();
            fr.FormClosed += new FormClosedEventHandler(fr_formclosed);
            fr.ADD = true;
            fr.EDIT = false;
            fr.Show();
        }


        private void dtNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtNCC.Rows[e.RowIndex];

                // Hiển thị thông tin từ dòng được chọn lên các textbox
                txtMaNCC.Text = row.Cells["MaNCC"].Value.ToString();
                txtTenNCC.Text = row.Cells["TenNCC"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtDienThoai.Text = row.Cells["DienThoai"].Value.ToString();
            }
        }

        private void dtNCC_SelectionChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng được chọn không
            if (dtNCC.SelectedRows.Count > 0)
            {
                // Lấy dữ liệu từ dòng được chọn
                DataGridViewRow selectedRow = dtNCC.SelectedRows[0];

                // Hiển thị dữ liệu lên các TextBox
                txtMaNCC.Text = selectedRow.Cells["MaNCC"].Value.ToString();
                txtTenNCC.Text = selectedRow.Cells["TenNCC"].Value.ToString();
                txtDiaChi.Text = selectedRow.Cells["DiaChi"].Value.ToString();
                txtDienThoai.Text = selectedRow.Cells["DienThoai"].Value.ToString();
            }
        }

       
        private void LoadData()
        {
            // Giả sử có một phương thức GetNhaCungCap để lấy dữ liệu từ cơ sở dữ liệu
            // và gán nó vào DataGridView dtNCC
            dtNCC.DataSource = GetNhaCungCap();
        }

        public List<NhaCungCap> GetNhaCungCap()
        {
            try
            {
                // Lấy danh sách nhà cung cấp từ cơ sở dữ liệu
                List<NhaCungCap> listNhaCungCap = db.NhaCungCaps.ToList();

                // Trả về danh sách nhà cung cấp
                return listNhaCungCap;
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có
                return null;
            }
        }
        private void btndel_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn không
            if (dtNCC.SelectedRows.Count > 0)
            {
                // Lấy mã nhà cung cấp từ dòng được chọn
                int maNCC = int.Parse(dtNCC.SelectedRows[0].Cells["MaNCC"].Value.ToString());

                // Kiểm tra xem có xóa thành công hay không
                if (ncc.deleteNhaCungCap(maNCC))
                {
                    MessageBox.Show("Xoá thành công");
                    LoadData(); // Load lại dữ liệu sau khi xóa
                }
                else
                {
                    MessageBox.Show("Xoá thất bại");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp để xoá.");
            }
        }

        

        private void btnfind_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có nhập mã hoặc tên không
            if (!string.IsNullOrEmpty(txtfind.Text))
            {
                int maNCC = 0;
                if (int.TryParse(txtfind.Text, out maNCC))
                {
                    // Nếu nhập vào là số (mã), thực hiện tìm kiếm theo mã
                    List<NhaCungCap> result = ncc.SearchNhaCungCap(maNCC, "");
                    DisplaySearchResult(result);
                }
                else
                {
                    // Nếu nhập vào không phải số (tên), thực hiện tìm kiếm theo tên
                    List<NhaCungCap> result = ncc.SearchNhaCungCap(0, txtfind.Text);
                    DisplaySearchResult(result);
                }
            }
            else
            {
                // Nếu không nhập gì thì hiển thị tất cả
                dtNCC.DataSource = ncc.GetNhaCungCap(0);
            }
        }
        private void DisplaySearchResult(List<NhaCungCap> result)
        {
            if (result != null && result.Count > 0)
            {
                // Hiển thị kết quả tìm kiếm đầu tiên lên các TextBox
                txtMaNCC.Text = result[0].MaNCC.ToString();
                txtTenNCC.Text = result[0].TenNCC;
                txtDiaChi.Text = result[0].DiaChi;
                txtDienThoai.Text = result[0].DienThoai;

                // Hiển thị kết quả tìm kiếm trong DataGridView
                dtNCC.DataSource = result;
            }
            else
            {
                MessageBox.Show("Không tìm thấy kết quả nào.");
            }
        }

        private void DisableEditMode()
        {
            // Không cho phép chỉnh sửa các TextBox
            txtMaNCC.ReadOnly = true;
            txtTenNCC.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            txtDienThoai.ReadOnly = true;

            // Thay đổi trạng thái các nút
            btnedit.Enabled = true; // Cho phép chỉnh sửa sau khi đã lưu
            btnsave.Enabled = false; // Vô hiệu hóa nút lưu
        }
        private void SaveChanges()
        {
            // Thực hiện lưu các thay đổi vào cơ sở dữ liệu hoặc nơi bạn cần
            int maNCC = int.Parse(txtMaNCC.Text);
            string tenNCC = txtTenNCC.Text;
            string diaChi = txtDiaChi.Text;
            string dienThoai = txtDienThoai.Text;

            // Gọi phương thức để cập nhật nhà cung cấp trong cơ sở dữ liệu
            if (ncc.UpdateNhaCungCap(maNCC, tenNCC, diaChi, dienThoai))
            {
                MessageBox.Show("Lưu thành công");
            }
            else
            {
                MessageBox.Show("Lưu thất bại");
            }
        }
        private void btnedit_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn nhà cung cấp nào chưa
            if (dtNCC.SelectedRows.Count > 0)
            {
                // Kích hoạt chế độ chỉnh sửa
                EnableEditMode();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp để chỉnh sửa.");
            }
        }
        private void EnableEditMode()
        {
            // Cho phép chỉnh sửa các TextBox
            txtMaNCC.ReadOnly = false;
            txtTenNCC.ReadOnly = false;
            txtDiaChi.ReadOnly = false;
            txtDienThoai.ReadOnly = false;

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
    }
}
