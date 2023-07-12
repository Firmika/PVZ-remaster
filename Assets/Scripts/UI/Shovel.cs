using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shovel : MonoBehaviour
{
    
    private Vector3 oriPos;
    void Start()
    {
        oriPos = transform.position;
    }

    public void OnBeginDrag(BaseEventData data)
    {
        AudioManager.instance.PlaySE(Globals.Shovel);
        PointerEventData pointerEventData = data as PointerEventData;
        transform.position = pointerEventData.position;
    }

    public void OnDrag(BaseEventData data)
    {
        PointerEventData pointerEventData = data as PointerEventData;
        transform.position = pointerEventData.position;
    }

    public void OnEndDrag(BaseEventData data)
    {
        PointerEventData pointerEventData = data as PointerEventData;
        // 还原铲子位置
        transform.position = oriPos;
        Collider2D[] cols = Physics2D.OverlapPointAll(TranslateScreenToWorld(pointerEventData.position));
        foreach (Collider2D c in cols) {
            if ( c.tag=="Land" && c.transform.childCount>0 )
            {
                AudioManager.instance.PlaySE(Globals.Plant);
                // 销毁最后一个种植的植物
                Destroy(c.transform.GetChild(c.transform.childCount-1).gameObject);
            }
        }
    }

    private Vector3 TranslateScreenToWorld(Vector3 ScreenPos)
    {
        Vector3 translatePos = Camera.main.ScreenToWorldPoint(ScreenPos);
        return new Vector3(translatePos.x, translatePos.y, 0);
    }
}
