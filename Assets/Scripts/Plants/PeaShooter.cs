using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : Plant
{
    public float attackInterval = 2f;
    public float damage = 10f;
    public GameObject bullet;
    public Transform bulletPosition;
    private float timer = 0;
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            Attack();
            timer = 0;
        }
    }

    void Attack() {
        AudioManager.instance.PlaySE(Globals.Shoot);
        Instantiate(bullet, bulletPosition.position, Quaternion.identity);
    }
}
