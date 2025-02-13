using UnityEngine;
using UnityEditor;
using OfficeOpenXml;
using System.IO;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

[InitializeOnLoad]

public class StartUp
{
    static string path = Application.dataPath + "/Editor/LevelData_Zombie.xlsx";
    static string objPath = "Assets/Resources/TableData/";
    static string assetName = "Level";
    static string[] levelNames = { "1-1", "1-2" };
    static FileInfo fileInfo;
    static ExcelPackage excelPackage;
    static StartUp()
    {
        // 从excel表格中读取关卡数据

        fileInfo = new FileInfo(path);

        using (excelPackage = new ExcelPackage(fileInfo))
        {
            foreach (string levelName in levelNames)
                ReadTableLevel(levelName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    static void ReadTableLevel(string levelName)
    {
        LevelData levelData = (LevelData)ScriptableObject.CreateInstance(typeof(LevelData));
        levelData.levelName = levelName;
        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[levelName];
        int curProgressNum = 0;
        LevelProgress curProgress = null;
        for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
        {
            LevelItem levelItem = new LevelItem();
            Type type = typeof(LevelItem);
            for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
            {
                string tableValue = worksheet.GetValue(i, j).ToString();
                if (j == worksheet.Dimension.Start.Column + 1 && curProgressNum.ToString() != tableValue)
                {
                    curProgressNum++;
                    curProgress = new LevelProgress();
                    levelData.AddProgress(curProgress);
                }
                FieldInfo variable = type.GetField(worksheet.GetValue(2, j).ToString());

                variable.SetValue(levelItem, Convert.ChangeType(tableValue, variable.FieldType));
            }
            curProgress.AddItem(levelItem);
        }
        AssetDatabase.CreateAsset(levelData, objPath + assetName + levelName + ".asset");
    }
}