using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float HP = 100;
    // 植物所在行列号
    private int line = -1;
    private int column = -1;
    protected float curHP;
    protected Animator animator;
    // 植物所在行列号
    public int Line
    {
        get => line;
        set
        {
            if (line != -1)
            {
                Debug.LogWarning("无法移动已种下的植物");
                return;
            }
            line = value;
        }
    }

    public int Column
    {
        get => column;
        set
        {
            if (column != -1)
            {
                Debug.LogWarning("无法移动已种下的植物");
                return;
            }
            column = value;
        }
    }

    protected virtual void Start()
    {
        curHP = HP;
        animator = GetComponent<Animator>();
    }

    public virtual void GetDamage(float damage)
    {
        curHP -= damage;
        if (curHP <= 0)
        {
            curHP = 0;
            DestroyPlant();
        }
    }

    public virtual void DestroyPlant()
    {
        Destroy(this.gameObject);
    }

    public void SetPosition(int line, int column)
    {
        if (this.line != -1 || this.column != -1)
        {
            Debug.LogWarning("无法移动已种植下的植物");
            return;
        }
        this.line = line;
        this.column = column;
    }
}
