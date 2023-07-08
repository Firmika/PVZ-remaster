using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHead : MonoBehaviour
{
    public void LostHeadAniOver() {
        Destroy(this.gameObject);
    }
}
