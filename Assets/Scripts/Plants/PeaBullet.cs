using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : Bullet
{
    protected override void DestroyBullet()
    {
        // Todo: 生成特效
        AudioManager.Instance.PlaySE(Globals.BulletHit);
        base.DestroyBullet();
    }
}