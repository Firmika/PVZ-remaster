using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "PVZ/LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public List<LevelProgress> levelDataList = new List<LevelProgress>();
    public int ProgressNum {get => levelDataList.Count;}
    public void AddProgress(LevelProgress progress) => levelDataList.Add(progress);
    public void AddProgress(List<LevelItem> progress) => levelDataList.Add(new LevelProgress(progress));
    public LevelProgress ProgressAt(int proIndex) => levelDataList[proIndex];
    public LevelItem ItemAt(int proIndex,int itemIndex) => ProgressAt(proIndex).ItemAt(itemIndex);
    public LevelProgress this[int index] => levelDataList[index];
}

[System.Serializable]
public class LevelProgress
{

    public List<LevelItem> items;
    public int ItemNum {get => items.Count;}
    public LevelProgress() => items = new List<LevelItem>();
    public LevelProgress(List<LevelItem> progress) => items = progress;
    public void AddItem(LevelItem item) => items.Add(item);
    public LevelItem ItemAt(int index) => items[index];
    public LevelItem this[int index] => items[index];
}

[System.Serializable]
public class LevelItem
{
    public int ID;
    public int progressID;
    public int createTime;
    public int zombieType;
    public int bornPos;
}