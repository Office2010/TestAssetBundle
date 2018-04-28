using UnityEngine;
using System.IO;

public class PathUtil
{
    /// <summary>
    /// 获取 AssetBundle 的输出目录
    /// </summary>
    /// <returns></returns>
    public static string GetAssetBundleOutPath()
    {
        string outPath = GetPlatformPath() + "/" + GetPlatformName();
        if (!Directory.Exists(outPath))
            Directory.CreateDirectory(outPath);
        return outPath;
    }


    /// <summary>
    /// 获取 WWW 协议的路径
    /// </summary>
    public static string GetWwwPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                {
                    return "fttp:///" + GetAssetBundleOutPath();
                }
            case RuntimePlatform.IPhonePlayer:
                {
                    return "IPhone";
                }
            case RuntimePlatform.Android:
                {
                    return "jar:file://" + GetAssetBundleOutPath();
                }
            default: return null;
        }
    }



    /// <summary>
    /// 判断 当前平台
    /// 自动获取当前平台的路径
    /// </summary>
    /// <returns></returns>
    private static string GetPlatformPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                {
                    return Application.streamingAssetsPath;
                }
            case RuntimePlatform.IPhonePlayer:
                {
                    return "IPhone";
                }
            case RuntimePlatform.Android:
                {
                    return Application.persistentDataPath;
                }
            default:return null;
        }
    }


    /// <summary>
    /// 获取 对应平台的名字
    /// </summary>
    /// <returns></returns>
    private static string GetPlatformName()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                {
                    return "Windows";
                }
            case RuntimePlatform.IPhonePlayer:
                {
                    return "IPhone";
                }
            case RuntimePlatform.Android:
                {
                    return "Android";
                }
            default: return null;
        }
    }

 }

