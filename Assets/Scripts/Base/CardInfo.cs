using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "PVZ/CardInfo", order = 1)]
public class CardInfo : ScriptableObject
{
    public List<CardData> CardInfoList = new List<CardData>();
    public CardData this[int index] {
        get => CardInfoList[index];
    }
}

[System.Serializable]
public class CardData 
{
    public int plantID;
    public string plantName;
    public string description;
    public GameObject cardPrefab;
    // public int sunlightAmount;
    // public int plantCD;
    // public GameObject plantPrefab;

    override public string ToString()
    {
        return "[id]: " + plantID.ToString();
    }
}
