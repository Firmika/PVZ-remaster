using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int initSunlightNum;
    public GameObject[] bornParents;
    public GameObject[] zombiePrefab;
    // public float bornZombieInterval = 8;
    // public bool isBornZombie = true;
    protected bool isStart = false;
    protected List<GameObject> curZombieList = new List<GameObject>();
    protected LevelData levelData;
    private int curProgress;
    private int curZombieIndex;
    private int zombieOrderIndex;
    void Start()
    {
        instance = this;
        StartLoadTable();
    }

    void StartLoadTable()
    {
        StartCoroutine(LoadTable());
    }

    IEnumerator LoadTable()
    {
        ResourceRequest request = Resources.LoadAsync("Level1-1");
        yield return request;
        levelData = request.asset as LevelData;
        Startgame();
    }

    void Startgame()
    {
        isStart = true;
        zombieOrderIndex = 0;
        curProgress = 0;
        StartBornProgress();
    }

    void StartBornProgress()
    {
        for (curZombieIndex = 0; curZombieIndex < levelData.ProgressAt(curProgress).ItemNum; curZombieIndex++)
            StartCoroutine(TableBornZombie());
    }

    IEnumerator TableBornZombie()
    {
        LevelItem curItem = levelData.ItemAt(curProgress, curZombieIndex);
        // 在对应时间产生僵尸
        yield return new WaitForSeconds(curItem.createTime);
        // 僵尸在对应行号生成
        int index = curItem.bornPos == -1 ?
                    Random.Range(0, bornParents.Length) : curItem.bornPos;
        GameObject newZombie = Instantiate(zombiePrefab[curItem.zombieType]);
        newZombie.transform.parent = bornParents[index].transform;
        newZombie.transform.localPosition = Vector3.zero;
        // 后生成的僵尸处于上级
        newZombie.transform.GetComponent<SpriteRenderer>().sortingOrder = zombieOrderIndex++;
        // 加入僵尸列表
        curZombieList.Add(newZombie);
    }

    public void ZombieDied(GameObject zombie) {
        curZombieList.Remove(zombie);
        if ( curZombieList.Count<=0 ) {
            if ( curProgress>=levelData.ProgressNum-1 ) {
                // TODO: 关卡胜利
                return;
            }
            // 刷新下一波僵尸
            curProgress++;
            StartBornProgress();
        }
    }

    // // 定时生成僵尸
    // void StartTimerBornZomibe()
    // {
    //     isBornZombie = true;
    //     StartCoroutine(TimerBornZombie());
    // }
    // IEnumerator TimerBornZombie()
    // {
    //     yield return new WaitForSeconds(bornZombieInterval);
    //     int index = Random.Range(0, bornParents.Length);
    //     GameObject newZombie = Instantiate(zombiePrefab);
    //     newZombie.transform.parent = bornParents[index].transform;
    //     newZombie.transform.localPosition = Vector3.zero;
    //     if (isBornZombie) StartCoroutine(TimerBornZombie());
    // }
}
