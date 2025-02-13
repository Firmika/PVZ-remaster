using System.Collections;
using System.Collections.Generic;
// Application.persistentDataPath路径配置
using UnityEngine;
// Json序列化与反序列化
using Newtonsoft.Json;
// 文件读写
using System.IO;
using System;

public class UserData
{
    public int UID { get; set; }
    public string Name { get; set; }
    public string Level { get; set; }

    protected static Dictionary<int, UserData> bufUserData = new();

    static public void SaveUserData(UserData userData)
    {
        string dir = Application.persistentDataPath + "/users";
        // 判断文件夹是否存在
        if (!Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
            Debug.Log("User目录创建");
        }

        string jsonData = JsonConvert.SerializeObject(userData);
        File.WriteAllText(dir + string.Format("/{0}.json", userData.UID), jsonData);
        // 保存至缓存中
        bufUserData[userData.UID] = userData;
    }
    static public UserData ReadUserData(int UID)
    {
        string dir = Application.persistentDataPath + "/users";
        // 尝试从缓存中读取
        if (bufUserData.ContainsKey(UID))
        {
            return bufUserData[UID];
        }
        // 判定文件存在性
        if (!Directory.Exists(dir) || !File.Exists(dir + string.Format("/{0}.json", UID)))
        {
            Debug.LogWarning("用户不存在!");
            return null;
        }
        // 将Json转换为数据
        string jsonData = File.ReadAllText(dir + string.Format("/{0}.json", UID));
        UserData userData = JsonConvert.DeserializeObject<UserData>(jsonData);
        // 保存至缓存
        bufUserData[userData.UID] = userData;
        return userData;
    }
}