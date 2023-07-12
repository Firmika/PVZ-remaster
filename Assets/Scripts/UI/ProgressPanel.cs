using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanel : MonoBehaviour
{
    // 进度条fillbar初始位置相对右侧锚点的偏移量
    public float progressOffset = 12;
    public float raiseFlagOffset = 5;
    private GameObject progressBg;
    private GameObject progressPointer;
    private GameObject fillBar;
    // public GameObject flagPrefab;
    private GameObject flagPrefab;
    private List<GameObject> progressFlags = new List<GameObject>();
    private int progressNum;
    private float fillBarLength;

    void Start()
    {
        progressBg = GameObject.Find("ProgressBg");
        progressPointer = GameObject.Find("ProgressPointer");
        fillBar = GameObject.Find("ProgressFillBar");
        flagPrefab = Resources.Load("Prefab/ProgressFlag") as GameObject;
    }

    // 加载完成后初始化进度条
    public void InitProgressBar()
    {
        progressNum = GameManager.instance.ProgressTot;
        // progressNum = 3;
        fillBarLength = progressBg.GetComponent<RectTransform>().sizeDelta.x - 2 * progressOffset;
        InitProgressFlag();
        progressPointer.transform.SetAsLastSibling();
        ChangeFillBar(0);
        progressPointer.GetComponent<RectTransform>().anchoredPosition = new Vector3(-progressOffset, 0, 0);
    }

    // 生成初始进度条旗子
    private void InitProgressFlag()
    {
        float perLength = fillBarLength / (progressNum - 1);
        for (int i = 1; i < progressNum; i++)
        {
            GameObject newFlag = Instantiate(flagPrefab);
            newFlag.transform.SetParent(progressBg.transform);
            progressFlags.Add(newFlag);
            newFlag.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-progressOffset - perLength * i, 0, 0);
        }
    }

    public void ChangeProgress(int curProgress, int zombieRemain)
    {
        float prPortion = 1f / (GameManager.instance.ProgressTot - 1);
        float percent = prPortion * curProgress + (1 - (float)zombieRemain / GameManager.instance.CurItemTot) * prPortion;
        RaiseFlag(curProgress);
        ChangeFillBar(percent);
        ChangeProgressPointer(curProgress, zombieRemain);
    }

    // 举起指定数量的旗子
    private void RaiseFlag(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (progressFlags[i].GetComponent<RectTransform>().localPosition.y > 0) continue;
            progressFlags[i].GetComponent<RectTransform>().position += new Vector3(0, raiseFlagOffset, 0);
        }
    }

    private void ChangeFillBar(float percent)
    {
        percent = percent > 1 ? 1 : percent;
        fillBar.GetComponent<Image>().fillAmount = percent;
    }

    private void ChangeProgressPointer(int curProgress, int zombieRemain)
    {
        if (curProgress == GameManager.instance.ProgressTot)
        {
            curProgress -= 1;
            zombieRemain = 0;
        }
        float perLength = fillBarLength / (progressNum - 1);
        float x = -progressOffset - perLength * curProgress
                  - perLength * (1 - (float)zombieRemain / GameManager.instance.CurItemTot);
        progressPointer.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(x, 0, 0);
    }
}