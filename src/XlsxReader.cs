using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

public class XlsxReader
{
    /// <summary>
    /// 通过OleDb方式将指定Excel文件的内容读取到DataSet中，但必须将Windows系统注册表中所有名为TypeGuessRows的项的值由默认值8改为0，否则会导致Excel中超过256字符的单元格中的内容无法读取完整
    /// </summary>
    public static DataSet ReadXlsxFileByOleDb(string filePath, out string errorString)
    {
        OleDbConnection conn = null;
        OleDbDataAdapter da = null;
        DataSet ds = null;

        try
        {
            // 初始化连接并打开
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1\"";

            conn = new OleDbConnection(connectionString);
            conn.Open();

            // 获取数据源的表定义元数据                       
            System.Data.DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            // 必须存在数据表
            bool isFoundDateSheet = false;

            for (int i = 0; i < dtSheet.Rows.Count; ++i)
            {
                string sheetName = dtSheet.Rows[i]["TABLE_NAME"].ToString();

                if (sheetName == AppValues.EXCEL_DATA_SHEET_NAME)
                    isFoundDateSheet = true;
            }
            if (!isFoundDateSheet)
            {
                errorString = string.Format("错误：{0}中不含有Sheet名为{1}的数据表", filePath, AppValues.EXCEL_DATA_SHEET_NAME.Replace("$", ""));
                return null;
            }

            // 初始化适配器
            da = new OleDbDataAdapter();
            da.SelectCommand = new OleDbCommand(String.Format("Select * FROM [{0}]", AppValues.EXCEL_DATA_SHEET_NAME), conn);

            ds = new DataSet();
            da.Fill(ds, AppValues.EXCEL_DATA_SHEET_NAME);
        }
        catch
        {
            errorString = "错误：连接Excel失败，你可能尚未安装Office数据连接组件: http://www.microsoft.com/en-US/download/details.aspx?id=23734 \n";
            return null;
        }
        finally
        {
            // 关闭连接
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                // 由于C#运行机制，即便因为表格中没有Sheet名为data的工作簿而return null，也会继续执行finally，而此时da为空，故需要进行判断处理
                if (da != null)
                    da.Dispose();
                conn.Dispose();
            }
        }

        errorString = null;
        return ds;
    }

    /// <summary>
    /// 通过Microsoft.Office.Interop类库方式将指定Excel文件的内容读取到DataSet中，读取数据量较大的Excel文件效率极低，几乎无法使用
    /// </summary>
    public static DataSet ReadXlsxFileByOfficeInterop(string filePath, out string errorString)
    {
        Excel.Application application = new Excel.Application();
        // 不显示Excel窗口
        application.Visible = false;
        // 不显示警告对话框
        application.DisplayAlerts = false;
        // 禁止屏幕刷新
        application.ScreenUpdating = false;
        // 编辑非空单元格时不进行警告提示
        application.AlertBeforeOverwriting = false;
        // 打开Excel工作簿
        Excel.Workbook workbook = application.Workbooks.Open(filePath);
        // 找到名为data的Sheet表
        Excel.Worksheet worksheet = null;
        int sheetCount = workbook.Sheets.Count;
        string DATA_SHEET_NAME = AppValues.EXCEL_DATA_SHEET_NAME.Replace("$", "");
        for (int i = 1; i <= sheetCount; ++i)
        {
            Excel.Worksheet sheet = workbook.Sheets[i] as Excel.Worksheet;
            if (sheet.Name.Equals(DATA_SHEET_NAME))
            {
                worksheet = sheet;
                break;
            }
        }
        if (worksheet == null)
        {
            errorString = string.Format("错误：{0}中不含有Sheet名为{1}的数据表", filePath, AppValues.EXCEL_DATA_SHEET_NAME.Replace("$", ""));
            return null;
        }

        DataSet ds = new DataSet();
        System.Data.DataTable dt = ds.Tables.Add();
        // 经测试发现，如果Excel表中左上角部分存在大片空单元格，比如E5单元格是表中最左上角有内容的单元格，就会导致通过Microsoft.Office.Interop类库获取已被使用的行列数错误，故这里为了正确读取表格，如果表格首个单元格内容为空，故意填上数据后再获取已使用的行列数
        string firstCellContent = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, 1]).Text.ToString();
        if (string.IsNullOrEmpty(firstCellContent))
            worksheet.Cells[1, 1] = "fill";

        int rowCount = worksheet.UsedRange.Cells.Rows.Count;
        int columnCount = worksheet.UsedRange.Cells.Columns.Count;

        if (string.IsNullOrEmpty(firstCellContent))
            worksheet.Cells[1, 1] = "";

        // 生成列头信息
        for (int columnIndex = 1; columnIndex <= columnCount; ++columnIndex)
            dt.Columns.Add(new DataColumn());

        // 逐行读取数据
        for (int rowIndex = 1; rowIndex <= rowCount; ++rowIndex)
        {
            DataRow dr = dt.NewRow();
            for (int columnIndex = 1; columnIndex <= columnCount; ++columnIndex)
                dr[columnIndex - 1] = ((Range)worksheet.Cells[rowIndex, columnIndex]).Value2;

            dt.Rows.Add(dr);
        }

        workbook.Close(false);
        application.Workbooks.Close();
        application.Quit();
        Utils.KillExcelProcess(application);

        errorString = null;
        return ds;
    }
}
