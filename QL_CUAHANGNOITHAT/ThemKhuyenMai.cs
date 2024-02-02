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
    public partial class ThemKhuyenMai : Form
    {
        BLL_KhuyenMai km = new BLL_KhuyenMai();
        public bool ADD { get; set; }
        public bool EDIT { get; set; }
        public ThemKhuyenMai()
        {
            InitializeComponent();
            // Gọi hàm để lấy mã khuyến mãi mới
            int newMaKM = GetNewMaKhuyenMai();

            // Ẩn TextBox mã khuyến mãi và hiển thị mã đã được tạo
            txtMaKM.Visible = false;
            lblMaKMValue.Text = newMaKM.ToString();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (ADD == true)
            {
                // Tạo mới một đối tượng khuyến mãi và gán giá trị từ giao diện cho các trường của đối tượng newKhuyenMai.
                KhuyenMai newKhuyenMai = new KhuyenMai();
                newKhuyenMai.MaKM = int.Parse(lblMaKMValue.Text); // Sử dụng giá trị từ Label
                newKhuyenMai.TenKM = txtTenKM.Text;
                newKhuyenMai.SoLuongToiThieu = int.Parse(txtSoLuongToiThieu.Text);
                newKhuyenMai.SoLuongToiDa = int.Parse(txtSoLuongToiDa.Text);
                newKhuyenMai.NgayBatDau = dtNgayBatDau.Value;
                newKhuyenMai.NgayKetThuc = dtNgayKetThuc.Value;
                newKhuyenMai.GiamGiaPhanTram = decimal.Parse(txtPhanTramGiam.Text);

                // Thêm khuyến mãi vào cơ sở dữ liệu
                bool success = km.InsertKhuyenMai(newKhuyenMai);

                if (success)
                {
                    MessageBox.Show("Thêm khuyến mãi thành công. Mã khuyến mãi mới: " + newKhuyenMai.MaKM);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm khuyến mãi thất bại.");
                }
            }
        }
        private int GetNewMaKhuyenMai()
        {
            // Gọi đối tượng BLL_KhuyenMai để lấy mã khuyến mãi mới
            int newMaKM = km.GetNewMaKhuyenMai();
            return newMaKM;
        }
    }
}
