using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sunlight : MonoBehaviour
{
    public float destroyTime = 8f;
    public int sunlightAmout = 25;

    void Start()
    {
        StartCoroutine(DestroySunlight());
    }

    IEnumerator DestroySunlight() 
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

    private void OnMouseDown() {
        AudioManager.instance.PlaySE(Globals.Points);
        GameManager.instance.CurSunlightNum += sunlightAmout;
        Destroy(this.gameObject);
    }

    private Vector3 TranslateScreenToWorld(Vector3 ScreenPos)
    {
        Vector3 translatePos = Camera.main.ScreenToWorldPoint(ScreenPos);
        return new Vector3(translatePos.x, translatePos.y, 0);
    }
}