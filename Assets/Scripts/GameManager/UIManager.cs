using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject sunlightNumText;
    void Start()
    {
        instance = this;
    }


    void Update()
    {
        sunlightNumText.GetComponent<Text>().text = GameManager.instance.initSunlightNum.ToString();
    }
}
