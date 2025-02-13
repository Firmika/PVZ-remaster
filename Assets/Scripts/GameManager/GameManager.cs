using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            return instance;
        }
    }
    public int initSunlightNum;
    public string dataPath = "TableData/";
    public string curLevelName = "1-2";
    public GameObject[] bornParents;
    public GameObject[] zombiePrefab;

    private bool isStart = false;

    private int curSunlightNum;
    private List<GameObject> curZombieList = new List<GameObject>();
    private LevelData levelData;
    public CardInfo cardInfo;
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
            UIManager.Instance.ChangeSunlightText(curSunlightNum);
        }
    }
    public int ProgressTot { get => levelData == null ? 0 : levelData.ProgressNum; }
    public int CurItemTot { get => levelData == null ? 0 : levelData[curProgress].ItemNum; }
    void Awake()
    {
        instance = this;
    }
    IEnumerator Start()
    {
        // 让unity读取完数据再初始化
        yield return new WaitForSeconds(1);
        StartLoadTable();
        // LoadTableAwait();
    }

    // private void LoadTableAwait()
    // {
    //     levelData = Resources.Load(dataPath + "Level" + curLevelName) as LevelData;
    //     cardInfo = Resources.Load(dataPath + "CardInfo") as CardInfo;
    //     GameReady();
    // }


    void StartLoadTable()
    {
        StartCoroutine(LoadTable());
    }

    IEnumerator LoadTable()
    {
        yield return new WaitForSeconds(2);

        // Debug.Log(dataPath + "CardInfo");
        // 读取关卡信息
        ResourceRequest request = Resources.LoadAsync(dataPath + "Level" + curLevelName);
        yield return request;
        levelData = request.asset as LevelData;

        // 读取卡片信息
        request = Resources.LoadAsync(dataPath + "CardInfo");
        yield return request;
        cardInfo = request.asset as CardInfo;
        // Debug.Log(cardInfo.CardInfoList.Count);

        // 完成读取，进入选卡阶段
        GameReady();
    }

    void GameReady()
    {
        CurSunlightNum = initSunlightNum;
        UIManager.Instance.InitUI();
    }
    public void LevelStart()
    {
        GetComponent<BornSunlight>().enabled = true;
        UIManager.Instance.InitPlayUI();
        zombieOrderIndex = 0;
        curProgress = 0;
        AudioManager.Instance.PlayBGM(Globals.BGM1);
        AudioManager.Instance.PlaySE(Globals.ZombieBornBegin);
        StartBornProgress();
    }

    // 生成单波僵尸
    void StartBornProgress()
    {
        // Debug.Log(levelData==null);
        if (levelData == null)
        {
            Debug.LogWarning("LevelData is null!");
            return;
        }
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
        newZombie.GetComponent<Zombie>().Line = index;
        // 后生成的僵尸处于上级
        newZombie.transform.GetComponent<SpriteRenderer>().sortingOrder = zombieOrderIndex++;
        // 将新僵尸加入僵尸列表
        curZombieList.Add(newZombie);
    }

    // 僵尸死亡时调用以通知GameMagager
    public void ZombieDied(GameObject zombie)
    {
        curZombieList.Remove(zombie);
        UIManager.Instance.ChangeProgress(curProgress, curZombieList.Count);
        if (curZombieList.Count <= 0)
        {
            if (curProgress >= levelData.ProgressNum - 1)
            {
                // TODO: 关卡胜利
                AudioManager.Instance.StopBGM();
                AudioManager.Instance.PlaySE(Globals.WinMusic);
                return;
            }
            // 刷新下一波僵尸
            curProgress++;
            // TODO: 一大波僵尸正在来袭
            AudioManager.Instance.PlaySE(Globals.HugeWave);
            if (curProgress + 1 == ProgressTot)
            {
                // TODO: 最后一波
            }
            StartBornProgress();
        }
    }

    public bool HasZombie(int line, Vector3 position, Direction direction = Direction.Right)
    {
        if (direction == Direction.Up || direction == Direction.Down)
            return false;
        foreach (GameObject zombie in curZombieList)
        {
            if (zombie.GetComponent<Zombie>().Line != line) continue;
            switch (direction)
            {
                case Direction.Left:
                    if (zombie.transform.position.x < position.x)
                        return true;
                    break;
                case Direction.Right:
                    if (zombie.transform.position.x > position.x)
                        return true;
                    break;
            }
        }
        return false;
    }

    public List<GameObject> GetZombies(int line)
    {
        List<GameObject> zombieLineList = new List<GameObject>();
        foreach (GameObject zombie in curZombieList)
        {
            if (zombie.GetComponent<Zombie>().Line == line)
                zombieLineList.Add(zombie);
        }
        return zombieLineList;
    }

    // 返回植物同行最接近的僵尸
    public GameObject GetClosestZombie(GameObject plant)
    {
        GameObject closestZombie = null;
        Plant plantComponent = plant.GetComponent<Plant>();
        foreach (GameObject zombie in GetZombies(plantComponent.Line))
        {
            if (closestZombie == null)
            {
                closestZombie = zombie;
                continue;
            }
            if (Vector2.Distance(plant.transform.position, zombie.transform.position)
                < Vector2.Distance(plant.transform.position, closestZombie.transform.position))
                closestZombie = zombie;
        }
        return closestZombie;
    }
}
