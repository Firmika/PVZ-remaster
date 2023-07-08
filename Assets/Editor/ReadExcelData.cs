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
    static StartUp()
    {
        // 从excel表格中读取关卡数据
        string path = Application.dataPath + "/Editor/LevelData_Zombie.xlsx";
        string assetName = "Level";
        FileInfo fileInfo = new FileInfo(path);
        LevelData levelData = (LevelData)ScriptableObject.CreateInstance(typeof(LevelData));
        levelData.levelName = "1-1";
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["1-1"];
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
        }
        AssetDatabase.CreateAsset(levelData, "Assets/Resources/" + assetName + levelData.levelName + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}