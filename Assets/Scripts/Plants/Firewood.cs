using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewood : Plant
{
    public GameObject firePea;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PeaBullet")
        {
            Vector3 pos = other.transform.position;
            Destroy(other.gameObject);
            Instantiate(firePea, pos, Quaternion.identity);
        }
    }
}