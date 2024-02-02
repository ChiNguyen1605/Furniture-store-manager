using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    // Trong lớp DAL_SanPham
    public class DAL_SanPham
    {
        // Chuỗi kết nối đến cơ sở dữ liệu
        private string connectionString = "Data Source=MSI;Initial Catalog=DB_CuaHangNoiThat;Integrated Security=True";

        // Hàm cập nhật số lượng tồn cho sản phẩm dựa trên mã sản phẩm
        public bool UpdateSoLuongTonByMaSP(string maSP, int soLuong)
        {
            try
            {
                // Thực hiện cập nhật số lượng tồn trong cơ sở dữ liệu
                string sqlQuery = "UPDATE SanPham SET SoLuongTon = @SoLuong WHERE MaSP = @MaSP";

                // Thực hiện kết nối cơ sở dữ liệu và thực hiện truy vấn
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                        cmd.Parameters.AddWithValue("@MaSP", maSP);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Trả về true nếu cập nhật thành công
                return true;
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ (nếu có)
                // Đây có thể là nơi bạn log lỗi hoặc thực hiện các bước xử lý khác
                Console.WriteLine("Lỗi khi cập nhật số lượng tồn trong DAL_SanPham: " + ex.Message);
                return false;
            }
        }
    }

}
