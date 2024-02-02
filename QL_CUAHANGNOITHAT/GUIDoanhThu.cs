using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;

using BLL;
namespace QL_CUAHANGNOITHAT
{
    public partial class GUIDoanhThu : UserControl
    {
        BLL_DoanhThu dt = new BLL_DoanhThu();
        public GUIDoanhThu()
        {
            InitializeComponent();
        }

        private void GUIDoanhThu_Load(object sender, EventArgs e)
        {
            lbIncome.Text = dt.TongThuNhap().ToString("N");
            lbSpending.Text = dt.TotalSpending().ToString("N");
            lbClient.Text = dt.CountClient().ToString();

            lbDoanhThu.Text = (dt.TongThuNhap() - dt.TotalSpending()).ToString("N") + " VND";
            dtDoanhThu.DataSource = dt.getHoaDon();
        }

        private void ExportToExcel(DataGridView dataGridView)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.Sheets[1];

            // Đặt tiêu đề và định dạng cho nó
            Excel.Range titleRange = worksheet.Range["A1", $"D1"];
            titleRange.Merge(); // Gộp ô cho tiêu đề
            titleRange.Value = "DOANH THU CỬA HÀNG";
            titleRange.Font.Bold = true;
            titleRange.Font.Size = 16;
            titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            titleRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue); // Màu nền

            // Đặt tiêu đề cột và định dạng cho nó
            for (int i = 1; i <= dataGridView.Columns.Count; i++)
            {

                worksheet.Cells[2, i] = dataGridView.Columns[i - 1].HeaderText;
                worksheet.Cells[2, i].Font.Bold = true;
                worksheet.Cells[2, i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                worksheet.Cells[2, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray); // Màu nền
            }

            // Đặt dữ liệu và định dạng cho nó
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    worksheet.Cells[i + 3, j + 1] = dataGridView.Rows[i].Cells[j].Value;
                    worksheet.Cells[i + 3, j + 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                }
            }

            // Đặt tiêu đề "Tổng tiền" và định dạng cho nó

            worksheet.Cells[dataGridView.Rows.Count + 3, 1] = "Tổng tiền";
            worksheet.Cells[dataGridView.Rows.Count + 3, 4] = lbDoanhThu.Text;
            worksheet.Cells[dataGridView.Rows.Count + 3, 4].Font.Bold = true;
            worksheet.Cells[dataGridView.Rows.Count + 3, 4].NumberFormat = "#,##0"; // Định dạng số có dấu phẩy
            worksheet.Cells[dataGridView.Rows.Count + 3, 4].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen); // Màu nền
            
            // Hiển thị Excel
            excelApp.Visible = true;
            
            // Giải phóng tài nguyên COM
            ReleaseComObjects(titleRange);
            ReleaseComObjects(worksheet);
            ReleaseComObjects(workbook);
            ReleaseComObjects(excelApp);
        }

        private void ReleaseComObjects(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            }
            catch
            {
                // Bỏ qua lỗi nếu không thể giải phóng tài nguyên COM
            }
            finally
            {
                obj = null;
            }
        }

        private void btnExport_Click(object sender, System.EventArgs e)
        {
            try
            {
                ExportToExcel(dtDoanhThu);
            }
            catch (System.Exception)
            {

                MessageBox.Show("Export thất bại");
            }
        }
    }
}
