using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleEditor 
{
	//自动打包工具

	//思路

	//1.找到资源保存的文件夹
	//2.遍历每个文件夹
	//3.如果访问的是文件夹，直到找到文件。
	//4.如果找到文件，修改问价的  assetbundle/lable
	//5.用AssetInporter 类 修改 名称 和 后缀。

	//6.保存对应的 文件夹名 和 AB包具体路径
	[MenuItem("Tools/Asset Bundle/Set AssetBundle Lable")]
	public  static void SetAssetBundleLabels()
	{
		Debug.Log ("Rubbish Company !");
		//移除所有没有使用的标记名字
		AssetDatabase.RemoveUnusedAssetBundleNames ();
		//1.找到资源保存的文件夹  
		//一个路径
		string assetDirectoryPath = @"E:/A徐学林/新建文件夹/AssetBundle/Assets/Res";
		DirectoryInfo directoryInfo = new DirectoryInfo (assetDirectoryPath);
		DirectoryInfo[] directoryArray = directoryInfo.GetDirectories ();

		//遍历每个文件夹
		foreach (DirectoryInfo item in directoryArray) 
		{
			string secondDirectoryPath = assetDirectoryPath + "/" + item.Name;

			DirectoryInfo secondDirectoryInfo = new DirectoryInfo (secondDirectoryPath);
			if (secondDirectoryInfo == null) {
				Debug.LogError ("错误，路径不存在");
				return;
			} else 
			{
 //                               文件夹名  ab包路径
                Dictionary<string, string> namePathDic = new Dictionary<string, string>();
//				int index = secondDirectoryPath.LastIndexOf ("/");
//				string fileName = secondDirectoryPath.Substring (index + 1);
				//Debug.Log (fileName + " : "+ item.Name + " : "+ secondDirectoryPath);
                //遍历文件夹
				OnChildFileSystemInfo (item, item.Name , namePathDic);

                //记录配置文件
                OnWriteConfig(item.Name, namePathDic);

            }
		}

        //刷新
        AssetDatabase.Refresh();
        Debug.Log(" Mark Success !!");
    }

	/// <summary>
	/// 遍历文件夹的 方法 递归
	/// </summary>
	/// <param name="_fileSystemInfo">File system info.</param>
	/// <param name="_fileName">File name.</param>
	private static void OnChildFileSystemInfo (FileSystemInfo _fileSystemInfo , string _secondFolderName , Dictionary<string,string> namePathDic)
	{
		if (!_fileSystemInfo.Exists) 
		{
			Debug.LogError (_fileSystemInfo.FullName + " 不存在");
			return;
		}

		DirectoryInfo directoryInfo = _fileSystemInfo as DirectoryInfo;
		FileSystemInfo[] fileChildArray = directoryInfo.GetFileSystemInfos (); //获取所有的， 包括文件夹和文件。

		foreach (FileSystemInfo item in fileChildArray) 
		{
			FileInfo fileInfo = item as FileInfo;
			if (fileInfo == null) 
			{
				// 代表强转失败 ， 不是文件 是文件夹
				//如果是文件夹 ，就继续遍历，知道找到文件。
				OnChildFileSystemInfo(item , _secondFolderName, namePathDic);
			}else
			{
				//设置 ab 路径与后缀 
				SetLabels(fileInfo , _secondFolderName,namePathDic);
			}
		}

       
	}

	/// <summary>
	/// 设置 修改 ab 路径与后缀 
	/// </summary>
	private static void  SetLabels(FileInfo fileInfo , string secondFolderName, Dictionary<string, string> namePathDic)
	{
		if (fileInfo.Extension == ".meta")
			return;

		string bundleName = GetBundleName (fileInfo, secondFolderName);

		int index = fileInfo.FullName.IndexOf ("Assets");
		string path = fileInfo.FullName.Substring (index);
		AssetImporter assetImporter = AssetImporter.GetAtPath (path); //此方法的路径是 从 Asset文件夹开始 的路径。

		assetImporter.assetBundleName = bundleName.ToLower();
		if (fileInfo.Extension == ".unity")
			assetImporter.assetBundleVariant = "u3d";
		else
			assetImporter.assetBundleVariant = "assetbundle";

        string _name = "";
        string _path = bundleName + "." + assetImporter.assetBundleVariant;
        //添加到字典里
        if (namePathDic.ContainsKey(_path)) return;
        if (bundleName.Contains("/"))
            _name = bundleName.Split('/')[1];
        else
            _name = secondFolderName;
        namePathDic.Add(_path, _name);
	}

	/// <summary>
	/// G获取包 名
	/// </summary>
	private static string GetBundleName(FileInfo fileInfo , string secondFolderName)
	{
		string windowPath = fileInfo.FullName;
		string unityPath = windowPath.Replace (@"\", "/");

//		E:/A徐学林/新建文件夹/AssetBundle/Assets/Res/Scene1    /Folder/Cube (2).prefab
//		截取后得到 ： /Folder/Cube (2).prefab
		int folderNameIndex = unityPath.IndexOf (secondFolderName) + secondFolderName.Length;
		string bundlePath = unityPath.Substring (folderNameIndex + 1);

		if(bundlePath.Contains("/"))
		{
			//Folder/Cube (2).prefab
			var arr = bundlePath.Split('/');
			return secondFolderName + "/" + arr [0];
		}else
		{
			//Cube (2).prefab
			return secondFolderName;
		}
		//return null;
	}

    /// <summary>
    /// 记录配置文件
    /// </summary>
    /// <param name="sceneDirectory">SceneName</param>
    /// <param name="namePathDic"></param>
    private static void OnWriteConfig(string sceneDirectory , Dictionary<string,string > namePathDic)
    {
        //"E:\A徐学林\新建文件夹\AssetBundle\Assets\AssetBundles\Scene1\Record.config".
        string path = Application.dataPath + "/AssetBundles/" + sceneDirectory + "Record.config";

        using (FileStream fs = new FileStream ( path , FileMode.OpenOrCreate , FileAccess.Write ))
        {
            using (StreamWriter sw = new StreamWriter(fs) )
            {
                sw.WriteLine(namePathDic.Count);

                foreach (KeyValuePair<string,string> kv in namePathDic)
                {
                    sw.WriteLine(kv.Value + "-" + kv.Key);
                }
            }
        }
    }
}

