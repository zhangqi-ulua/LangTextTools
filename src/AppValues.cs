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
    public static string[] RESOLVE_COMMIT_DIFF_OPTINOS = new string[] { "使用本地表", "使用SVN表" };

    /// <summary>
    /// 提交时是否已经导出提交主语言对应lang文件的ComboBox选项文字
    /// </summary>
    public static string[] COMMIT_LANG_FILE_OPTINOS = new string[] { "不导出", "仅导出不提交", "导出并提交" };

    /// <summary>
    /// 本工具所在目录，不能用System.Environment.CurrentDirectory因为当本工具被其他程序调用时取得的CurrentDirectory将是调用者的路径
    /// </summary>
    public static string PROGRAM_FOLDER_PATH = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

    /// <summary>
    /// 与本工具同目录下的配置文件的文件名
    /// </summary>
    public const string CONFIG_FILE_NAME = "config.txt";

    // 以下为所有配置项的键名
    public const string CONFIG_KEY_EXCEL_PATH = "ExcelPath";
    public const string CONFIG_KEY_COMMENT_LINE_START_CHAR = "CommentLineStartChar";
    public const string CONFIG_KEY_KEY_VALUE_SPLIT_CHAR = "KeyValueSplitChar";
    public const string CONFIG_KEY_LANG_FILE_EXTENSION = "LangFileExtension";
    public const string CONFIG_KEY_IS_EXPORT_UNIFIED_DIR = "IsExportUnifiedDir";
    public const string CONFIG_KEY_EXPORT_UNIFIED_DIR = "ExportUnifiedDir";
    public const string CONFIG_KEY_EXPORT_LANG_FILE_START_STRING = "ExportLangFile_";
    public const string CONFIG_KEY_OLD_EXCEL_PATH = "OldExcelPath";
    public const string CONFIG_KEY_EXPORT_NEED_TRANSLATE_EXCEL_FILE = "ExportNeedTranslateExcelFile";
    public const string CONFIG_KEY_COLOR_FOR_ADD = "ColorForAdd";
    public const string CONFIG_KEY_COLOR_FOR_CHANGE = "ColorForChange";
    public const string CONFIG_KEY_FILL_NULL_CELL_TEXT = "FillNullCellText";
    public const string CONFIG_KEY_COMPARED_EXCEL_PATH = "ComparedExcelPath";
    public const string CONFIG_KEY_TRANSLATED_EXCEL_PATH = "TranslatedExcelPath";
    public const string CONFIG_KEY_MERGED_EXCEL_PATH = "MergedExcelPath";
    public const string CONFIG_KEY_LOCAL_EXCEL_FILE_PATH = "LocalExcelFilePath";
    public const string CONFIG_KEY_COMMIT_LOG_MESSAGE = "CommitLogMessage";
    public const string CONFIG_KEY_COMMIT_LANG_FILE_PATH = "CommitLangFilePath";

    /// <summary>
    /// 存储用户在配置文件中声明的配置
    /// </summary>
    public static Dictionary<string, string> Config = new Dictionary<string, string>();

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

    /// <summary>
    /// 左半功能区选择的Excel母表的MD5
    /// </summary>
    public static string ExcelMD5 = null;

    /// <summary>
    /// 右半功能区选择的本地表的MD5
    /// </summary>
    public static string LocalExcelMD5 = null;
}
