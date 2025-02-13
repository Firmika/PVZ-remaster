using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCardPanel : MonoBehaviour
{
    public GameObject CardSlot;
    public GameObject CardsParent;
    public int CardSlotNum = 40;
    private List<CardData> cardDataList;
    public void InitCards()
    {
        cardDataList = GameManager.Instance.cardInfo.CardInfoList;
        // cards = new List<CardData>();
        // Debug.Log(cards.Count);
        for (int i = 0; i < 40; i++)
        {
            // 生成卡片槽
            GameObject newCardSlot = Instantiate(CardSlot);
            newCardSlot.transform.SetParent(CardsParent.transform);
            newCardSlot.name = "CardSlot" + i.ToString();
            // 生成卡片
            if (i < cardDataList.Count)
            {
                GameObject newCard = Instantiate(cardDataList[i].cardPrefab);
                newCard.transform.SetParent(newCardSlot.transform, false);
            }
        }
    }
}
