using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jalapeno : Plant
{
    public GameObject attackEffect;
    public float effectX = -75;
    public float effcetYOffset = -20;
    protected override void Start()
    {
        base.Start();
    }

    public void BoomAniOver()
    {
        foreach (GameObject zombie in GameManager.Instance.GetZombies(Line))
            zombie.GetComponent<Zombie>().BoomDie();
        Burn();
        DestroyPlant();
    }

    private void Burn()
    {
        AudioManager.Instance.PlaySE(Globals.Jalapeno, 0.5f);
        GameObject fire = Instantiate(attackEffect, new Vector3(effectX, transform.position.y - effcetYOffset, 0),
                                        Quaternion.identity);
    }
}