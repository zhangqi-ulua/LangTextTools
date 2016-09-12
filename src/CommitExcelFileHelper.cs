using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

/// <summary>
/// 该类用于将本地表提交SVN时进行对比、选择等
/// </summary>
public class CommitExcelFileHelper
{
    public static CommitExcelInfo AnalyzeCommitExcelFile(string filePath, string commentLineStartChar, long revision, out string errorString)
    {
        CommitExcelInfo commitExcelInfo = new CommitExcelInfo();
        commitExcelInfo.Revision = revision;

        DataSet dataSet = XlsxReader.ReadXlsxFileByOleDb(filePath, out errorString);
        if (errorString != null)
            return null;

        System.Data.DataTable dataTable = dataSet.Tables[0];

        int rowCount = dataTable.Rows.Count;
        int columnCount = dataTable.Columns.Count;
        if (rowCount < 2)
        {
            errorString = "Excel表格格式非法，必须在前两行声明语种描述和名称";
            return null;
        }
        if (columnCount < 2)
        {
            errorString = "Excel表格格式非法，列自左向右应分别声明Key、主语言";
            return null;
        }

        // 从表格第2列开始寻找主语言列
        bool hasDefaultLanguage = false;
        // 记录已读取的Key列值，不允许出现非空的重复Key（key：key值，value：从1开始计的行号）
        Dictionary<string, int> keyDict = new Dictionary<string, int>();
        // 记录重复Key所在行的信息（key：重复的Key名，value：此Key所在行列表，行号从1开始计）
        Dictionary<string, List<int>> repeatedKeyInfo = new Dictionary<string, List<int>>();
        // 记录主语言未进行翻译的行索引（从0开始计）
        List<int> notTranslatedRowIndex = new List<int>();

        for (int i = 1; i < columnCount; ++i)
        {
            string languageName = dataTable.Rows[AppValues.EXCEL_NAME_ROW_INDEX - 1][i].ToString().Trim();
            if (!string.IsNullOrEmpty(languageName))
            {
                // 解析Key列和主语言列
                for (int dataIndex = AppValues.EXCEL_DATA_START_INDEX - 1; dataIndex < rowCount; ++dataIndex)
                {
                    string keyString = dataTable.Rows[dataIndex][0].ToString().Trim();
                    // 空行或注释行
                    if (string.IsNullOrEmpty(keyString) || (commentLineStartChar != null && keyString.StartsWith(commentLineStartChar)))
                    {
                        commitExcelInfo.Keys.Add(null);
                        commitExcelInfo.Data.Add(null);
                    }
                    else
                    {
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
                            commitExcelInfo.Keys.Add(keyString);
                            keyDict.Add(keyString, dataIndex + 1);
                            commitExcelInfo.KeyToDataIndex.Add(keyString, dataIndex - AppValues.EXCEL_NAME_ROW_INDEX);

                            // 检查主语言必须含有翻译
                            string translatedString = dataTable.Rows[dataIndex][i].ToString();
                            if (string.IsNullOrEmpty(translatedString))
                            {
                                notTranslatedRowIndex.Add(dataIndex);
                                commitExcelInfo.Data.Add(null);
                            }
                            else
                                commitExcelInfo.Data.Add(translatedString);
                        }
                    }
                }

                commitExcelInfo.DataColumnIndex = i + 1;
                hasDefaultLanguage = true;
                break;
            }
        }
        if (hasDefaultLanguage == false)
        {
            errorString = "Excel表格格式非法，未找到主语言列";
            return null;
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
                string key = commitExcelInfo.Keys[rowIndex - AppValues.EXCEL_DATA_START_INDEX - 1];
                errorStringBuilder.AppendFormat("第{0}行（Key名为\"{1}\"）", rowIndex + 1, key).AppendLine();
            }
        }

        errorString = errorStringBuilder.ToString();
        if (string.IsNullOrEmpty(errorString))
        {
            errorString = null;
            return commitExcelInfo;
        }
        else
            return null;
    }

    /// <summary>
    /// 对比即将提交的本地母表与SVN中母表返回对比结果
    /// </summary>
    public static CommitCompareResult CompareCommitExcelFile(CommitExcelInfo localFile, CommitExcelInfo svnFile)
    {
        CommitCompareResult compareResult = new CommitCompareResult();
        compareResult.LocalFileRevision = localFile.Revision;
        compareResult.SvnFileRevision = svnFile.Revision;

        // 遍历本地表的Key找到本地表独有的Key以及与SVN主语言译文不同的Key信息
        int localFileKeyCount = localFile.Keys.Count;
        for (int i = 0; i < localFileKeyCount; ++i)
        {
            string localFileKey = localFile.Keys[i];
            if (localFileKey == null)
                continue;

            if (svnFile.KeyToDataIndex.ContainsKey(localFileKey))
            {
                // 比较主语言译文是否相同
                string localFileData = localFile.GetDataByKey(localFileKey);
                string svnFileData = svnFile.GetDataByKey(localFileKey);
                if (!localFileData.Equals(svnFileData))
                {
                    CommitDifferentDefaultLanguageInfo oneDiffInfo = new CommitDifferentDefaultLanguageInfo();
                    oneDiffInfo.Key = localFileKey;
                    oneDiffInfo.LocalFileDefaultLanguageValue = localFileData;
                    oneDiffInfo.SvnFileDefaultLanguageValue = svnFileData;
                    oneDiffInfo.LocalFileLineNum = localFile.KeyToDataIndex[localFileKey] + AppValues.EXCEL_DATA_START_INDEX;
                    oneDiffInfo.SvnFileLineNum = svnFile.KeyToDataIndex[localFileKey] + AppValues.EXCEL_DATA_START_INDEX;

                    compareResult.DiffInfo.Add(oneDiffInfo);
                }
            }
            else
            {
                // 本地母表有但SVN中无的Key
                CommitDifferentKeyInfo oneLocalAddKeyInfo = new CommitDifferentKeyInfo();
                oneLocalAddKeyInfo.Key = localFileKey;
                oneLocalAddKeyInfo.ExcelLineNum = localFile.KeyToDataIndex[localFileKey] + AppValues.EXCEL_DATA_START_INDEX;
                oneLocalAddKeyInfo.DefaultLanguageValue = localFile.GetDataByKey(localFileKey);

                compareResult.LocalAddKeyInfo.Add(oneLocalAddKeyInfo);
            }
        }
        // 遍历SVN中母表的Key找到SVN中母表独有的Key
        int svnFileKeyCount = svnFile.Keys.Count;
        for (int i = 0; i < svnFileKeyCount; ++i)
        {
            string svnFileKey = svnFile.Keys[i];
            if (svnFileKey == null)
                continue;

            if (!localFile.KeyToDataIndex.ContainsKey(svnFileKey))
            {
                CommitDifferentKeyInfo oneSvnAddKeyInfo = new CommitDifferentKeyInfo();
                oneSvnAddKeyInfo.Key = svnFileKey;
                oneSvnAddKeyInfo.ExcelLineNum = svnFile.KeyToDataIndex[svnFileKey] + AppValues.EXCEL_DATA_START_INDEX;
                oneSvnAddKeyInfo.DefaultLanguageValue = svnFile.GetDataByKey(svnFileKey);

                compareResult.SvnAddKeyInfo.Add(oneSvnAddKeyInfo);
            }
        }

        return compareResult;
    }

    /// <summary>
    /// 根据用户选择的差异处理结果，以SVN最新版本文件作为副本，与本地表内容进行合并生成新的Excel母表文件，返回生成文件的路径
    /// </summary>
    public static string GenerateCommitExcelFile(CommitCompareResult compareResult, CommitExcelInfo localExcelInfo, CommitExcelInfo svnExcelInfo, out string errorString)
    {
        // 下载对应版本的SVN表，以此作为副本进行修改合并
        string svnCopySavePath = Utils.CombinePath(AppValues.PROGRAM_FOLDER_PATH, string.Format("合并后用于提交的SVN母表副本 {0:yyyy年MM月dd日 HH时mm分ss秒} 对应SVN版本号{1}.xlsx", DateTime.Now, compareResult.SvnFileRevision));
        Exception exportException;
        bool result = OperateSvnHelper.ExportSvnFileToLocal(AppValues.SvnExcelFilePath, svnCopySavePath, compareResult.SvnFileRevision, out exportException);
        if (exportException != null)
        {
            errorString = string.Format("下载SVN中最新版本号（{0}）的母表文件存到本地失败，错误原因为：{1}", compareResult.SvnFileRevision, exportException.Message);
            return null;
        }
        else if (result == false)
        {
            errorString = string.Format("下载SVN中最新版本号（{0}）的母表文件存到本地失败", compareResult.SvnFileRevision);
            return null;
        }

        // 打开并编辑这个Excel文件，写入本地表中要合并进去的变动
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
        Excel.Workbook workbook = application.Workbooks.Open(svnCopySavePath);
        // 找到名为data的Sheet表
        Excel.Worksheet dataWorksheet = null;
        int sheetCount = workbook.Sheets.Count;
        string DATA_SHEET_NAME = AppValues.EXCEL_DATA_SHEET_NAME.Replace("$", "");
        for (int i = 1; i <= sheetCount; ++i)
        {
            Excel.Worksheet sheet = workbook.Sheets[i] as Excel.Worksheet;
            if (sheet.Name.Equals(DATA_SHEET_NAME))
            {
                dataWorksheet = sheet;
                break;
            }
        }
        if (dataWorksheet == null)
        {
            errorString = string.Format("新版母表（{0}）找不到Sheet名为{1}的数据表，请勿在使用本工具过程中对母表文件进行操作，导出操作被迫中止", AppValues.ExcelFullPath, DATA_SHEET_NAME);
            return null;
        }

        // 将要提交的变动与该最新SVN表进行合并
        // 最新SVN表中主语言列的列号
        int defaultLanguageColumnNumInSvnFile = svnExcelInfo.DataColumnIndex;

        // 先合并主语言译文不同的差异项，只需将该表中Key对应的主语言译文替换为本地表中的内容即可
        int diffInfoCount = compareResult.DiffInfo.Count;
        for (int i = 0; i < diffInfoCount; ++i)
        {
            CommitDifferentDefaultLanguageInfo oneDiffInfo = compareResult.DiffInfo[i];
            if (oneDiffInfo.ResolveConflictWay == ResolveConflictWays.UseLocal)
            {
                string key = oneDiffInfo.Key;
                string replaceValue = oneDiffInfo.LocalFileDefaultLanguageValue;
                int dataIndexInSvnFile = svnExcelInfo.KeyToDataIndex[key];
                dataWorksheet.Cells[dataIndexInSvnFile + AppValues.EXCEL_DATA_START_INDEX, defaultLanguageColumnNumInSvnFile] = replaceValue;
            }
        }

        // 克隆一份SVN表中的Key信息供之后进行修改而不影响到引用的svnExcelInfo中的Key列表
        List<string> cloneSvnKeys = new List<string>(svnExcelInfo.Keys);
        // 再从该最新SVN表中删除用户选定的Key行
        // 先在最新SVN表中找到要删除Key所在的行号，但要注意因为是逐条删除，删除上一条之后下一条所在行号减一，下下条减二，以此类推
        List<int> deleteLineNumList = new List<int>();
        int svnAddKeyCount = compareResult.SvnAddKeyInfo.Count;
        // 记录要删除的个数
        int tempDeleteCount = 0;
        for (int i = 0; i < svnAddKeyCount; ++i)
        {
            CommitDifferentKeyInfo oneSvnAddKeyInfo = compareResult.SvnAddKeyInfo[i];
            if (oneSvnAddKeyInfo.ResolveConflictWay == ResolveConflictWays.UseLocal)
            {
                deleteLineNumList.Add(oneSvnAddKeyInfo.ExcelLineNum - tempDeleteCount);
                ++tempDeleteCount;
            }
        }
        int deleteLineNumCount = deleteLineNumList.Count;
        for (int i = 0; i < deleteLineNumCount; ++i)
        {
            int lineNum = deleteLineNumList[i];
            dataWorksheet.get_Range(string.Concat("A", lineNum)).EntireRow.Delete();
            cloneSvnKeys.RemoveAt(lineNum - AppValues.EXCEL_DATA_START_INDEX);
        }

        // 最后向该最新SVN表中添加用户指定新增的Key行
        // 新增Key行插入到最新SVN表中的位置按此方式确定：本地表中新增Key（比如叫key1）在哪一个Key（比如叫key2）之后（要求Key2也必须也存在于最新SVN表中），就在最新SVN表key2的下一行插入key1。如果本地表key1为首行或者以上的行没有任何key也存在于最新SVN表中，则将key1直接置于最新SVN表首行
        int localAddKeyCount = compareResult.LocalAddKeyInfo.Count;
        for (int i = 0; i < localAddKeyCount; ++i)
        {
            CommitDifferentKeyInfo oneLocalAddKeyInfo = compareResult.LocalAddKeyInfo[i];
            if (oneLocalAddKeyInfo.ResolveConflictWay == ResolveConflictWays.UseLocal)
            {
                string addKey = oneLocalAddKeyInfo.Key;
                // 找到该Key在本地表中的行号，然后向上寻找首个在最新SVN表也存在的Key，将Key行插入到最新SVN表中那个Key之后的新建行中
                int keyDataIndexInLocalFile = localExcelInfo.KeyToDataIndex[addKey];
                // 上一个SVN表存在的key名以及在SVN表中数据索引位置
                string lastKey = null;
                int lastKeyDataIndex = -1;
                for (int j = keyDataIndexInLocalFile - 1; j > -1; --j)
                {
                    if (lastKey != null)
                        break;

                    string tempLastKey = localExcelInfo.Keys[j];
                    if (tempLastKey != null)
                    {
                        int svnKeyCount = cloneSvnKeys.Count;
                        for (int temp = 0; temp < svnKeyCount; ++temp)
                        {
                            if (tempLastKey.Equals(cloneSvnKeys[temp]))
                            {
                                lastKey = tempLastKey;
                                lastKeyDataIndex = temp;
                                break;
                            }
                        }
                    }
                }
                // 未找到新增Key行之前的Key（该Key必须也存在于最新SVN表中），将此新增Key行直接置于最新SVN表首行
                if (lastKey == null)
                {
                    // 在首行数据之前再插入一个新行，注意Insert方法会在指定行的上面插入新行
                    dataWorksheet.get_Range(string.Concat("A", AppValues.EXCEL_DATA_START_INDEX)).EntireRow.Insert();
                    // 填写Key名以及主语言译文
                    dataWorksheet.Cells[AppValues.EXCEL_DATA_START_INDEX, 1] = addKey;
                    dataWorksheet.Cells[AppValues.EXCEL_DATA_START_INDEX, defaultLanguageColumnNumInSvnFile] = oneLocalAddKeyInfo.DefaultLanguageValue;

                    cloneSvnKeys.Insert(0, addKey);
                }
                else
                {
                    int lineNumInSvnFile = lastKeyDataIndex + AppValues.EXCEL_DATA_START_INDEX;
                    dataWorksheet.get_Range(string.Concat("A", lineNumInSvnFile + 1)).EntireRow.Insert();

                    dataWorksheet.Cells[lineNumInSvnFile + 1, 1] = addKey;
                    dataWorksheet.Cells[lineNumInSvnFile + 1, defaultLanguageColumnNumInSvnFile] = oneLocalAddKeyInfo.DefaultLanguageValue;

                    cloneSvnKeys.Insert(lastKeyDataIndex + 1, addKey);
                }
            }
        }

        // 保存Excel
        dataWorksheet.SaveAs(svnCopySavePath);
        workbook.SaveAs(svnCopySavePath);
        // 关闭Excel
        workbook.Close(false);
        application.Workbooks.Close();
        application.Quit();
        Utils.KillExcelProcess(application);

        errorString = null;
        return svnCopySavePath;
    }
}

/// <summary>
/// 提交SVN时解析的本地母表或SVN中母表的信息
/// </summary>
public class CommitExcelInfo
{
    // 所有的Key名，注意若某行Key为空或者是以指定字符开头的注释行，此行Key值存储为null
    public List<string> Keys { get; set; }
    // key：Key名，value：对应在data列表中的索引
    public Dictionary<string, int> KeyToDataIndex { get; set; }
    // 该语种对应每个Key的译文，注意若某行Key无效对应各个语种的译文存储为null
    public List<string> Data { get; set; }
    // 对应SVN中的版本号
    public long Revision { get; set; }
    // 主语言列的列号（从1开始编号）
    public int DataColumnIndex { get; set; }

    public CommitExcelInfo()
    {
        Keys = new List<string>();
        KeyToDataIndex = new Dictionary<string, int>();
        Data = new List<string>();
    }

    public string GetDataByKey(string key)
    {
        if (!string.IsNullOrEmpty(key) && KeyToDataIndex.ContainsKey(key))
        {
            int dataIndex = KeyToDataIndex[key];
            return Data[dataIndex];
        }

        return null;
    }
}

/// <summary>
/// 用于记录一次本地表与SVN表对比的差异结果
/// </summary>
public class CommitCompareResult
{
    // 本地表版本号
    public long LocalFileRevision { get; set; }
    // SVN表版本号
    public long SvnFileRevision { get; set; }
    // 本地母表与SVN中Key相同但主语言翻译不同
    public List<CommitDifferentDefaultLanguageInfo> DiffInfo { get; set; }
    // 本地母表有但SVN母表没有的Key
    public List<CommitDifferentKeyInfo> LocalAddKeyInfo { get; set; }
    // SVN母表有但本地母表没有的Key
    public List<CommitDifferentKeyInfo> SvnAddKeyInfo { get; set; }

    public CommitCompareResult()
    {
        DiffInfo = new List<CommitDifferentDefaultLanguageInfo>();
        LocalAddKeyInfo = new List<CommitDifferentKeyInfo>();
        SvnAddKeyInfo = new List<CommitDifferentKeyInfo>();
    }

    public bool IsHasDiff()
    {
        return DiffInfo.Count > 0 || LocalAddKeyInfo.Count > 0 || SvnAddKeyInfo.Count > 0;
    }
}

/// <summary>
/// 用于记录一条即将提交到SVN时发现的本地母表与SVN中Key相同但主语言翻译不同信息
/// </summary>
public class CommitDifferentDefaultLanguageInfo
{
    // 本地母表中的行号
    public int LocalFileLineNum;
    // SVN母表中的行号
    public int SvnFileLineNum;
    // Key名
    public string Key;
    // 本地母表中的主语言译文
    public string LocalFileDefaultLanguageValue;
    // SVN母表中的主语言译文
    public string SvnFileDefaultLanguageValue;

    // 处理差异的方式
    public ResolveConflictWays ResolveConflictWay;
}

/// <summary>
/// 用于记录一条即将提交到SVN时发现的本地母表与SVN中存在的独有Key信息（既可用于记录本地母表有但SVN母表没有的Key信息也可记录SVN中有但本地无）
/// </summary>
public class CommitDifferentKeyInfo
{
    // 行号
    public int ExcelLineNum;
    // Key名
    public string Key;
    // 主语言译文
    public string DefaultLanguageValue;

    // 处理差异的方式
    public ResolveConflictWays ResolveConflictWay;
}

/// <summary>
/// 处理一条本地母表与SVN中母表差异的方式
/// </summary>
public enum ResolveConflictWays
{
    NotChoose,
    UseLocal,
    UseSvn,
}
