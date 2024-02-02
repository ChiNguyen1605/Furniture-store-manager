using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_KhuyenMai
    {
        DB_CuaHangNoiThatDataContext db = new DB_CuaHangNoiThatDataContext();
        public List<KhuyenMai> SearchKhuyenMai(int maKM, string tenKM)
        {
            try
            {
                // Tìm kiếm nhà cung cấp theo mã và tên
                var result = from km in db.KhuyenMais
                             where km.MaKM == maKM || km.TenKM.Contains(tenKM)
                             select km;

                // Trả về danh sách nhà cung cấp tìm được
                return result.ToList();
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có
                return null;
            }
        }
        public object GetKhuyenMai(int v)
        {
            return db.KhuyenMais.Select(r => r);
        }
        public int GetNewMaKhuyenMai()
        {
            // Lấy mã khuyến mãi mới từ cơ sở dữ liệu
            int newMaKM = 1;

            // Kiểm tra nếu có khuyến mãi nào trong danh sách thì lấy mã lớn nhất và tăng lên 1
            if (db.KhuyenMais.Any())
            {
                newMaKM = db.KhuyenMais.Max(km => km.MaKM) + 1;
            }

            return newMaKM;
        }
        public bool UpdateKhuyenMai(int maKM, string tenKM, int soLuongToiThieu, int soLuongToiDa, DateTime ngayBatDau, DateTime ngayKetThuc, decimal phanTramGiam)
        {
            try
            {
                // Lấy đối tượng KhuyenMai từ cơ sở dữ liệu
                KhuyenMai khuyenMai = db.KhuyenMais.SingleOrDefault(km => km.MaKM == maKM);

                if (khuyenMai != null)
                {
                    // Cập nhật thông tin cho đối tượng KhuyenMai
                    khuyenMai.TenKM = tenKM;
                    khuyenMai.SoLuongToiThieu = soLuongToiThieu;
                    khuyenMai.SoLuongToiDa = soLuongToiDa;
                    khuyenMai.NgayBatDau = ngayBatDau;
                    khuyenMai.NgayKetThuc = ngayKetThuc;
                    khuyenMai.GiamGiaPhanTram = phanTramGiam;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.SubmitChanges();

                    return true;
                }
                else
                {
                    // Không tìm thấy KhuyenMai với mã tương ứng
                    return false;
                }
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có
                return false;
            }
        }
        public bool InsertKhuyenMai(KhuyenMai km)
        {
            try
            {
                db.KhuyenMais.InsertOnSubmit(km);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteKhuyenMai(int maKM)
        {
            try
            {
                KhuyenMai km = db.KhuyenMais.FirstOrDefault(r => r.MaKM == maKM);

                if (km != null)
                {
                    db.KhuyenMais.DeleteOnSubmit(km);
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
