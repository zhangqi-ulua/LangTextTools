using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// 该类用于导出各语种对应的lang文本文件
/// </summary>
public class ExportLangFileHelper
{
    public static bool ExportLangFile(string languageName, string savePath, out string errorString)
    {
        // 检查输入的路径是否合法
        string fullPath = null;
        try
        {
            fullPath = Path.GetFullPath(savePath);
        }
        catch
        {
            errorString = "路径非法";
            return false;
        }
        string dirPath = Path.GetDirectoryName(fullPath);
        if (string.IsNullOrEmpty(Path.GetFileName(fullPath)))
        {
            errorString = "路径非法，未输入文件名";
            return false;
        }
        if (!Directory.Exists(dirPath))
        {
            try
            {
                Directory.CreateDirectory(dirPath);
            }
            catch (Exception exception)
            {
                errorString = string.Format("创建目录（{0}）失败：{1}", dirPath, exception.Message);
                return false;
            }
        }
        if (string.IsNullOrEmpty(Path.GetExtension(fullPath)))
            fullPath = string.Concat(fullPath, ".", AppValues.LangFileExtension);

        // 记录未填写译文的数据索引（下标从0开始）
        List<int> untranslatedDataIndex = new List<int>();
        // 记录要写入lang文件的内容
        StringBuilder langFileContent = new StringBuilder();

        List<string> keys = AppValues.LangExcelInfo.Keys;
        List<string> translatedText = AppValues.LangExcelInfo.GetLanguageInfoByLanguageName(languageName).Data;
        int keyCount = keys.Count;
        for (int i = 0; i < keyCount; ++i)
        {
            if (keys[i] == null)
                continue;

            if (string.IsNullOrEmpty(translatedText[i]))
            {
                untranslatedDataIndex.Add(i);
                // 当发现有未翻译的问题时，该lang文件不会进行导出，后面也无需组织要写入lang文件的内容，只需继续检查后面是否还有未翻译的问题
                for (int j = i + 1; j < keyCount; ++j)
                {
                    if (keys[j] == null)
                        continue;

                    if (string.IsNullOrEmpty(translatedText[j]))
                        untranslatedDataIndex.Add(j);
                }
                break;
            }
            else
                langFileContent.AppendLine(string.Concat(keys[i], AppValues.KeyAndValueSplitChar, translatedText[i]));
        }

        if (untranslatedDataIndex.Count > 0)
        {
            StringBuilder errorStringBuilder = new StringBuilder();
            errorStringBuilder.AppendLine("错误：在以下行中存在未翻译的内容，请完全翻译后重试");
            foreach (int dataIndex in untranslatedDataIndex)
                errorStringBuilder.AppendFormat("第{0}行，Key为\"{1}\"", dataIndex + AppValues.EXCEL_DATA_START_INDEX, keys[dataIndex]).AppendLine();

            errorString = errorStringBuilder.ToString();
            return false;
        }
        // 检查无误后生成lang文件
        if (Utils.SaveFile(fullPath, langFileContent.ToString(), out errorString) == true)
            return true;
        else
        {
            errorString = string.Concat("生成lang文件失败：", errorString);
            return false;
        }
    }
}
