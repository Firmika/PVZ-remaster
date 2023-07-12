using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : Plant
{
    public float attackInterval = 2f;
    public float damage = 10f;
    public GameObject bullet;
    public Transform bulletPosition;
    public float updateInterval = 0.5f;
    private bool isAttack = false;

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("UpdateAttackState", 0, updateInterval);
        InvokeRepeating("Attack", attackInterval, attackInterval);
    }

    void Attack()
    {
        if (!isAttack) return;
        AudioManager.instance.PlaySE(Globals.Shoot);
        Instantiate(bullet, bulletPosition.position, Quaternion.identity);
    }

    private void UpdateAttackState()
    {
        if (GameManager.instance.HasZombie(Line, transform.position))
        {
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }
    }
}
