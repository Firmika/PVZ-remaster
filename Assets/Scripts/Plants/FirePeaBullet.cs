using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePeaBullet : Bullet
{
    public GameObject destroyEffect;
    protected override void DestroyBullet()
    {
        Instantiate(destroyEffect,transform.position,Quaternion.identity);
        AudioManager.instance.PlaySE(Globals.FirePea);
        base.DestroyBullet();
    }
}
