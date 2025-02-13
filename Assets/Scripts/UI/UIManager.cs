
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(UIManager)) as UIManager;
            return instance;
        }
    }
    private GameObject sunlightNumText;
    private GameObject progressPanel;
    private GameObject mainCardPanel;
    public GameObject MainCardPanel { get => mainCardPanel; }
    private GameObject mainCardSlot;
    public GameObject MainCardSlot { get => mainCardSlot; }
    private GameObject cardPanel;
    public GameObject CardPanel { get => cardPanel; }
    private GameObject cardPanelSlot;
    public GameObject CardPanelSlot { get => cardPanelSlot; }

    private UIManager()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        // 让unity读取完数据再初始化
        yield return new WaitForSeconds(0.8f);
        sunlightNumText = GameObject.Find("SunlightNumText");
        progressPanel = GameObject.Find("ProgressPanel");
        mainCardPanel = GameObject.Find("MainCardPanel");
        mainCardSlot = GameObject.Find("MainCards").gameObject;
        cardPanel = GameObject.Find("CardPanel");
        cardPanelSlot = cardPanel.transform.Find("Cards").gameObject;
    }

    public void InitUI()
    {

        InitMainCardPanel();

    }
    public void InitPlayUI()
    {
        InitProgressBar();
        InitPlayCardPanel();
    }
    public void FinishChoose()
    {
        mainCardPanel.transform.DOMove(new Vector3(0, -550, 0), 1).SetRelative().OnComplete(
            () =>
            {
                Destroy(mainCardPanel);
                // 画面转移
                // Todo...
                // 准备...安放...植物！
                // Todo...
                // 触发游戏开始
                GameManager.Instance.LevelStart();
            }
        );

    }
    private void InitProgressBar()
    {
        progressPanel.GetComponent<ProgressPanel>().InitProgressBar();
    }

    private void InitMainCardPanel()
    {
        mainCardPanel.GetComponent<MainCardPanel>().InitCards();
        mainCardPanel.transform.DOMove(new Vector3(0, -550, 0), 1).From(true);
    }
    private void InitPlayCardPanel()
    {
        cardPanel.GetComponent<CardPanel>().RenewPlayState();
    }

    public void ChangeProgress(int curProgress, int zombieRemain)
    {
        if (curProgress == GameManager.Instance.ProgressTot - 1)
        {
            curProgress = GameManager.Instance.ProgressTot - 2;
            zombieRemain = 0;
        }
        progressPanel.GetComponent<ProgressPanel>().ChangeProgress(curProgress, zombieRemain);
    }

    public void ChangeSunlightText(int num)
    {
        sunlightNumText.GetComponent<Text>().text = num.ToString();
    }
    public void ChangeSunlightText(string text)
    {
        sunlightNumText.GetComponent<Text>().text = text;
    }
    public CardData GetCardData(int index) => GameManager.Instance.cardInfo[index];
    public GameObject GetCardPrefab(int index) => GameManager.Instance.cardInfo[index].cardPrefab;
}
