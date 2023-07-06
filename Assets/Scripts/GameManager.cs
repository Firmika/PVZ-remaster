using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int sunlightNum;
    public GameObject[] bornParents;
    public GameObject zombiePrefab;
    public float bornZombieInterval = 8;
    public bool isBornZombie = true;
    void Start()
    {
        instance = this;
        StartBornZomibe();
    }

    void StartBornZomibe()
    {
        isBornZombie = true;
        StartCoroutine(BornZombie());
    }

    IEnumerator BornZombie()
    {
        yield return new WaitForSeconds(bornZombieInterval);
        int index = Random.Range(0, bornParents.Length);
        GameObject newZombie = Instantiate(zombiePrefab);
        newZombie.transform.parent = bornParents[index].transform;
        newZombie.transform.localPosition = Vector3.zero;
        if (isBornZombie) StartCoroutine(BornZombie());
    }
}
