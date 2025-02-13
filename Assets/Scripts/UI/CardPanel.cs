using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardPanel : MonoBehaviour
{
    private List<GameObject> cards = new List<GameObject>();
    public List<GameObject> Cards { get => cards; }
    public int maxCount = 8;
    public int Count { get => cards.Count; }
    public bool IsFull { get => Count >= maxCount; }
    public void AddCard(GameObject card)
    {
        cards.Add(card);
    }
    public void RemoveCard(GameObject card)
    {
        cards.Remove(card);
        // 所有卡片调整位置
        GameObject cardSlots = transform.Find("Cards").gameObject;
        for (int i=0; i<cards.Count; i++)
        {
            GameObject c = cards[i];
            Vector3 oriPos = c.transform.position;
            c.transform.SetParent(cardSlots.transform.Find("CardSlot" + i),false);
            c.GetComponent<Card>().SetState(Card.CardState.Moving);
            c.transform.DOMove(oriPos, 0.5f).From().OnComplete(() => { c.GetComponent<Card>().SetState(Card.CardState.Chosen); });
        }
    }
    public void RenewPlayState() {
        foreach(GameObject card in cards)
            card.GetComponent<Card>().SetState(Card.CardState.Playing);
    }
    public GameObject this[int index]
    {
        get => cards[index];
    }
}
