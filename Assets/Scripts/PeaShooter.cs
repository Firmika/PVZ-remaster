using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : Plant
{
    public float attackInterval = 2f;
    public float damage = 10f;
    public GameObject bullet;
    public Transform bulletPosition;
    private float timer=0;
    void Start() {
        curHP = HP;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if ( timer>=attackInterval ) {
            Instantiate(bullet, bulletPosition.position,Quaternion.identity);
            timer = 0;
        }
    }
}
