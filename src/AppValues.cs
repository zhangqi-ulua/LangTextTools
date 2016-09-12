using System;
using System.Collections.Generic;
using System.Text;

public class AppValues
{
    /// <summary>
    /// 本工具支持的Excel文件的扩展名
    /// </summary>
    public const string EXCEL_FILE_EXTENSION = ".xlsx";

    /// <summary>
    /// Excel文件中存放数据的工作簿Sheet名，其余Sheet表可自定义内容，不会被本工具读取
    /// </summary>
    public const string EXCEL_DATA_SHEET_NAME = "data$";

    // 每张表格前两行分别配置语种描述和名称（行号从1开始）
    public const int EXCEL_DESC_ROW_INDEX = 1;
    public const int EXCEL_NAME_ROW_INDEX = 2;
    public const int EXCEL_DATA_START_INDEX = 3;

    /// <summary>
    /// Windows操作系统中禁止文件名中含有的字符
    /// </summary>
    public static string[] ILLEGAL_CHAR_FOR_FILENAME = new string[] { "\\", "/", ":", "*", "?", "<", ">", "!" };

    /// <summary>
    /// 提交时用于选择处理差异方式的ComboBox选项文字
    /// </summary>
    public static string[] RESOLVE_COMMIT_DIFF_WAYS = new string[] { "使用本地表", "使用SVN表" };

    /// <summary>
    /// 本工具所在目录，不能用System.Environment.CurrentDirectory因为当本工具被其他程序调用时取得的CurrentDirectory将是调用者的路径
    /// </summary>
    public static string PROGRAM_FOLDER_PATH = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

    /// <summary>
    /// 设置的注释行开头字符
    /// </summary>
    public static string CommentLineStartChar = null;

    /// <summary>
    /// 存储解析后的Excel母表信息
    /// </summary>
    public static LangExcelInfo LangExcelInfo = null;

    /// <summary>
    /// 存储解析后的旧版Excel母表信息
    /// </summary>
    public static LangExcelInfo OldLangExcelInfo = null;

    /// <summary>
    /// 选择的Excel母表路径
    /// </summary>
    public static string ExcelFullPath = null;

    /// <summary>
    /// 导出lang文件中Key与Value的分隔字符
    /// </summary>
    public static string KeyAndValueSplitChar = null;

    /// <summary>
    /// 导出lang文件的扩展名（不含点号）
    /// </summary>
    public static string LangFileExtension = null;

    /// <summary>
    /// lang文件的统一导出路径
    /// </summary>
    public static string ExportLangFileUnifiedDir = null;

    /// <summary>
    /// 当前界面中是否显示母表工具部分
    /// </summary>
    public static bool IsShowExcelFileTools = false;

    /// <summary>
    /// SVN中母表文件对应本机中的Working Copy路径
    /// </summary>
    public static string LocalExcelFilePath = null;

    /// <summary>
    /// SVN中母表文件的路径
    /// </summary>
    public static string SvnExcelFilePath = null;
}
