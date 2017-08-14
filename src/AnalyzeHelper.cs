using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

/// <summary>
/// 该类用于解析国际化Excel母表文件中所有语种信息
/// </summary>
public class AnalyzeHelper
{
    public static LangExcelInfo AnalyzeLangExcelFile(string filePath, string commentLineStartChar, out string errorString)
    {
        LangExcelInfo langExcelInfo = new LangExcelInfo();

        DataSet dataSet = XlsxReader.ReadXlsxFileByOleDb(filePath, out errorString);
        if (errorString != null)
            return null;

        DataTable dataTable = dataSet.Tables[0];
        // 依次记录各语种的信息
        List<LanguageInfo> languageInfoList = new List<LanguageInfo>();
        // 依次记录各语种的名称，不允许同名语种
        List<string> languageNames = new List<string>();

        int rowCount = dataTable.Rows.Count;
        int columnCount = dataTable.Columns.Count;
        if (rowCount < 2)
        {
            errorString = "Excel表格格式非法，必须在前两行声明语种描述和名称";
            return null;
        }
        if (columnCount < 3)
        {
            errorString = "Excel表格格式非法，列自左向右应分别声明Key、主语言和至少一种外语";
            return null;
        }

        // 从表格第2列开始，查找各语言所在列
        for (int i = 1; i < columnCount; ++i)
        {
            string languageName = dataTable.Rows[AppValues.EXCEL_NAME_ROW_INDEX - 1][i].ToString().Trim();
            if (!string.IsNullOrEmpty(languageName))
            {
                // 检查不同语种不允许名称相同
                if (languageNames.Contains(languageName))
                {
                    errorString = string.Format("表格第{0}行的语种名称定义中出现了名称均为\"{1}\"的两列，请修正后重试", AppValues.EXCEL_NAME_ROW_INDEX, languageName);
                    return null;
                }
                else
                {
                    languageNames.Add(languageName);

                    LanguageInfo languageInfo = new LanguageInfo();
                    languageInfo.Desc = dataTable.Rows[AppValues.EXCEL_DESC_ROW_INDEX - 1][i].ToString().Trim().Replace(System.Environment.NewLine, " ").Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ');
                    languageInfo.Name = languageName;
                    languageInfo.ColumnIndex = i + 1;

                    languageInfoList.Add(languageInfo);
                }
            }
        }
        if (languageInfoList.Count < 2)
        {
            errorString = "Excel母表格式非法，列自左向右应分别声明Key、主语言和至少一种外语";
            return null;
        }

        // 记录所有的Key值，当Key为空或者以指定字符开头时认为是无效Key，存储为null
        List<string> keys = new List<string>();
        // 记录已读取的Key列值，不允许出现非空的重复Key（key：key值，value：从1开始计的行号）
        Dictionary<string, int> keyDict = new Dictionary<string, int>();
        // 记录重复Key所在行的信息（key：重复的Key名，value：此Key所在行列表，行号从1开始计）
        Dictionary<string, List<int>> repeatedKeyInfo = new Dictionary<string, List<int>>();
        // 记录主语言未进行翻译的行索引（从0开始计）
        List<int> notTranslatedRowIndex = new List<int>();

        // 逐行读取表格并生成所有语种的数据
        for (int dataIndex = AppValues.EXCEL_DATA_START_INDEX - 1; dataIndex < rowCount; ++dataIndex)
        {
            string keyString = dataTable.Rows[dataIndex][0].ToString().Trim();
            if (string.IsNullOrEmpty(keyString) || (commentLineStartChar != null && keyString.StartsWith(commentLineStartChar)))
            {
                keys.Add(null);
                for (int i = 0; i < languageInfoList.Count; ++i)
                {
                    LanguageInfo languageInfo = languageInfoList[i];
                    languageInfo.Data.Add(null);
                }
            }
            else
            {
                for (int i = 0; i < languageInfoList.Count; ++i)
                {
                    LanguageInfo languageInfo = languageInfoList[i];
                    string inputData = dataTable.Rows[dataIndex][languageInfo.ColumnIndex - 1].ToString();
                    languageInfo.Data.Add(inputData);
                }

                // 如果Key列值出现重复错误进行记录
                if (keyDict.ContainsKey(keyString))
                {
                    if (repeatedKeyInfo.ContainsKey(keyString))
                    {
                        List<int> repeatedKeyLineNums = repeatedKeyInfo[keyString];
                        repeatedKeyLineNums.Add(dataIndex + 1);
                    }
                    else
                    {
                        List<int> repeatedKeyLineNums = new List<int>();
                        repeatedKeyLineNums.Add(keyDict[keyString]);
                        repeatedKeyLineNums.Add(dataIndex + 1);
                        repeatedKeyInfo.Add(keyString, repeatedKeyLineNums);
                    }
                }
                else
                {
                    keys.Add(keyString);
                    keyDict.Add(keyString, dataIndex + 1);
                    langExcelInfo.KeyToDataIndex.Add(keyString, dataIndex - AppValues.EXCEL_NAME_ROW_INDEX);

                    // 检查主语言必须含有翻译
                    LanguageInfo defaultLanguageInfo = languageInfoList[0];
                    if (string.IsNullOrEmpty(defaultLanguageInfo.Data[dataIndex - AppValues.EXCEL_NAME_ROW_INDEX].ToString()))
                        notTranslatedRowIndex.Add(dataIndex);
                }
            }
        }
        StringBuilder errorStringBuilder = new StringBuilder();
        if (repeatedKeyInfo.Count > 0)
        {
            errorStringBuilder.AppendLine("以下行Key重复：");
            foreach (var item in repeatedKeyInfo)
            {
                string key = item.Key;
                List<int> repeatedKeyLineNums = item.Value;
                errorStringBuilder.AppendFormat("名为\"{0}\"的Key在以下行中重复出现：{1}", key, Utils.CombineString<int>(repeatedKeyLineNums, ",")).AppendLine();
            }
        }
        if (notTranslatedRowIndex.Count > 0)
        {
            errorStringBuilder.AppendLine("以下行中主语言未填写对应文字：");
            for (int i = 0; i < notTranslatedRowIndex.Count; ++i)
            {
                int rowIndex = notTranslatedRowIndex[i];
                string key = keys[rowIndex - AppValues.EXCEL_NAME_ROW_INDEX];
                errorStringBuilder.AppendFormat("第{0}行（Key名为\"{1}\"）", rowIndex + 1, key).AppendLine();
            }
        }

        errorString = errorStringBuilder.ToString();
        if (string.IsNullOrEmpty(errorString))
        {
            langExcelInfo.Keys = keys;
            langExcelInfo.DefaultLanguageInfo = languageInfoList[0];
            for (int i = 1; i < languageInfoList.Count; ++i)
            {
                LanguageInfo languageInfo = languageInfoList[i];
                langExcelInfo.OtherLanguageInfo.Add(languageInfo.Name, languageInfo);
            }

            errorString = null;
            return langExcelInfo;
        }
        else
            return null;
    }
}

// 一个语种的信息
public class LanguageInfo
{
    // 语种描述
    public string Desc { get; set; }
    // 语种名称（各语种不允许重复）
    public string Name { get; set; }
    // 所在Excel表的列号（从1开始编号）
    public int ColumnIndex { get; set; }
    // 该语种对应每个Key的译文，注意若某行Key无效对应各个语种的译文存储为null
    public List<string> Data { get; set; }

    public LanguageInfo()
    {
        Data = new List<string>();
    }
}

// 国际化Excel母表的信息
public class LangExcelInfo
{
    // 所有的Key名，注意若某行Key为空或者是以指定字符开头的注释行，此行Key值存储为null
    public List<string> Keys { get; set; }
    // key：Key名，value：对应在data列表中的索引
    public Dictionary<string, int> KeyToDataIndex { get; set; }
    // 主语言（靠近Key值列，最先声明的语种）信息
    public LanguageInfo DefaultLanguageInfo { get; set; }
    // 所有非主语言信息（key：语种名称，value：对应的语种信息）
    public Dictionary<string, LanguageInfo> OtherLanguageInfo { get; set; }

    public LangExcelInfo()
    {
        Keys = new List<string>();
        OtherLanguageInfo = new Dictionary<string, LanguageInfo>();
        KeyToDataIndex = new Dictionary<string, int>();
    }

    public LanguageInfo GetLanguageInfoByLanguageName(string languageName)
    {
        if (DefaultLanguageInfo.Name.Equals(languageName))
            return DefaultLanguageInfo;
        else if (OtherLanguageInfo.ContainsKey(languageName))
            return OtherLanguageInfo[languageName];
        else
            return null;
    }

    public List<LanguageInfo> GetAllLanguageInfoList()
    {
        List<LanguageInfo> languageInfoList = new List<LanguageInfo>();
        languageInfoList.Add(DefaultLanguageInfo);
        foreach (LanguageInfo info in OtherLanguageInfo.Values)
            languageInfoList.Add(info);

        return languageInfoList;
    }
}
