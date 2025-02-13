using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public enum CardState
    {
        // 可选
        Choosable,
        // 已选（在卡片栏中）
        Chosen,
        // 虚假（用作垫底）
        Virtual,
        // 游玩中
        Playing,
        // 移动中
        Moving,
    };
    public int cardID;
    private CardState state;
    public CardState State
    {
        get => state;
        private set
        {
            switch (value)
            {
                // 可选状态
                case CardState.Choosable:
                    state = CardState.Choosable;
                    isReady = false;
                    darkBg.SetActive(false);
                    progressBg.SetActive(false);
                    timer = 0;
                    break;
                // 选中但游戏未开始
                case CardState.Chosen:
                    state = CardState.Chosen;
                    isReady = false;
                    darkBg.SetActive(false);
                    progressBg.SetActive(false);
                    timer = 0;
                    break;
                // 虚拟卡片
                case CardState.Virtual:
                    state = CardState.Virtual;
                    isReady = false;
                    darkBg.SetActive(true);
                    progressBg.SetActive(true);
                    timer = 0;
                    break;
                // 游戏进行
                case CardState.Playing:
                    state = CardState.Playing;
                    isReady = true;
                    darkBg.SetActive(true);
                    progressBg.SetActive(true);
                    timer = 0;
                    UpdateProgress();
                    UpdateDarkBg();
                    break;
                // 卡片移动
                case CardState.Moving:
                    state = CardState.Moving;
                    isReady = false;
                    darkBg.SetActive(false);
                    progressBg.SetActive(false);
                    timer = 0;
                    break;
            }
        }
    }
    public void SetState(CardState state)
    {
        State = state;
    }
    public int sunlightAmout;
    public float duration;
    public CardData cardData;
    public GameObject plantPrefab;
    public GameObject staticPlantPrefab;
    private GameObject virtualDragPlant;
    private bool isReady;
    private float timer;
    private GameObject darkBg;
    private GameObject progressBg;
    private GameObject cardPrefab;

    private void Awake()
    {
        cardData = UIManager.Instance.GetCardData(cardID);
        cardPrefab = cardData.cardPrefab;
        darkBg = transform.Find("Dark").gameObject;
        progressBg = transform.Find("Progress").gameObject;
        State = CardState.Choosable;
    }

    void Update()
    {
        if (State != CardState.Playing)
            return;
        // 更新压黑进度条
        if (!isReady)
            timer += Time.deltaTime;
        UpdateProgress();
        UpdateDarkBg();
    }

    private void UpdateDarkBg()
    {
        if (isReady && GameManager.Instance.CurSunlightNum >= sunlightAmout)
            darkBg.SetActive(false);
        else
            darkBg.SetActive(true);
    }

    private void UpdateProgress()
    {
        if (timer >= duration)
            isReady = true;
        if (isReady)
            progressBg.GetComponent<Image>().fillAmount = 0;
        else
            progressBg.GetComponent<Image>().fillAmount = 1 - timer / duration;
    }

    public void OnBeginDrag(BaseEventData data)
    {
        // 仅在游戏进行中启用拖拽种植
        if (State != CardState.Playing)
            return;
        if (!IsPlantable())
        {
            AudioManager.Instance.PlaySE(Globals.SunlightLack);
            return;
        }
        AudioManager.Instance.PlaySE(Globals.SeedLift);
        PointerEventData pointerEventData = data as PointerEventData;
        virtualDragPlant = Instantiate(staticPlantPrefab);
        virtualDragPlant.transform.position = TranslateScreenToWorld(pointerEventData.position);
    }

    public void OnDrag(BaseEventData data)
    {
        // 仅在游戏进行中启用拖拽种植
        if (State != CardState.Playing)
            return;
        if (virtualDragPlant == null) return;
        PointerEventData pointerEventData = data as PointerEventData;
        virtualDragPlant.transform.position = TranslateScreenToWorld(pointerEventData.position);
    }

    public void OnEndDrag(BaseEventData data)
    {
        // 仅在游戏进行中启用拖拽种植
        if (State != CardState.Playing)
            return;
        if (virtualDragPlant == null) return;
        // Destroy virtualPlant
        PointerEventData pointerEventData = data as PointerEventData;
        Destroy(virtualDragPlant);
        virtualDragPlant = null;

        // Plant a new plant if available
        Collider2D[] cols = Physics2D.OverlapPointAll(TranslateScreenToWorld(pointerEventData.position));
        foreach (Collider2D c in cols)
        {
            if (c.tag == "Land" && c.transform.childCount == 0)
            {
                PlantNewPlant(c.transform);
            }
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // Debug.Log("点击了卡片");
        switch (State)
        {
            // 选中卡片
            case CardState.Choosable:
                Choose();
                break;
            // 取消选中卡片
            case CardState.Chosen:
                UndoChoose();
                break;
            default:
                return;
        }
    }
    private GameObject virtualCard;
    private void Choose()
    {
        CardPanel cardPanel = UIManager.Instance.CardPanel.GetComponent<CardPanel>();
        if (cardPanel.IsFull)
            return;
        virtualCard = Instantiate(cardPrefab, transform.position, Quaternion.identity, transform.parent);
        virtualCard.GetComponent<Card>().State = CardState.Virtual;
        int curIndex = UIManager.Instance.CardPanel.GetComponent<CardPanel>().Count;
        GameObject parentSlot = UIManager.Instance.CardPanelSlot.transform.Find("CardSlot" + curIndex).gameObject;
        transform.SetParent(parentSlot.transform, false);
        State = CardState.Moving;
        // 移动结束时改变状态
        transform.DOMove(virtualCard.transform.position, 0.5f).From()
                 .OnComplete(() => { State = CardState.Chosen; });
        // 在CardPanel列表中添加植物
        cardPanel.AddCard(this.gameObject);
    }
    private void UndoChoose()
    {
        CardPanel cardPanel = UIManager.Instance.CardPanel.GetComponent<CardPanel>();
        State = CardState.Moving;
        // 通知面板删除卡片
        cardPanel.RemoveCard(this.gameObject);
        // 卡片移动
        transform.DOMove(virtualCard.transform.position, 0.5f)
                .OnComplete(() =>
                {
                    State = CardState.Choosable;
                    transform.SetParent(virtualCard.transform.parent);
                    DestroyVirtual();
                });

    }
    private void DestroyVirtual()
    {
        if (virtualCard == null)
        {
            Debug.LogWarning("虚拟卡片不存在！");
            return;
        }
        Destroy(virtualCard);
    }

    private void PlantNewPlant(Transform parent)
    {
        // 播放音效
        AudioManager.Instance.PlaySE(Globals.Plant);

        // 实例化预制体
        GameObject newPlantObject = Instantiate(plantPrefab);
        newPlantObject.transform.parent = parent;
        newPlantObject.transform.localPosition = Vector3.zero;

        // 更新植物行列号
        Plant newPlant = newPlantObject.GetComponent<Plant>();
        Land parentLand = parent.GetComponent<Land>();
        newPlant.SetPosition(parentLand.GetLine(), parentLand.GetColumn());
        // Debug.Log(newPlant.line + " " + newPlant.column);

        // 扣除阳光数量并更新卡片状态
        GameManager.Instance.CurSunlightNum -= sunlightAmout;
        isReady = false;
        timer = 0;
    }

    private bool IsPlantable()
    {
        return sunlightAmout <= GameManager.Instance.CurSunlightNum && isReady;
    }

    private Vector3 TranslateScreenToWorld(Vector3 ScreenPos)
    {
        Vector3 translatePos = Camera.main.ScreenToWorldPoint(ScreenPos);
        return new Vector3(translatePos.x, translatePos.y, 0);
    }
}