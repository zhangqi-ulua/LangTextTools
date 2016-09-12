using SharpSvn;
using SharpSvn.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

/// <summary>
/// 该类用于操作SVN
/// </summary>
public class OperateSvnHelper
{
    private static SvnClient _svnClient;

    static OperateSvnHelper()
    {
        _svnClient = new SvnClient();

        // 绑定图形化SVN操作窗口，比如当需要对SVN鉴权时弹出账户密码输入对话框
        SvnUIBindArgs uiBindArgs = new SvnUIBindArgs();
        SvnUI.Bind(_svnClient, uiBindArgs);
    }

    /// <summary>
    /// 获取本地Working Copy中某个文件的信息
    /// </summary>
    public static SvnInfoEventArgs GetLocalFileInfo(string localPath, out SvnException svnException)
    {
        SvnInfoEventArgs localFileInfo;
        SvnPathTarget localPathTarget = new SvnPathTarget(localPath);

        try
        {
            _svnClient.GetInfo(localPathTarget, out localFileInfo);
            svnException = null;
            return localFileInfo;
        }
        catch (SvnException exception)
        {
            svnException = exception;
            return null;
        }
    }

    /// <summary>
    /// 获取SVN中某个文件的信息
    /// </summary>
    public static SvnInfoEventArgs GetSvnFileInfo(string svnPath, out SvnException svnException)
    {
        SvnInfoEventArgs svnFileInfo;
        SvnUriTarget svnUriTarger = new SvnUriTarget(svnPath);

        try
        {
            _svnClient.GetInfo(svnPath, out svnFileInfo);
            svnException = null;
            return svnFileInfo;
        }
        catch (SvnException exception)
        {
            svnException = exception;
            return null;
        }
    }

    /// <summary>
    /// 获取本地Working Copy中某个文件的状态
    /// </summary>
    public static SvnStatusEventArgs GetLocalFileState(string localPath, out SvnException svnException)
    {
        Collection<SvnStatusEventArgs> status = new Collection<SvnStatusEventArgs>();
        SvnStatusArgs statusArgs = new SvnStatusArgs();
        statusArgs.RetrieveAllEntries = true;

        try
        {
            _svnClient.GetStatus(localPath, statusArgs, out status);
            if (status.Count > 0)
            {
                svnException = null;
                return status[0];
            }
            else
            {
                svnException = new SvnException("未知原因导致无法读取本地文件信息");
                return null;
            }
        }
        catch (SvnException exception)
        {
            svnException = exception;
            return null;
        }
    }

    /// <summary>
    /// 执行SVN的Revert操作
    /// </summary>
    public static bool Revert(string localPath, out SvnException svnException)
    {
        try
        {
            bool result = _svnClient.Revert(localPath);
            svnException = null;
            return result;
        }
        catch (SvnException exception)
        {
            svnException = exception;
            return false;
        }
    }

    /// <summary>
    /// 执行SVN的Update操作
    /// </summary>
    public static bool Update(string localPath, out SvnException svnException)
    {
        try
        {
            bool result = _svnClient.Update(localPath);
            svnException = null;
            return result;
        }
        catch (SvnException exception)
        {
            svnException = exception;
            return false;
        }
    }

    /// <summary>
    /// 执行SVN的Commit操作
    /// </summary>
    public static bool Commit(string localFilePath, string logMessage, out SvnException svnException)
    {
        try
        {
            SvnCommitArgs commitArgs = new SvnCommitArgs();
            commitArgs.LogMessage = logMessage;
            bool result = _svnClient.Commit(localFilePath, commitArgs);
            svnException = null;
            return result;
        }
        catch (SvnException exception)
        {
            svnException = exception;
            return false;
        }
    }

    /// <summary>
    /// 将SVN中某文件的某个版本保存到本地指定路径
    /// </summary>
    public static bool ExportSvnFileToLocal(string svnFilePath, string savePath, SvnRevision svnRevision, out Exception exception)
    {
        SvnExportArgs exportArgs = new SvnExportArgs();
        exportArgs.Overwrite = true;
        exportArgs.Revision = svnRevision;
        try
        {
            bool result = _svnClient.Export(svnFilePath, savePath, exportArgs);
            exception = null;
            return result;
        }
        catch (Exception e)
        {
            exception = e;
            return false;
        }
    }

    /// <summary>
    /// 获取某个本地文件相较SVN中状态的描述
    /// </summary>
    public static string GetSvnStatusDescription(SvnStatus svnStatus)
    {
        switch (svnStatus)
        {
            case SvnStatus.Zero:
                // Zero value. Never used by Subversion
                return "无法获取状态";
            case SvnStatus.None:
                // does not exist
                return "文件不存在";
            case SvnStatus.NotVersioned:
                // is not a versioned thing in this wc
                return "本地文件不被SVN管理";
            case SvnStatus.Normal:
                // exists, but uninteresting
                return "本地文件与SVN中对应版本的内容完全一致";
            case SvnStatus.Added:
                // is scheduled for addition
                return "SVN中不存在该本地文件，但在本地已将此文件标记为需要上传至SVN";
            case SvnStatus.Missing:
                // under v.c., but is missing
                return "SVN中存在此文件，但在本地已将其删除";
            case SvnStatus.Deleted:
                // scheduled for deletion
                return "已在本地将此文件标记为要在SVN中删除";
            case SvnStatus.Modified:
                // text or props have been modified
                return "本地文件相较SVN中对应版本的内容已发生变动";
            case SvnStatus.Conflicted:
                // local mods received conflicting repos mods
                return "本地文件相较SVN中对应的文件发生变动并且无法合并已引发冲突";
            case SvnStatus.Ignored:
                // is unversioned but configured to be ignored
                return "此文件被设置为SVN忽略对其管理";
            case SvnStatus.Replaced:
                // was deleted and then re-added
                return "此文件被删除后重新添加";
            case SvnStatus.Merged:
                // local mods received repos mods
                return "此文件与SVN中最新版本文件内容不同但成功合并";
            // an unversioned resource is in the way of the versioned resource
            case SvnStatus.Obstructed:
            // an unversioned path populated by an svn:externals property
            case SvnStatus.External:
            // a directory doesn't contain a complete entries list
            case SvnStatus.Incomplete:
                return "SVN状态异常";
            default:
                return string.Empty;
        }
    }
}
