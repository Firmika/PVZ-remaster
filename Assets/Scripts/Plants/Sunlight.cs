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
        AudioManager.Instance.PlaySE(Globals.Points);
        GameManager.Instance.CurSunlightNum += sunlightAmout;
        Destroy(this.gameObject);
    }
}