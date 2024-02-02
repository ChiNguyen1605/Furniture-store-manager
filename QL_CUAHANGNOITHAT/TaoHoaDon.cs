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
    public partial class TaoHoaDon : Form
    {
        DB_CuaHangNoiThatDataContext db = new DB_CuaHangNoiThatDataContext();

        BLLHoaDon hd = new BLLHoaDon();
        BLL_SanPham sp = new BLL_SanPham();
        public NhanVien UserAccout { get; set; }
        public TaoHoaDon(NhanVien UserAccout)
        {
            this.UserAccout = UserAccout;
            InitializeComponent();
        }

        private void TaoHoaDon_Load(object sender, EventArgs e)
        {
            loadData();
        }
        private void loadData()
        {

            dtSanPham.DataSource = sp.GetSanPham(0);
            dtChiTietPhieuNhap.Columns.Add("Column1", "Tên sản phẩm");
            dtChiTietPhieuNhap.Columns.Add("Column2", "Số lượng bán");
            dtChiTietPhieuNhap.Columns.Add("Column3", "Đơn giá");
            Random random = new Random();
            string IDSP = "HD" + random.Next(10000, 99999);
            while (hd.checkID(IDSP) == true)
            {
                IDSP = "HD" + random.Next(10000, 99999);
            }
            txtMaPN.Text = IDSP;
            txtNgayLap.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtTenNV.Text = UserAccout.TenNV;
        }
        List<cart> list = new List<cart>();

        public List<cart> push(string MaSP, string TenSP, int SoLuong, int DonGia)
        {
            // Kiểm tra xem sản phẩm đã tồn tại trong danh sách hay chưa
            cart existingProduct = list.FirstOrDefault(p => p.MaSP == MaSP);

            if (existingProduct == null)
            {
                // Nếu sản phẩm chưa có trong danh sách, thêm mới vào danh sách
                cart c = new cart();
                c.MaSP = MaSP;
                c.TenSP = TenSP;
                c.SoLuong = SoLuong;
                c.DonGia = DonGia;
                list.Add(c);
            }
            else
            {
                // Nếu sản phẩm đã có trong danh sách, cộng dồn số lượng
                existingProduct.SoLuong += SoLuong;
            }

            return list;
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

        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã nhập số lượng hợp lệ hay không
            if (!int.TryParse(txtSLNhap.Text, out int soLuongNhap))
            {
                MessageBox.Show("Vui lòng nhập số lượng là một số nguyên.");
                return; // Dừng xử lý nếu nhập không hợp lệ
            }

            // Hiển thị hộp thoại xác nhận
            string thongBao = $"Bạn có chắc là nhập sản phẩm {txtTenSP.Text} và số lượng là {soLuongNhap}?";
            DialogResult result = MessageBox.Show(thongBao, "Xác nhận nhập sản phẩm", MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                // Tiếp tục xử lý khi người dùng bấm OK
                // Kiểm tra số lượng tồn
                if (sp.checkSLTon(txtMaSP.Text) == 0 || sp.checkSLTon(txtMaSP.Text) < soLuongNhap)
                {
                    MessageBox.Show("Số lượng sản phẩm không đủ");
                }
                else
                {
                    if (dtChiTietPhieuNhap.Rows.Count > 0)
                    {
                        dtChiTietPhieuNhap.Rows.Clear();
                        SanPham item = sp.FindSanPham(txtMaSP.Text);
                        if (item != null)
                        {
                            push(item.MaSP, item.TenSP, soLuongNhap, int.Parse(item.DonGia.ToString()));
                        }
                    }
                    double total = 0;
                    foreach (var i in list)
                    {
                        dtChiTietPhieuNhap.Rows.Add(i.TenSP, i.SoLuong, i.DonGia);
                        total += (i.DonGia * i.SoLuong * 1.0);
                    }
                    txtTongTien.Text = total.ToString();
                }
                
            }
            else
            {
                // Xử lý khi người dùng bấm Cancel hoặc đóng hộp thoại
                MessageBox.Show("Bạn đã hủy nhập sản phẩm.");
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Xác nhận nhập hàng", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Tạo mới 1 khách hàng khi mua trực tiếp tại cửa hàng : Lưu thông tin (Tên khách hàng , sdt , địa chỉ)
                KhachHang kh = new KhachHang();
                Random random = new Random();
                string IDKH = "KH" + random.Next(10000, 99999);
                while (hd.checkIDKLH(IDKH) == true)
                {
                    IDKH = "KH" + random.Next(10000, 99999);
                }
                kh.MaKH = IDKH;
                kh.HoTen = txtTenKH.Text;
                kh.DienThoai = txtSDT.Text;
                kh.DiaChi = txtDiaChi.Text;
                hd.insertKhachHangTemp(kh);

                // Tạo mới 1 hóa đơn và chi tiết hóa đơn
                HoaDon insertHoaDon = new HoaDon();
                insertHoaDon.MaHD = txtMaPN.Text;
                insertHoaDon.NgayLap = DateTime.Parse(txtNgayLap.Text);
                insertHoaDon.MaNV = UserAccout.MaNV;
                insertHoaDon.MaKH = kh.MaKH;
                insertHoaDon.TinhTrang = false;

                // Kiểm tra số lượng sản phẩm để áp dụng chương trình khuyến mãi
                int soLuongSanPham = list.Sum(item => item.SoLuong);

                // Truy xuất thông tin khuyến mãi từ cơ sở dữ liệu
                KhuyenMai khuyenMai = GetApplicablePromotion(soLuongSanPham);

                // Tính tổng tiền
                decimal total = (decimal)Total(list);
                decimal giamGia = 0; // Thêm dòng này để khai báo biến giamGia

                if (khuyenMai != null)
                {
                    // Áp dụng giảm giá
                    giamGia = total * (decimal)khuyenMai.GiamGiaPhanTram / 100;
                    total -= giamGia;
                    // Cập nhật giá trị tổng tiền trong đối tượng HoaDon
                    insertHoaDon.TongTien = total;
                    txtTongTien.Text = total.ToString();
                    MessageBox.Show($"Đã áp dụng chương trình khuyến mãi: {khuyenMai.TenKM} - Giảm giá {khuyenMai.GiamGiaPhanTram}%");
                }
                else
                {
                    // Nếu không có chương trình khuyến mãi
                    insertHoaDon.TongTien = total;
                    txtTongTien.Text = total.ToString();
                }

                hd.insertHoaDon(insertHoaDon);

                foreach (var i in list)
                {
                    CTHoaDon cthd = new CTHoaDon();
                    cthd.MaCT = (hd.CountCTHD() + 1);
                    cthd.MaHD = txtMaPN.Text;
                    cthd.MaSP = i.MaSP;
                    cthd.SoLuong = i.SoLuong;
                    hd.insertCTHoaDon(cthd);
                }

                MessageBox.Show("Tạo hóa đơn thành công");
                this.Close();
            }
            else
            {
                return;
            }
        }

        private double Total(List<cart> cartItems)
        {
            return cartItems.Sum(item => item.SoLuong * item.DonGia);
        }

        // Hàm truy xuất thông tin khuyến mãi từ cơ sở dữ liệu
        private KhuyenMai GetApplicablePromotion(int soLuongSanPham)
        {
            // Điều chỉnh câu truy vấn để phản ánh cấu trúc thực tế của cơ sở dữ liệu của bạn
            // Trong ví dụ này, giả sử có một bảng KhuyenMai với các trường MaKM, TenKM, SoLuongToiThieu, SoLuongToiDa, GiamGiaPhanTram

            // Lấy khuyến mãi phù hợp dựa trên số lượng sản phẩm
            return db.KhuyenMais
                .Where(km => soLuongSanPham >= km.SoLuongToiThieu && soLuongSanPham <= km.SoLuongToiDa)
                .OrderByDescending(km => km.GiamGiaPhanTram)
                .FirstOrDefault();
        }
    }

}
