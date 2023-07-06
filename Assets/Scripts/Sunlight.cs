using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sunlight : MonoBehaviour
{
    public float destroyTime = 8f;
    public int sunlightAmout = 25;
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > destroyTime)
            Destroy(this.gameObject);
    }

    private void OnMouseDown() {
        GameManager.instance.sunlightNum += sunlightAmout;
        Destroy(this.gameObject);
    }

    private Vector3 TranslateScreenToWorld(Vector3 ScreenPos)
    {
        Vector3 translatePos = Camera.main.ScreenToWorldPoint(ScreenPos);
        return new Vector3(translatePos.x, translatePos.y, 0);
    }
}