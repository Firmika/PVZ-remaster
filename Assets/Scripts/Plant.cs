using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float HP = 100;
    protected float curHP;

    public void GetDamage(float damage)
    {
        curHP -= damage;
        if (curHP <= 0)
        {
            curHP = 0;
            Destroy(this.gameObject);
        }
    }
}