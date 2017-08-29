using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class TxtConfigReader
{
    /// <summary>
    /// 将文本文件中的键值对读取并加入Dictionary
    /// </summary>
    public static Dictionary<string, string> ParseTxtConfigFile(string filePath, string separator, bool isAddNullValue, bool isWarnNullValue, out string errorString)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        StringBuilder errorStringBuilder = new StringBuilder();

        using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
        {
            string line = null;
            int lineNumber = 0;
            while ((line = reader.ReadLine()) != null)
            {
                ++lineNumber;

                // 以#开头的注释行和空行忽略
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;
                // 找到key、value
                int separatorIndex = line.IndexOf(separator);
                if (separatorIndex == -1)
                {
                    errorStringBuilder.AppendFormat("第{0}行（内容为：{1}）不包含分隔符{2}\n", lineNumber, line, separator);
                    continue;
                }
                // 获取key
                string key = line.Substring(0, separatorIndex);
                if (string.IsNullOrEmpty(key))
                {
                    errorStringBuilder.AppendFormat("第{0}行（内容为：{1}）不包含Key\n", lineNumber, line);
                    continue;
                }
                if (result.ContainsKey(key))
                {
                    errorStringBuilder.AppendFormat("第{0}行中的key({1})重复定义\n", lineNumber, key);
                    continue;
                }
                // 获取value
                string value = line.Substring(separatorIndex + 1);
                if (string.IsNullOrEmpty(value))
                {
                    if (isWarnNullValue == true)
                        errorStringBuilder.AppendFormat("第{0}行中的key({1})未声明对应的Value\n", lineNumber, key);
                    if (isAddNullValue == true)
                        result.Add(key, value);
                }
                else
                    result.Add(key, value);
            }
        }

        errorString = errorStringBuilder.ToString();
        if (string.IsNullOrEmpty(errorString))
            errorString = null;

        return result;
    }
}
