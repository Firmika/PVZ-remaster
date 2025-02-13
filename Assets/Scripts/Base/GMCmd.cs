using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq.Expressions;
using System.IO;

public class GMCmd
{
    [MenuItem("GMCmd/SaveTestUserData")]
    public static void SaveTestUserData()
    {
        for (int i = 0; i < 5; i++)
        {
            UserData user = new()
            {
                UID = i,
                Name = "user" + i.ToString(),
                Level = "1-1",
            };
            UserData.SaveUserData(user);
        }
        Debug.Log("Saved Test Data");
    }
    [MenuItem("GMCmd/ReadTestUserData")]
    public static void ReadTestUserData()
    {
        string dir = Application.persistentDataPath + "/users";
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(UserData.ReadUserData(i));
        }
    }

}
