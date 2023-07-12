using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int initSunlightNum;
    public string curLevelName = "1-2";
    public GameObject[] bornParents;
    public GameObject[] zombiePrefab;

    private bool isStart = false;

    private int curSunlightNum;
    private List<GameObject> curZombieList = new List<GameObject>();
    private LevelData levelData;
    private int curProgress;
    private int curZombieIndex;
    private int zombieOrderIndex;
    public bool IsStart { get => isStart; }
    public int CurSunlightNum
    {
        get => curSunlightNum;
        set
        {
            curSunlightNum = value >= 0 ? value : 0;
            UIManager.instance.ChangeSunlightText(curSunlightNum);
        }
    }
    public int ProgressTot { get => levelData.ProgressNum; }
    public int CurItemTot { get => levelData[curProgress].ItemNum; }
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
        ResourceRequest request = Resources.LoadAsync("Level" + curLevelName);
        yield return request;
        levelData = request.asset as LevelData;
        // 完成读取，开始游戏
        Startgame();
    }

    void Startgame()
    {
        isStart = true;
        zombieOrderIndex = 0;
        curProgress = 0;
        CurSunlightNum = initSunlightNum;
        UIManager.instance.InitProgressBar();
        AudioManager.instance.PlayBGM(Globals.BGM1);
        AudioManager.instance.PlaySE(Globals.ZombieBornBegin);
        StartBornProgress();
    }

    // 生成单波僵尸
    void StartBornProgress()
    {
        for (curZombieIndex = 0; curZombieIndex < levelData[curProgress].ItemNum; curZombieIndex++)
            StartCoroutine(TableBornZombie());
    }

    IEnumerator TableBornZombie()
    {
        LevelItem curItem = levelData[curProgress][curZombieIndex];
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
        // 将新僵尸加入僵尸列表
        curZombieList.Add(newZombie);
    }

    // 僵尸死亡时调用以通知GameMagager
    public void ZombieDied(GameObject zombie)
    {
        curZombieList.Remove(zombie);
        UIManager.instance.ChangeProgress(curProgress, curZombieList.Count);
        if (curZombieList.Count <= 0)
        {
            if (curProgress >= levelData.ProgressNum - 1)
            {
                // TODO: 关卡胜利
                AudioManager.instance.StopBGM();
                AudioManager.instance.PlaySE(Globals.WinMusic);
                return;
            }
            // 刷新下一波僵尸
            curProgress++;
            // TODO: 一大波僵尸正在来袭
            if (curProgress+1 < ProgressTot)
                AudioManager.instance.PlaySE(Globals.HugeWave);
            else
                AudioManager.instance.PlaySE(Globals.FinalWave);
            StartBornProgress();
        }
    }

}
