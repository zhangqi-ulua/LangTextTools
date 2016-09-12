using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Drawing;

/// <summary>
/// 该类用于导出经对比、合并等操作后的Excel文件及报告文件
/// </summary>
public class ExportExcelFileHelper
{
    /// <summary>
    /// 对比新旧两张Excel母表，返回旧表中含有但新表中已删除的Key所在旧表中的数据索引、新表中新增Key所在新表中的数据索引、新表中的主语言翻译相对旧表变动的Key所在新表中的数据索引（下标均从0开始）
    /// </summary>
    public static void CompareExcelFile(LangExcelInfo langExcelInfo, LangExcelInfo oldLangExcelInfo, out List<int> delectedKeyIndex, out List<int> newKeyIndex, out List<int> translationChangedIndex)
    {
        delectedKeyIndex = new List<int>();
        newKeyIndex = new List<int>();
        translationChangedIndex = new List<int>();

        List<LanguageInfo> languageInfoList = langExcelInfo.GetAllLanguageInfoList();
        List<LanguageInfo> oldLanguageInfoList = oldLangExcelInfo.GetAllLanguageInfoList();

        // 查找新表中已删除的Key
        List<string> keys = langExcelInfo.Keys;
        List<string> oldKeys = oldLangExcelInfo.Keys;

        int oldKeyCount = oldKeys.Count;
        for (int i = 0; i < oldKeyCount; ++i)
        {
            string oldKey = oldKeys[i];
            if (oldKey != null && !keys.Contains(oldKey))
                delectedKeyIndex.Add(i);
        }
        // 查找新增Key、主语言翻译变动
        int keyCount = keys.Count;
        List<string> defaultLanguageDataList = langExcelInfo.DefaultLanguageInfo.Data;
        List<string> oldDefaultLanguageDataList = oldLangExcelInfo.DefaultLanguageInfo.Data;
        for (int i = 0; i < keyCount; ++i)
        {
            string key = keys[i];
            if (key == null)
                continue;
            else if (!oldKeys.Contains(key))
                newKeyIndex.Add(i);
            else
            {
                string data = defaultLanguageDataList[i];
                if (data == null)
                    continue;
                else
                {
                    int dataIndexInOldLanguageFile = oldLangExcelInfo.KeyToDataIndex[key];
                    string oldData = oldDefaultLanguageDataList[dataIndexInOldLanguageFile];
                    if (!data.Equals(oldData))
                        translationChangedIndex.Add(i);
                }
            }
        }
    }

    /// <summary>
    /// 导出新版母表相对旧版新增Key、主语言翻译变动所在行信息到新建的Excel文件中。返回true表示生成了Excel文件，反之表示无需生成或存在错误
    /// </summary>
    public static bool ExportNeedTranslateExcelFile(string savePath, out string errorString, out string promptMessage)
    {
        promptMessage = null;
        errorString = null;

        // 旧表中含有但新表中已删除的Key所在旧表中的数据索引（下标从0开始）
        List<int> delectedKeyIndex = new List<int>();
        // 新表中新增Key所在新表中的数据索引（下标从0开始）
        List<int> newKeyIndex = new List<int>();
        // 新表中的主语言翻译相对旧表变动的Key所在新表中的数据索引（下标从0开始）
        List<int> translationChangedIndex = new List<int>();

        List<LanguageInfo> languageInfoList = AppValues.LangExcelInfo.GetAllLanguageInfoList();
        List<LanguageInfo> oldLanguageInfoList = AppValues.OldLangExcelInfo.GetAllLanguageInfoList();
        List<string> keys = AppValues.LangExcelInfo.Keys;
        List<string> oldKeys = AppValues.OldLangExcelInfo.Keys;
        List<string> defaultLanguageDataList = AppValues.LangExcelInfo.DefaultLanguageInfo.Data;
        List<string> oldDefaultLanguageDataList = AppValues.OldLangExcelInfo.DefaultLanguageInfo.Data;

        // 进行新旧母表对比
        CompareExcelFile(AppValues.LangExcelInfo, AppValues.OldLangExcelInfo, out delectedKeyIndex, out newKeyIndex, out translationChangedIndex);

        if (delectedKeyIndex.Count == 0 && newKeyIndex.Count == 0 && translationChangedIndex.Count == 0)
        {
            promptMessage = "新旧母表经对比未发现需要更新翻译的内容";
            return false;
        }
        // 新版母表中已删除的Key信息写入新建文本文件中，路径与选择导出的新建Excel文件相同
        if (delectedKeyIndex.Count > 0)
        {
            StringBuilder delectedKeyInfoBuilder = new StringBuilder();
            for (int i = 0; i < delectedKeyIndex.Count; ++i)
            {
                int dataIndex = delectedKeyIndex[i];
                delectedKeyInfoBuilder.AppendFormat("第{0}行，Key为\"{1}\"，主语言译文为\"{2}\"", dataIndex + AppValues.EXCEL_DATA_START_INDEX, oldKeys[dataIndex], oldDefaultLanguageDataList[dataIndex]).AppendLine();
            }

            string txtFileName = string.Format("新版母表相对于旧版已删除Key信息 {0:yyyy年MM月dd日 HH时mm分ss秒}.txt", DateTime.Now);
            string txtFileSavePath = Utils.CombinePath(Path.GetDirectoryName(savePath), txtFileName);
            if (Utils.SaveFile(txtFileSavePath, delectedKeyInfoBuilder.ToString(), out errorString) == true)
                promptMessage = string.Format("发现新版母表相对于旧版存在已删除的Key，相关信息已保存到{0}", txtFileSavePath);
            else
                promptMessage = string.Format("发现以下新版母表相对于旧版存在已删除的Key信息：\n{0}", delectedKeyInfoBuilder.ToString());
        }
        // 新增Key、主语言翻译变动信息需写入新建Excel文件中
        if (newKeyIndex.Count > 0 || translationChangedIndex.Count > 0)
        {
            // 导出待翻译内容到指定的新建Excel文件中
            Excel.Application application = new Excel.Application();
            // 不显示Excel窗口
            application.Visible = false;
            // 不显示警告对话框
            application.DisplayAlerts = false;
            // 禁止屏幕刷新
            application.ScreenUpdating = false;
            // 编辑非空单元格时不进行警告提示
            application.AlertBeforeOverwriting = false;
            // 新建Excel工作簿
            Excel.Workbook workbook = application.Workbooks.Add();
            // 在名为data的Sheet表中填充数据
            Excel.Worksheet dataWorksheet = workbook.Sheets[1] as Excel.Worksheet;
            dataWorksheet.Name = AppValues.EXCEL_DATA_SHEET_NAME.Replace("$", "");
            // 设置表格中所有单元格均为文本格式
            dataWorksheet.Cells.NumberFormatLocal = "@";

            // 写入待翻译的内容，列自左向右分别为Key、新版主语言译文、旧版主语言译文、旧版各语种译文（注意Excel中左上角单元格下标为[1,1]）
            // 定义各功能列在Excel的列号（从1开始计）
            const int EXCEL_KEY_COLUMN_INDEX = 1;
            const int EXCEL_DEFAULT_LANGUAGE_COLUMN_INDEX = 2;
            const int EXCEL_OLD_DEFAULT_LANGUAGE_COLUMN_INDEX = 3;
            const int EXCEL_OLD_OTHER_LANGUAGE_START_COLUMN_INDEX = 4;
            // 写入语种描述信息、名称
            dataWorksheet.Cells[AppValues.EXCEL_DESC_ROW_INDEX, EXCEL_KEY_COLUMN_INDEX] = "新增或主语言翻译变动的Key";
            dataWorksheet.Cells[AppValues.EXCEL_DESC_ROW_INDEX, EXCEL_DEFAULT_LANGUAGE_COLUMN_INDEX] = "新版母表中主语言译文";
            dataWorksheet.Cells[AppValues.EXCEL_NAME_ROW_INDEX, EXCEL_DEFAULT_LANGUAGE_COLUMN_INDEX] = languageInfoList[0].Name;
            dataWorksheet.Cells[AppValues.EXCEL_DESC_ROW_INDEX, EXCEL_OLD_DEFAULT_LANGUAGE_COLUMN_INDEX] = "旧版母表中主语言译文";
            int languageCount = languageInfoList.Count;
            int otherLanguageCount = languageCount - 1;
            for (int i = 1; i < languageCount; ++i)
            {
                string languageName = oldLanguageInfoList[i].Name;
                dataWorksheet.Cells[AppValues.EXCEL_DESC_ROW_INDEX, i + EXCEL_OLD_DEFAULT_LANGUAGE_COLUMN_INDEX] = string.Format("旧版{0}语种的译文", languageName);
                dataWorksheet.Cells[AppValues.EXCEL_NAME_ROW_INDEX, i + EXCEL_OLD_DEFAULT_LANGUAGE_COLUMN_INDEX] = languageName;
            }
            // 先将新版中主语言翻译变动内容写入新建的Excel文件
            int translationChangedCount = translationChangedIndex.Count;
            for (int i = 0; i < translationChangedCount; ++i)
            {
                int rowIndex = i + AppValues.EXCEL_DATA_START_INDEX;
                int excelDataIndex = translationChangedIndex[i];
                string key = keys[excelDataIndex];
                int oldExcelDataIndex = AppValues.OldLangExcelInfo.KeyToDataIndex[key];
                // Key
                dataWorksheet.Cells[rowIndex, EXCEL_KEY_COLUMN_INDEX] = key;
                // 新版主语言译文
                dataWorksheet.Cells[rowIndex, EXCEL_DEFAULT_LANGUAGE_COLUMN_INDEX] = defaultLanguageDataList[excelDataIndex];
                // 旧版主语言译文
                dataWorksheet.Cells[rowIndex, EXCEL_OLD_DEFAULT_LANGUAGE_COLUMN_INDEX] = oldDefaultLanguageDataList[oldExcelDataIndex];
                // 旧版各语种的译文
                for (int j = 1; j < otherLanguageCount + 1; ++j)
                {
                    int columnIndex = EXCEL_OLD_OTHER_LANGUAGE_START_COLUMN_INDEX + j - 1;
                    string data = oldLanguageInfoList[j].Data[oldExcelDataIndex];
                    dataWorksheet.Cells[rowIndex, columnIndex] = data;
                }
            }
            // 空出3行后，将新版母表中新增Key内容写入新建的Excel文件，只需写入Key和新版母表中的主语言译文
            const int SPACE_LINE_COUNT = 3;
            int newKeyDataStartRowIndex = AppValues.EXCEL_DATA_START_INDEX + translationChangedCount + SPACE_LINE_COUNT;
            if (translationChangedCount == 0)
                newKeyDataStartRowIndex = AppValues.EXCEL_DATA_START_INDEX;

            int newKeyCount = newKeyIndex.Count;
            for (int i = 0; i < newKeyCount; ++i)
            {
                int excelDataIndex = newKeyIndex[i];
                int rowIndex = i + newKeyDataStartRowIndex;
                // Key
                dataWorksheet.Cells[rowIndex, EXCEL_KEY_COLUMN_INDEX] = keys[excelDataIndex];
                // 新版主语言译文
                dataWorksheet.Cells[rowIndex, EXCEL_DEFAULT_LANGUAGE_COLUMN_INDEX] = defaultLanguageDataList[excelDataIndex];
            }

            // 对前2行配置行执行窗口冻结
            Excel.Range excelRange = dataWorksheet.get_Range(dataWorksheet.Cells[AppValues.EXCEL_DATA_START_INDEX, 1], dataWorksheet.Cells[AppValues.EXCEL_DATA_START_INDEX + 1, 1]);
            excelRange.Select();
            application.ActiveWindow.FreezePanes = true;

            // 美化生成的Excel文件
            int lastColumnIndex = EXCEL_OLD_OTHER_LANGUAGE_START_COLUMN_INDEX + otherLanguageCount - 1;
            _BeautifyExcelWorksheet(dataWorksheet, 30, lastColumnIndex);

            // 保存Excel
            dataWorksheet.SaveAs(savePath);
            workbook.SaveAs(savePath);
            // 关闭Excel
            workbook.Close(false);
            application.Workbooks.Close();
            application.Quit();
            Utils.KillExcelProcess(application);

            return true;
        }

        return false;
    }

    /// <summary>
    /// 复制最新母表，并将新增Key、主语言翻译变动所在行信息用指定颜色突出标注。返回true表示生成了Excel文件，反之表示无需生成或存在错误
    /// </summary>
    public static bool ExportComparedExcelFile(Color colorForAdd, Color colorForChange, string fillNullCellText, string savePath, out string errorString, out string promptMessage)
    {
        promptMessage = null;
        errorString = null;

        // 旧表中含有但新表中已删除的Key所在旧表中的数据索引（下标从0开始）
        List<int> delectedKeyIndex = new List<int>();
        // 新表中新增Key所在新表中的数据索引（下标从0开始）
        List<int> newKeyIndex = new List<int>();
        // 新表中的主语言翻译相对旧表变动的Key所在新表中的数据索引（下标从0开始）
        List<int> translationChangedIndex = new List<int>();

        List<string> oldKeys = AppValues.OldLangExcelInfo.Keys;
        List<string> oldDefaultLanguageDataList = AppValues.OldLangExcelInfo.DefaultLanguageInfo.Data;
        // 找到所有外语语种所在Excel文件中的列号（从1开始计）
        List<int> otherLanguageColumnIndex = new List<int>();
        foreach (LanguageInfo info in AppValues.LangExcelInfo.OtherLanguageInfo.Values)
            otherLanguageColumnIndex.Add(info.ColumnIndex);

        // 进行新旧母表对比
        CompareExcelFile(AppValues.LangExcelInfo, AppValues.OldLangExcelInfo, out delectedKeyIndex, out newKeyIndex, out translationChangedIndex);

        if (delectedKeyIndex.Count == 0 && newKeyIndex.Count == 0 && translationChangedIndex.Count == 0)
        {
            promptMessage = "新旧母表经对比未发现需要更新翻译的内容";
            return false;
        }
        // 新版母表中已删除的Key信息写入新建文本文件中，路径与选择导出的新建Excel文件相同
        if (delectedKeyIndex.Count > 0)
        {
            StringBuilder delectedKeyInfoBuilder = new StringBuilder();
            for (int i = 0; i < delectedKeyIndex.Count; ++i)
            {
                int dataIndex = delectedKeyIndex[i];
                delectedKeyInfoBuilder.AppendFormat("第{0}行，Key为\"{1}\"，主语言译文为\"{2}\"", dataIndex + AppValues.EXCEL_DATA_START_INDEX, oldKeys[dataIndex], oldDefaultLanguageDataList[dataIndex]).AppendLine();
            }

            string txtFileName = string.Format("新版母表相对于旧版已删除Key信息 {0:yyyy年MM月dd日 HH时mm分ss秒}.txt", DateTime.Now);
            string txtFileSavePath = Utils.CombinePath(Path.GetDirectoryName(savePath), txtFileName);
            if (Utils.SaveFile(txtFileSavePath, delectedKeyInfoBuilder.ToString(), out errorString) == true)
                promptMessage = string.Format("发现新版母表相对于旧版存在已删除的Key，相关信息已保存到{0}", txtFileSavePath);
            else
                promptMessage = string.Format("发现以下新版母表相对于旧版存在已删除的Key信息：\n{0}", delectedKeyInfoBuilder.ToString());
        }
        // 新增Key、主语言翻译变动所在行需在复制的新版母表中用指定背景色进行标注
        if (newKeyIndex.Count > 0 || translationChangedIndex.Count > 0)
        {
            // 复制新版母表
            FileState fileState = Utils.GetFileState(AppValues.ExcelFullPath);
            if (fileState == FileState.Inexist)
            {
                errorString = string.Format("新版母表所在路径（{0}）已不存在，请勿在使用本工具过程中对母表文件进行操作，导出操作被迫中止", AppValues.ExcelFullPath);
                return false;
            }
            try
            {
                File.Copy(AppValues.ExcelFullPath, savePath, true);
            }
            catch (Exception exception)
            {
                errorString = string.Format("复制新版母表（{0}）至指定路径（{1}）失败：{2}，导出操作被迫中止", AppValues.ExcelFullPath, savePath, exception.Message);
                return false;
            }

            // 打开复制后的母表
            // 导出待翻译内容到指定的新建Excel文件中
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
            Excel.Workbook workbook = application.Workbooks.Open(savePath);
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
                return false;
            }
            // 先将所有行的背景色清除
            dataWorksheet.Cells.Interior.ColorIndex = 0;
            // 将新增Key所在行背景色调为指定颜色并将对应译文部分统一填充为指定的字符串
            int newKeyCount = newKeyIndex.Count;
            for (int i = 0; i < newKeyCount; ++i)
            {
                int excelDataIndex = newKeyIndex[i];
                int rowIndex = excelDataIndex + AppValues.EXCEL_DATA_START_INDEX;
                // 调整背景色
                dataWorksheet.get_Range(string.Concat("A", rowIndex)).EntireRow.Interior.Color = ColorTranslator.ToOle(colorForAdd);
                // 新增Key所在行的外语单元格填充为指定的字符串
                foreach (int columnIndex in otherLanguageColumnIndex)
                    dataWorksheet.Cells[rowIndex, columnIndex] = fillNullCellText;
            }
            // 将翻译变动Key所在行背景色调为指定颜色，保留新版母表中储存的旧版外语译文
            int translationChangedCount = translationChangedIndex.Count;
            for (int i = 0; i < translationChangedCount; ++i)
            {
                int excelDataIndex = translationChangedIndex[i];
                int rowIndex = excelDataIndex + AppValues.EXCEL_DATA_START_INDEX;
                // 调整背景色
                dataWorksheet.get_Range(string.Concat("A", rowIndex)).EntireRow.Interior.Color = ColorTranslator.ToOle(colorForChange);
            }

            // 保存Excel
            dataWorksheet.SaveAs(savePath);
            workbook.SaveAs(savePath);
            // 关闭Excel
            workbook.Close(false);
            application.Workbooks.Close();
            application.Quit();
            Utils.KillExcelProcess(application);

            return true;
        }

        return false;
    }

    /// <summary>
    /// 复制最新母表，将翻译完的Excel文件内容与之合并，并将合并结果报告写入新建的Excel文件中
    /// </summary>
    public static bool ExportMergedExcelFile(string mergedExcelSavePath, string reportExcelSavePath, LangExcelInfo langExcelInfo, LangExcelInfo translatedLangExcelInfo, List<string> mergeLanguageNames, out string errorString)
    {
        int languageCount = mergeLanguageNames.Count;
        // 记录合并翻译时发现的新版母表与翻译完的Excel文件中Key相同但主语言翻译不同信息
        List<MergedResultDifferentDefaultLanguageInfo> differentDefaultLanguageInfo = new List<MergedResultDifferentDefaultLanguageInfo>();
        // 记录合并翻译时发现的新版母表与翻译完的Excel文件中Key不同信息
        List<MergedResultDifferentKeyInfo> differentKeyInfo = new List<MergedResultDifferentKeyInfo>();
        // 记录各个报告部分起始行行号
        List<int> partStartRowIndexList = new List<int>();
        partStartRowIndexList.Add(1);

        // 复制新版母表
        FileState fileState = Utils.GetFileState(AppValues.ExcelFullPath);
        if (fileState == FileState.Inexist)
        {
            errorString = string.Format("新版母表所在路径（{0}）已不存在，请勿在使用本工具过程中对母表文件进行操作，合并操作被迫中止", AppValues.ExcelFullPath);
            return false;
        }
        try
        {
            File.Copy(AppValues.ExcelFullPath, mergedExcelSavePath, true);
        }
        catch (Exception exception)
        {
            errorString = string.Format("复制新版母表（{0}）至指定路径（{1}）失败：{2}，合并操作被迫中止", AppValues.ExcelFullPath, mergedExcelSavePath, exception.Message);
            return false;
        }

        // 打开复制后的母表，将翻译完的Excel文件中的内容与之合并
        Excel.Application mergedApplication = new Excel.Application();
        // 不显示Excel窗口
        mergedApplication.Visible = false;
        // 不显示警告对话框
        mergedApplication.DisplayAlerts = false;
        // 禁止屏幕刷新
        mergedApplication.ScreenUpdating = false;
        // 编辑非空单元格时不进行警告提示
        mergedApplication.AlertBeforeOverwriting = false;
        // 打开Excel工作簿
        Excel.Workbook mergedWorkbook = mergedApplication.Workbooks.Open(mergedExcelSavePath);
        // 找到名为data的Sheet表
        Excel.Worksheet mergedDataWorksheet = null;
        int sheetCount = mergedWorkbook.Sheets.Count;
        string DATA_SHEET_NAME = AppValues.EXCEL_DATA_SHEET_NAME.Replace("$", "");
        for (int i = 1; i <= sheetCount; ++i)
        {
            Excel.Worksheet sheet = mergedWorkbook.Sheets[i] as Excel.Worksheet;
            if (sheet.Name.Equals(DATA_SHEET_NAME))
            {
                mergedDataWorksheet = sheet;
                break;
            }
        }
        if (mergedDataWorksheet == null)
        {
            errorString = string.Format("新版母表（{0}）找不到Sheet名为{1}的数据表，请勿在使用本工具过程中对母表文件进行操作，导出操作被迫中止", AppValues.ExcelFullPath, DATA_SHEET_NAME);
            return false;
        }

        // 还要新建一张新表，保存合并报告
        Excel.Application reportApplication = new Excel.Application();
        reportApplication.Visible = false;
        reportApplication.DisplayAlerts = false;
        reportApplication.ScreenUpdating = false;
        reportApplication.AlertBeforeOverwriting = false;
        // 新建Excel工作簿
        Excel.Workbook reportWorkbook = reportApplication.Workbooks.Add();
        // 在名为合并报告的Sheet表中填充数据
        Excel.Worksheet reportWorksheet = reportWorkbook.Sheets[1] as Excel.Worksheet;
        reportWorksheet.Name = "合并报告";
        // 设置表格中所有单元格均为文本格式
        reportWorksheet.Cells.NumberFormatLocal = "@";
        // 报告Excel文件中依次按Key与主语言译文均相同、Key相同但主语言译文不同、母表不存在指定Key分成三部分进行报告
        // 不同部分之间隔开的行数
        const int SPACE_LINE_COUNT = 3;
        // Key与主语言译文相同的报告部分，列依次为Key名、母表行号、翻译完的Excel文件中的行号、主语言译文、各外语译文，其中用无色背景标识两表译文相同的单元格，用绿色背景标识母表未翻译而翻译完的Excel文件中新增译文的单元格，用黄色背景标识译文不同的单元格（并以批注形式写入母表中旧的译文）
        const int ALL_SAME_KEY_COLUMN_INDEX = 1;
        const int ALL_SAME_FILE_LINE_NUM_COLUMN_INDEX = 2;
        const int ALL_SAME_TRANSLATED_FILE_LINE_NUM_COLUMN_INDEX = 3;
        const int ALL_SAME_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX = 4;
        const int ALL_SAME_OTHER_LANGUAGE_START_COLUMN_INDEX = 5;
        // 每个部分首行写入说明文字
        reportWorksheet.Cells[1, 1] = "以下为已合并的译文报告，其中用无色背景标识两表译文相同的单元格，用绿色背景标识母表未翻译而翻译完的Excel文件中新增译文的单元格，用黄色背景标识译文不同的单元格（并以批注形式写入母表中旧的译文）";
        // 写入Key与主语言译文均相同部分的列标题说明
        reportWorksheet.Cells[2, ALL_SAME_KEY_COLUMN_INDEX] = "Key名";
        reportWorksheet.Cells[2, ALL_SAME_FILE_LINE_NUM_COLUMN_INDEX] = "母表中的行号";
        reportWorksheet.Cells[2, ALL_SAME_TRANSLATED_FILE_LINE_NUM_COLUMN_INDEX] = "翻译完的Excel表中的行号";
        reportWorksheet.Cells[2, ALL_SAME_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX] = "主语言译文";
        for (int i = 0; i < languageCount; ++i)
        {
            int columnIndex = ALL_SAME_OTHER_LANGUAGE_START_COLUMN_INDEX + i;
            reportWorksheet.Cells[2, columnIndex] = mergeLanguageNames[i];
        }
        // 当前报告Excel表中下一个可用空行的行号（从1开始计）
        int nextCellLineNum = 3;
        // 逐行读取翻译完的Excel表中的内容并与最新母表比较，若Key相同主语言翻译相同，直接将翻译完的Excel表中对应的外语译文合并到母表，若Key相同但主语言翻译不同或者翻译完的Excel表中存在母表中已没有的Key则不合并且记入报告
        int translatedExcelDataCount = translatedLangExcelInfo.Keys.Count;
        for (int i = 0; i < translatedExcelDataCount; ++i)
        {
            string mergedExcelKey = translatedLangExcelInfo.Keys[i];
            if (mergedExcelKey == null)
                continue;

            // 判断母表中是否存在指定Key
            if (langExcelInfo.Keys.Contains(mergedExcelKey))
            {
                // 判断母表与翻译完的Excel文件中该Key对应的主语言译文是否相同
                // 母表中该Key所在行的数据索引
                int excelDataIndex = langExcelInfo.KeyToDataIndex[mergedExcelKey];
                string excelDefaultLanguageValue = langExcelInfo.DefaultLanguageInfo.Data[excelDataIndex];
                string translatedExcelDefaultLanguageValue = translatedLangExcelInfo.DefaultLanguageInfo.Data[i];
                if (excelDefaultLanguageValue.Equals(translatedExcelDefaultLanguageValue))
                {
                    // 如果该行外语的翻译均相同，则无需合并且不需要记入报告
                    bool isAllSame = true;
                    foreach (string languageName in mergeLanguageNames)
                    {
                        string excelLanguageValue = langExcelInfo.OtherLanguageInfo[languageName].Data[excelDataIndex];
                        string translatedLanguageValue = translatedLangExcelInfo.OtherLanguageInfo[languageName].Data[i];
                        if (!excelLanguageValue.Equals(translatedLanguageValue))
                        {
                            isAllSame = false;
                            break;
                        }
                    }
                    // 存在不同的译文，则要合并到母表中并记入报告
                    if (isAllSame == false)
                    {
                        reportWorksheet.Cells[nextCellLineNum, ALL_SAME_KEY_COLUMN_INDEX] = mergedExcelKey;
                        reportWorksheet.Cells[nextCellLineNum, ALL_SAME_FILE_LINE_NUM_COLUMN_INDEX] = excelDataIndex + AppValues.EXCEL_DATA_START_INDEX;
                        reportWorksheet.Cells[nextCellLineNum, ALL_SAME_TRANSLATED_FILE_LINE_NUM_COLUMN_INDEX] = i + AppValues.EXCEL_DATA_START_INDEX;
                        reportWorksheet.Cells[nextCellLineNum, ALL_SAME_TRANSLATED_FILE_LINE_NUM_COLUMN_INDEX] = i + AppValues.EXCEL_DATA_START_INDEX;
                        reportWorksheet.Cells[nextCellLineNum, ALL_SAME_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX] = translatedExcelDefaultLanguageValue;

                        for (int j = 0; j < languageCount; ++j)
                        {
                            string languageName = mergeLanguageNames[j];
                            string excelLanguageValue = langExcelInfo.OtherLanguageInfo[languageName].Data[excelDataIndex];
                            string translatedLanguageValue = translatedLangExcelInfo.OtherLanguageInfo[languageName].Data[i];
                            int columnIndex = ALL_SAME_OTHER_LANGUAGE_START_COLUMN_INDEX + j;
                            // 报告中外语列单元格都要写入翻译完的Excel文件中对应的译文
                            reportWorksheet.Cells[nextCellLineNum, columnIndex] = translatedLanguageValue;
                            if (!excelLanguageValue.Equals(translatedLanguageValue))
                            {
                                int mergedExcelRowIndex = excelDataIndex + AppValues.EXCEL_DATA_START_INDEX;
                                int mergedExcelColumnIndex = langExcelInfo.OtherLanguageInfo[languageName].ColumnIndex;

                                if (string.IsNullOrEmpty(excelLanguageValue))
                                {
                                    // 母表中原来没有译文，则将报告Excel表中对应单元格背景色设为绿色
                                    reportWorksheet.get_Range(reportWorksheet.Cells[nextCellLineNum, columnIndex], reportWorksheet.Cells[nextCellLineNum, columnIndex]).Interior.ColorIndex = 4;
                                }
                                else
                                {
                                    // 母表中和翻译完的Excel表中译文不同，则用黄色背景标识译文不同的单元格（并以批注形式写入母表中旧的译文）
                                    reportWorksheet.get_Range(reportWorksheet.Cells[nextCellLineNum, columnIndex], reportWorksheet.Cells[nextCellLineNum, columnIndex]).Interior.ColorIndex = 6;
                                    reportWorksheet.get_Range(reportWorksheet.Cells[nextCellLineNum, columnIndex], reportWorksheet.Cells[nextCellLineNum, columnIndex]).AddComment(string.Concat("母表中旧的译文：", System.Environment.NewLine, excelLanguageValue));
                                }

                                // 将翻译完的Excel表中的译文写入母表
                                mergedDataWorksheet.Cells[mergedExcelRowIndex, mergedExcelColumnIndex] = translatedLanguageValue;
                            }
                        }

                        ++nextCellLineNum;
                    }
                }
                else
                {
                    // Key相同，主语言译文不同则不合并且记入报告
                    MergedResultDifferentDefaultLanguageInfo info = new MergedResultDifferentDefaultLanguageInfo();

                    info.ExcelLineNum = excelDataIndex + AppValues.EXCEL_DATA_START_INDEX;
                    info.TranslatedExcelLineNum = i + AppValues.EXCEL_DATA_START_INDEX;
                    info.Key = mergedExcelKey;
                    info.ExcelDefaultLanguageValue = excelDefaultLanguageValue;
                    info.TranslatedExcelDefaultLanguageValue = translatedExcelDefaultLanguageValue;

                    differentDefaultLanguageInfo.Add(info);
                }
            }
            else
            {
                // 翻译完的Excel表中存在母表中已没有的Key则不合并且记入报告
                MergedResultDifferentKeyInfo info = new MergedResultDifferentKeyInfo();
                info.TranslatedExcelLineNum = i + AppValues.EXCEL_DATA_START_INDEX;
                info.Key = mergedExcelKey;
                info.TranslatedExcelDefaultLanguageValue = translatedLangExcelInfo.DefaultLanguageInfo.Data[i];

                differentKeyInfo.Add(info);
            }
        }
        // 设置框线及标题行格式
        _FormatPart(reportWorksheet, 1, nextCellLineNum - 1, ALL_SAME_OTHER_LANGUAGE_START_COLUMN_INDEX + languageCount - 1);

        // Key相同但主语言译文不同的报告部分，列依次为Key名、母表行号、翻译完的Excel文件中的行号、母表中主语言译文、翻译完的Excel文件中主语言译文
        const int KEY_SAME_KEY_COLUMN_INDEX = 1;
        const int KEY_SAME_FILE_LINE_NUM_COLUMN_INDEX = 2;
        const int KEY_SAME_TRANSLATED_FILE_LINE_NUM_COLUMN_INDEX = 3;
        const int KEY_SAME_EXCEL_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX = 4;
        const int KEY_SAME_TRANSLATED_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX = 5;
        if (differentDefaultLanguageInfo.Count > 0)
        {
            nextCellLineNum = nextCellLineNum + SPACE_LINE_COUNT;
            partStartRowIndexList.Add(nextCellLineNum);
            // 每个部分首行写入说明文字
            reportWorksheet.Cells[nextCellLineNum, 1] = "以下为母表与翻译完的Excel表中Key相同但主语言译文不同，无法进行合并的信息";
            ++nextCellLineNum;
            // 写入Key相同但主语言译文不同部分的列标题说明
            reportWorksheet.Cells[nextCellLineNum, KEY_SAME_KEY_COLUMN_INDEX] = "Key名";
            reportWorksheet.Cells[nextCellLineNum, KEY_SAME_FILE_LINE_NUM_COLUMN_INDEX] = "母表中的行号";
            reportWorksheet.Cells[nextCellLineNum, KEY_SAME_TRANSLATED_FILE_LINE_NUM_COLUMN_INDEX] = "翻译完的Excel表中的行号";
            reportWorksheet.Cells[nextCellLineNum, KEY_SAME_EXCEL_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX] = "母表中主语言译文";
            reportWorksheet.Cells[nextCellLineNum, KEY_SAME_TRANSLATED_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX] = "翻译完的Excel文件中主语言译文";
            ++nextCellLineNum;
            // 将所有Key相同但主语言译文不同信息写入报告
            foreach (MergedResultDifferentDefaultLanguageInfo info in differentDefaultLanguageInfo)
            {
                reportWorksheet.Cells[nextCellLineNum, KEY_SAME_KEY_COLUMN_INDEX] = info.Key;
                reportWorksheet.Cells[nextCellLineNum, KEY_SAME_FILE_LINE_NUM_COLUMN_INDEX] = info.ExcelLineNum;
                reportWorksheet.Cells[nextCellLineNum, KEY_SAME_TRANSLATED_FILE_LINE_NUM_COLUMN_INDEX] = info.TranslatedExcelLineNum;
                reportWorksheet.Cells[nextCellLineNum, KEY_SAME_EXCEL_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX] = info.ExcelDefaultLanguageValue;
                reportWorksheet.Cells[nextCellLineNum, KEY_SAME_TRANSLATED_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX] = info.TranslatedExcelDefaultLanguageValue;

                ++nextCellLineNum;
            }
        }
        // 设置框线及标题行格式
        _FormatPart(reportWorksheet, partStartRowIndexList[1], nextCellLineNum - 1, KEY_SAME_TRANSLATED_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX);

        // 母表不存在指定Key的报告部分，列依次为Key名、翻译完的Excel文件中的行号以及主语言译文
        const int KEY_DIFFERENT_KEY_COLUMN_INDEX = 1;
        const int KEY_DIFFERENT_TRANSLATED_FILE_LINE_NUM_COLUMN_INDEX = 2;
        const int KEY_DIFFERENT_TRANSLATED_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX = 3;
        if (differentKeyInfo.Count > 0)
        {
            nextCellLineNum = nextCellLineNum + SPACE_LINE_COUNT;
            partStartRowIndexList.Add(nextCellLineNum);
            // 每个部分首行写入说明文字
            reportWorksheet.Cells[nextCellLineNum, 1] = "以下为翻译完的Excel文件含有但母表已经没有的Key，无法进行合并的信息";
            ++nextCellLineNum;
            // 写入母表中已没有的Key部分的列标题说明
            reportWorksheet.Cells[nextCellLineNum, KEY_DIFFERENT_KEY_COLUMN_INDEX] = "Key名";
            reportWorksheet.Cells[nextCellLineNum, KEY_DIFFERENT_TRANSLATED_FILE_LINE_NUM_COLUMN_INDEX] = "翻译完的Excel表中的行号";
            reportWorksheet.Cells[nextCellLineNum, KEY_DIFFERENT_TRANSLATED_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX] = "翻译完的Excel文件中主语言译文";
            ++nextCellLineNum;
            // 将所有母表中已没有的Key信息写入报告
            foreach (MergedResultDifferentKeyInfo info in differentKeyInfo)
            {
                reportWorksheet.Cells[nextCellLineNum, KEY_DIFFERENT_KEY_COLUMN_INDEX] = info.Key;
                reportWorksheet.Cells[nextCellLineNum, KEY_DIFFERENT_TRANSLATED_FILE_LINE_NUM_COLUMN_INDEX] = info.TranslatedExcelLineNum;
                reportWorksheet.Cells[nextCellLineNum, KEY_DIFFERENT_TRANSLATED_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX] = info.TranslatedExcelDefaultLanguageValue;

                ++nextCellLineNum;
            }
        }
        // 设置框线及标题行格式
        _FormatPart(reportWorksheet, partStartRowIndexList[2], nextCellLineNum - 1, KEY_DIFFERENT_TRANSLATED_DEFAULT_LANGUAGE_VALUE_COLUMN_INDEX);

        // 美化生成的Excel文件
        _BeautifyExcelWorksheet(reportWorksheet, 40, nextCellLineNum - 1);

        // 因为Excel中执行过合并的单元格即便设置了自动换行也无法实现效果，故为了防止每个部分首行的描述文字不完全可见，手工修改其行高
        for (int i = 0; i < partStartRowIndexList.Count; ++i)
        {
            int partStartRowIndex = partStartRowIndexList[i];
            reportWorksheet.get_Range("A" + partStartRowIndex).EntireRow.RowHeight = 80;
        }

        // 保存报告Excel文件
        reportWorksheet.SaveAs(reportExcelSavePath);
        reportWorkbook.SaveAs(reportExcelSavePath);
        // 关闭Excel
        reportWorkbook.Close(false);
        reportApplication.Workbooks.Close();
        reportApplication.Quit();
        Utils.KillExcelProcess(reportApplication);

        // 保存合并后的Excel文件
        mergedDataWorksheet.SaveAs(mergedExcelSavePath);
        mergedWorkbook.SaveAs(mergedExcelSavePath);
        // 关闭Excel
        mergedWorkbook.Close(false);
        mergedApplication.Workbooks.Close();
        mergedApplication.Quit();
        Utils.KillExcelProcess(mergedApplication);

        errorString = null;
        return true;
    }

    /// <summary>
    /// 格式化每个报告部分的报告内容，将每个部分设置粗实线外边框，首行（部分描述行）字体设为16号加粗并合并单元格，第二行（列字段说明行）设为细实线内外边框绿色背景、字体加粗
    /// </summary>
    public static void _FormatPart(Worksheet worksheet, int startRowIndex, int endRowIndex, int columnCount)
    {
        // 首行（部分描述行）字体设为16号加粗并合并单元格
        worksheet.get_Range(worksheet.Cells[startRowIndex, 1], worksheet.Cells[startRowIndex, columnCount]).Font.Bold = true;
        worksheet.get_Range(worksheet.Cells[startRowIndex, 1], worksheet.Cells[startRowIndex, columnCount]).Font.Size = 16;
        worksheet.get_Range(worksheet.Cells[startRowIndex, 1], worksheet.Cells[startRowIndex, columnCount]).Merge();
        // 第二行（列字段说明行）设为细实线内外边框绿色背景、字体加粗
        int secondLineIndex = startRowIndex + 1;
        worksheet.get_Range(worksheet.Cells[secondLineIndex, 1], worksheet.Cells[secondLineIndex, columnCount]).Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThin;
        worksheet.get_Range(worksheet.Cells[secondLineIndex, 1], worksheet.Cells[secondLineIndex, columnCount]).Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;
        worksheet.get_Range(worksheet.Cells[secondLineIndex, 1], worksheet.Cells[secondLineIndex, columnCount]).Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThin;
        worksheet.get_Range(worksheet.Cells[secondLineIndex, 1], worksheet.Cells[secondLineIndex, columnCount]).Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
        worksheet.get_Range(worksheet.Cells[secondLineIndex, 1], worksheet.Cells[secondLineIndex, columnCount]).Borders[XlBordersIndex.xlInsideHorizontal].Weight = XlBorderWeight.xlThin;
        worksheet.get_Range(worksheet.Cells[secondLineIndex, 1], worksheet.Cells[secondLineIndex, columnCount]).Borders[XlBordersIndex.xlInsideVertical].Weight = XlBorderWeight.xlThin;
        worksheet.get_Range(worksheet.Cells[secondLineIndex, 1], worksheet.Cells[secondLineIndex, columnCount]).Interior.ColorIndex = 35;
        worksheet.get_Range(worksheet.Cells[secondLineIndex, 1], worksheet.Cells[secondLineIndex, columnCount]).Font.Bold = true;
        // 每个部分设置粗实线外边框
        worksheet.get_Range(worksheet.Cells[startRowIndex, 1], worksheet.Cells[endRowIndex, columnCount]).Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThick;
        worksheet.get_Range(worksheet.Cells[startRowIndex, 1], worksheet.Cells[endRowIndex, columnCount]).Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThick;
        worksheet.get_Range(worksheet.Cells[startRowIndex, 1], worksheet.Cells[endRowIndex, columnCount]).Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThick;
        worksheet.get_Range(worksheet.Cells[startRowIndex, 1], worksheet.Cells[endRowIndex, columnCount]).Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThick;
    }

    /// <summary>
    /// 美化Excel文件中指定工作簿，设置单元格自动列宽（使得列宽根据内容自动调整，每个单元格在一行中可显示完整内容）。然后对于因内容过多而通过自动列宽后超过指定最大列宽的单元格，强制缩小列宽到所允许的最大宽度。最后设置单元格内容自动换行，使得单元格自动扩大高度以显示所有内容
    /// 注意执行本操作需在插入完所有数据后进行，否则插入数据前设置自动列宽无效
    /// </summary>
    public static void _BeautifyExcelWorksheet(Worksheet worksheet, int excelColumnMaxWidth, int lastColumnIndex)
    {
        // 设置表格中所有单元格均自动列宽
        worksheet.Columns.AutoFit();
        // 对于因内容过多而通过自动列宽后超过配置文件中配置的最大列宽的单元格，强制缩小列宽到所允许的最大宽度
        for (int columnIndex = 1; columnIndex <= lastColumnIndex; ++columnIndex)
        {
            double columnWidth = Convert.ToDouble(worksheet.get_Range(Utils.GetExcelColumnName(columnIndex) + "1").EntireColumn.ColumnWidth);
            if (columnWidth > excelColumnMaxWidth)
                worksheet.get_Range(Utils.GetExcelColumnName(columnIndex) + "1").EntireColumn.ColumnWidth = excelColumnMaxWidth;
        }
        // 设置表格中所有单元格均自动换行
        worksheet.Cells.WrapText = true;
    }
}

/// <summary>
/// 用于记录一条合并翻译时发现的新版母表与翻译完的Excel文件中Key相同但主语言翻译不同信息
/// </summary>
public struct MergedResultDifferentDefaultLanguageInfo
{
    // 母表中的行号
    public int ExcelLineNum;
    // 翻译完的Excel文件中的行号
    public int TranslatedExcelLineNum;
    // Key名
    public string Key;
    // 母表中的主语言译文
    public string ExcelDefaultLanguageValue;
    // 翻译完的Excel文件中的主语言译文
    public string TranslatedExcelDefaultLanguageValue;
}

/// <summary>
/// 用于记录一条合并翻译时发现的新版母表与翻译完的Excel文件中Key不同信息
/// </summary>
public struct MergedResultDifferentKeyInfo
{
    // 翻译完的Excel文件中的行号
    public int TranslatedExcelLineNum;
    // Key名
    public string Key;
    // 翻译完的Excel文件中的主语言译文
    public string TranslatedExcelDefaultLanguageValue;
}
