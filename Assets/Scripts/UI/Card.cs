using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    public int sunlightAmout;
    public float duration;
    public GameObject plantPrefab;
    public GameObject staticPlantPrefab;
    private GameObject virtualDragPlant;
    private bool isReady;
    private float timer;
    private GameObject darkBg;
    private GameObject progressBg;

    void Start()
    {
        isReady = true;
        timer = 0;
        darkBg = transform.Find("Dark").gameObject;
        progressBg = transform.Find("Progress").gameObject;
    }
    void Update()
    {
        if (!isReady)
            timer += Time.deltaTime;
        UpdateProgress();
        UpdateDarkBg();
    }

    private void UpdateDarkBg()
    {
        if (isReady && GameManager.instance.CurSunlightNum >= sunlightAmout)
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
        if (!IsPlantable()) {
            AudioManager.instance.PlaySE(Globals.SunlightLack);
            return;
        }
        AudioManager.instance.PlaySE(Globals.SeedLift);
        PointerEventData pointerEventData = data as PointerEventData;
        virtualDragPlant = Instantiate(staticPlantPrefab);
        virtualDragPlant.transform.position = TranslateScreenToWorld(pointerEventData.position);
    }

    public void OnDrag(BaseEventData data)
    {
        if (virtualDragPlant == null) return;
        PointerEventData pointerEventData = data as PointerEventData;
        virtualDragPlant.transform.position = TranslateScreenToWorld(pointerEventData.position);
    }

    public void OnEndDrag(BaseEventData data)
    {
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

    private void PlantNewPlant(Transform parent)
    {
        // 播放音效
        AudioManager.instance.PlaySE(Globals.Plant);

        // 实例化预制体
        GameObject newPlantObject = Instantiate(plantPrefab);
        newPlantObject.transform.parent = parent;
        newPlantObject.transform.localPosition = Vector3.zero;
        
        // 更新植物行列号
        Plant newPlant = newPlantObject.GetComponent<Plant>();
        Land parentLand = parent.GetComponent<Land>();
        newPlant.SetPosition(parentLand.GetLine(),parentLand.GetColumn());
        // Debug.Log(newPlant.line + " " + newPlant.column);

        // 扣除阳光数量并更新卡片状态
        GameManager.instance.CurSunlightNum -= sunlightAmout;
        isReady = false;
        timer = 0;
    }

    private bool IsPlantable()
    {
        return sunlightAmout <= GameManager.instance.CurSunlightNum && isReady;
    }

    private Vector3 TranslateScreenToWorld(Vector3 ScreenPos)
    {
        Vector3 translatePos = Camera.main.ScreenToWorldPoint(ScreenPos);
        return new Vector3(translatePos.x, translatePos.y, 0);
    }
}