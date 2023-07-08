using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNut : Plant
{
    protected override void Start()
    {
        base.Start();
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        animator.SetFloat("curHP", curHP/HP);
    }
}