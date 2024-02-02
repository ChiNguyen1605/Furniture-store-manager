using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_CUAHANGNOITHAT
{
    public partial class ThemNCC : Form
    {
        BLL_NhaCungCap ncc = new BLL_NhaCungCap();
        public bool ADD { get; set; }
        public bool EDIT { get; set; }
        public ThemNCC()
        {
            InitializeComponent();
        }

        private void ThemNCC_Load(object sender, EventArgs e)
        {
            if (ADD == true)
            {
                NhaCungCap newNCC = new NhaCungCap();

            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (ADD == true)
            {
                // Tạo mới một đối tượng nhà cung cấp và gán giá trị từ giao diện cho các trường của đối tượng newNCC.
                NhaCungCap newNCC = new NhaCungCap();
                newNCC.TenNCC = txtTenNCC.Text;
                newNCC.DiaChi = txtDiaChi.Text;
                newNCC.DienThoai = txtDienThoai.Text;

                // Thêm nhà cung cấp vào cơ sở dữ liệu
                bool success = ncc.insertNhaCungCap(newNCC);

                if (success)
                {
                    MessageBox.Show("Thêm nhà cung cấp thành công. Mã nhà cung cấp mới: " + newNCC.MaNCC);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm nhà cung cấp thất bại.");
                }
            }
        }
    }
}
