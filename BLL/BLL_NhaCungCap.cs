using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_NhaCungCap
    {
        DB_CuaHangNoiThatDataContext db = new DB_CuaHangNoiThatDataContext();

        public List<NhaCungCap> SearchNhaCungCap(int maNCC, string tenNCC)
        {
            try
            {
                // Tìm kiếm nhà cung cấp theo mã và tên
                var result = from ncc in db.NhaCungCaps
                             where ncc.MaNCC == maNCC || ncc.TenNCC.Contains(tenNCC)
                             select ncc;

                // Trả về danh sách nhà cung cấp tìm được
                return result.ToList();
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có
                return null;
            }
        }
        public object GetNhaCungCap(int v)
        {
            return db.NhaCungCaps.Select(r => r);
        }
        public bool insertNhaCungCap(NhaCungCap ncc)
        {
            try
            {
                db.NhaCungCaps.InsertOnSubmit(ncc);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool deleteNhaCungCap(int maNCC)
        {
            try
            {
                // Tìm nhà cung cấp cần xóa theo mã
                NhaCungCap ncc = db.NhaCungCaps.FirstOrDefault(r => r.MaNCC == maNCC);

                // Kiểm tra xem nhà cung cấp có tồn tại không
                if (ncc != null)
                {
                    db.NhaCungCaps.DeleteOnSubmit(ncc);
                    db.SubmitChanges();
                    return true;
                }
                else
                {
                    return false; // Trả về false nếu nhà cung cấp không tồn tại
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateNhaCungCap(int maNCC, string tenNCC, string diaChi, string dienThoai)
        {
            try
            {
                // Kiểm tra xem có nhà cung cấp nào có mã là maNCC không
                NhaCungCap ncc = db.NhaCungCaps.FirstOrDefault(n => n.MaNCC == maNCC);

                if (ncc != null)
                {
                    // Cập nhật thông tin nhà cung cấp
                    ncc.TenNCC = tenNCC;
                    ncc.DiaChi = diaChi;
                    ncc.DienThoai = dienThoai;

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    db.SubmitChanges();

                    return true; // Trả về true nếu cập nhật thành công
                }
                else
                {
                    // Trả về false nếu không tìm thấy nhà cung cấp
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                // (Bạn có thể thay thế bằng cách log lỗi hoặc xử lý ngoại lệ theo cách khác)
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
