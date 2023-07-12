using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private GameObject sunlightNumText;
    private GameObject progressPanel;

    void Start()
    {
        instance = this;
        sunlightNumText = GameObject.Find("SunlightNumText");
        progressPanel = GameObject.Find("ProgressPanel");
    }

    public void InitProgressBar()
    {
        progressPanel.GetComponent<ProgressPanel>().InitProgressBar();
    }

    public void ChangeProgress(int curProgress, int zombieRemain)
    {
        if (curProgress == GameManager.instance.ProgressTot) return;
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
}
