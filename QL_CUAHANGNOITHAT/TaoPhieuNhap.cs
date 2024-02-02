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
    public partial class TaoPhieuNhap : Form
    {
        BLL_PhieuNhap pn = new BLL_PhieuNhap();
        BLL_SanPham sp = new BLL_SanPham();
        public NhanVien UserAccout { get; set; }
        public TaoPhieuNhap(NhanVien UserAccout)
        {
            this.UserAccout = UserAccout;
            InitializeComponent();
        }


        private void TaoPhieuNhap_Load(object sender, EventArgs e)
        {

            dtSanPham.DataSource = sp.GetSanPham(0);
            dtChiTietPhieuNhap.Columns.Add("Column1", "Tên sản phẩm");
            dtChiTietPhieuNhap.Columns.Add("Column2", "Số lượng nhập");
            Random random = new Random();
            string IDSP = "PN" + random.Next(10000, 99999);
            while (pn.checkID(IDSP) == false)
            {
                IDSP = "PN" + random.Next(10000, 99999);
            }
            txtMaPN.Text = IDSP;
            txtNgayLap.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtTenNV.Text = UserAccout.TenNV;

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

        }

        private void dtSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = dtSanPham.SelectedCells[0].RowIndex;
            string selectedValue = dtSanPham.Rows[selectedRowIndex].Cells[0].Value.ToString();

            SanPham item = sp.FindSanPham(selectedValue);
            if (item != null)
            {
                txtMaSP.Text = item.MaSP;
                txtTenSP.Text = item.TenSP;
                txtSoLuong.Text = item.SoLuongTon.ToString();
            }
        }

        List<cart> list = new List<cart>();
        
        public List<cart> push(string MaSP, string TenSP, int SoLuong,int GiaNhap)
        {
            if (list.Count() == 0)
            {
                cart c = new cart();
                c.MaSP = MaSP;
                c.TenSP = TenSP;
                c.SoLuong = SoLuong;
                c.GiaNhap = GiaNhap;
                list.Add(c);
            }
            else
            {
                if (list.Where(r => r.MaSP == MaSP).FirstOrDefault() == null)
                {
                    cart c = new cart();
                    c.MaSP = MaSP;
                    c.TenSP = TenSP;
                    c.SoLuong = SoLuong;
                    c.GiaNhap = GiaNhap;
                    list.Add(c);
                }
                else
                {
                    cart c = list.Where(r => r.MaSP == MaSP).FirstOrDefault();
                    c.SoLuong = SoLuong;
                }
            }
            return list;
        }
        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Bạn có chắc muốn nhập hàng \"{txtTenSP.Text}\" với số lượng {txtSLNhap.Text} không?",
                                          "Xác nhận nhập hàng", MessageBoxButtons.OKCancel);

            // Kiểm tra xem người dùng đã nhấn OK hay không
            if (result == DialogResult.OK)
            {
                if (dtChiTietPhieuNhap.Rows.Count > 0)
                {
                    dtChiTietPhieuNhap.Rows.Clear();
                    SanPham item = sp.FindSanPham(txtMaSP.Text);
                    if (item != null)
                    {
                        int soluongNhap = int.Parse(txtSoLuong.Text) - int.Parse(item.SoLuongTon.ToString());
                        push(item.MaSP, item.TenSP, int.Parse(txtSLNhap.Text), int.Parse(item.GiaNhap.ToString())); // Sử dụng giá nhập
                    }
                }
                double total = 0;
                foreach (var i in list)
                {
                    dtChiTietPhieuNhap.Rows.Add(i.TenSP, i.SoLuong);
                    total += (i.GiaNhap * i.SoLuong * 1.0); // Sử dụng giá nhập
                }
                txtTongTien.Text = total.ToString();
            }
        }

        private void btnCreate_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Xác nhận nhập hàng", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                PhieuNhap insertPhieuNhap = new PhieuNhap();
                insertPhieuNhap.MaPN = txtMaPN.Text;
                insertPhieuNhap.NgayLap = DateTime.Parse(txtNgayLap.Text);
                insertPhieuNhap.MaNV = UserAccout.MaNV;
                insertPhieuNhap.TongTien = int.Parse(txtTongTien.Text);
                pn.insertPhieuNhap(insertPhieuNhap);
                foreach (var i in list)
                {
                    CTPhieuNhap ctpn = new CTPhieuNhap();
                    ctpn.MaCT = ""+(pn.CountCTPN() + 1);
                    ctpn.MaPN = txtMaPN.Text;
                    ctpn.MaSP = i.MaSP;
                    ctpn.SoLuong = i.SoLuong;
                    pn.insertCTPhieuNhap(ctpn);
                }
                this.Close();
            }
            else
            {
                return;
            }
        }

    }
    public class cart
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int GiaNhap { get; set; } // Thêm thuộc tính GiaNhap
    }
}
