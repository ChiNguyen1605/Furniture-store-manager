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
    public partial class PNPhieuNhap : UserControl
    {
        BLL_PhieuNhap pn = new BLL_PhieuNhap();
        public NhanVien UserAccout { get; set; }
        public PNPhieuNhap(NhanVien UserAccout)
        {
            this.UserAccout= UserAccout;
            InitializeComponent();
        }



        private void PNPhieuNhap_Load(object sender, EventArgs e)
        {
            dtPhieuNhap.DataSource = pn.GetPhieuNhap("");
        }

        private void dtPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int selectedRowIndex = dtPhieuNhap.SelectedCells[0].RowIndex;
                string selectedValue = dtPhieuNhap.Rows[selectedRowIndex].Cells[0].Value.ToString();

                PhieuNhap phieuNhap = pn.FindPhieuNhap(selectedValue);

                txtMaPN.Text = phieuNhap.MaPN;
                txtTenNV.Text = phieuNhap.NhanVien.TenNV;
                txtNgayLap.Text = phieuNhap.NgayLap.ToString();

                dtChiTietPhieuNhap.DataSource = pn.GetCTPhieuNhap(phieuNhap.MaPN);
            }
            catch (Exception)
            {
                return;
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Xác nhận hủy phiếu nhập hàng", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                PhieuNhap phieuNhap = pn.FindPhieuNhap(txtMaPN.Text);
                foreach (CTPhieuNhap ctphieuNhap in phieuNhap.CTPhieuNhaps)
                {
                    BLL_SanPham sp = new BLL_SanPham();
                    SanPham sanPham = sp.FindSanPham(ctphieuNhap.MaSP);
                    sp.UpdateSoLuongTonByMaSP(ctphieuNhap.MaSP, sanPham.SoLuongTon - ctphieuNhap.SoLuong ?? 0);
                }

                if (pn.deletePhieuNhap(txtMaPN.Text))
                {
                    MessageBox.Show("Hủy phiếu nhập hàng thành công");
                    // Làm mới dữ liệu trong DataGridView
                    dtPhieuNhap.DataSource = pn.GetPhieuNhap("");
                }
                else
                {
                    MessageBox.Show("Hủy phiếu nhập hàng thất bại");
                }
            }
            else
            {
                return;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            TaoPhieuNhap fr = new TaoPhieuNhap(UserAccout);
            fr.FormClosed += new FormClosedEventHandler(TaoPhieuNhap_Closed);
            fr.Show();
        }
        private void TaoPhieuNhap_Closed(object sender, FormClosedEventArgs e)
        {
            dtPhieuNhap.DataSource = pn.GetPhieuNhap("");
        }
    }
}
