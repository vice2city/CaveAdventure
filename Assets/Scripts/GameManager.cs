using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        GetPlayer();
        if (instance != null)
        {
            instance.SceneIsLoaded();
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public Cave[] caveInstance;
    
    public bool[] caveState;           //对应洞穴的打开状态
    public bool[] skillState;
    private int spareNum;
    private List<Vector2Int> openedBox;
    private List<Vector2Int> unlockedDoor;     //已开锁的门id列表
    private List<Vector2Int> usedKey;          //使用过的钥匙id列表
    
    private List<int> usedSource;       //使用过的燃料罐id列表
    
    private bool isGamePause;
    private bool isGameEasy;
    
    private GameObject player;          //Player实例
    private PlayerManager controller;   //PlayerManager实例

    // Start is called before the first frame update
    private void Start()
    {
        spareNum = 0;
        openedBox = new List<Vector2Int>();
        unlockedDoor = new List<Vector2Int>();
        usedKey = new List<Vector2Int>();
        usedSource = new List<int>();
        isGamePause = false;
        isGameEasy = false;
        if (GameSettings.instance == null || !GameSettings.instance.isNeedLoadData) return;
        GameObject.Find("DataManager").GetComponent<DataManager>().LoadGameData();
        GameSettings.instance.isNeedLoadData = false;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateUI();
    }

    private void SceneIsLoaded()
    {
        usedSource = new List<int>();
    }

    //更新主界面UI
    private void UpdateUI()
    {
        if (!player) return;
        var keyCount = controller.GetKeyCount();
        UIManager.instance.UpdateKeyText(keyCount);
        var lightTime = controller.GetLightTime();
        UIManager.instance.UpdateLightTime(lightTime);
        UIManager.instance.UpdateSpareText(spareNum);
        UIManager.instance.UpdateSkillBoard(skillState);
    }

    //检查洞穴完成情况
    public bool IsCaveFinish(int n)
    {
        return n <= caveState.Length && caveState[n];
    }

    //完成洞穴
    public bool FinishCave(int n)
    {
        if (caveState[n]) return false;
        caveState[n] = true;
        return true;
    }

    //游戏回合结束
    public void GameOver()
    {
        isGamePause = true;
        UIManager.instance.OpenCover();
        UIManager.instance.OpenGameOverPanel();
    }

    public void ReloadScene()
    {
        isGamePause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public GameProcess GetGameData()
    {
        var process = new GameProcess
        {
            caveState = caveState,
            skillState = skillState,
            spareNum = spareNum,
            openedBox = openedBox,
            unlockedDoor = unlockedDoor,
            usedKey = usedKey,
            isGameEasy = isGameEasy
        };
        return process;
    }
    
    public void LoadGameData(GameProcess data)
    {
        caveState = data.caveState;
        skillState = data.skillState;
        spareNum = data.spareNum;
        openedBox = data.openedBox;
        unlockedDoor = data.unlockedDoor;
        usedKey = data.usedKey;
        isGameEasy = data.isGameEasy;
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void GamePause(bool b)
    {
        isGamePause = b;
    }

    public bool IsGamePause()
    {
        return isGamePause;
    }

    //获取Player实例
    public GameObject GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerManager>();
        return player;
    }

    //匹配钥匙
    public bool MatchKey(Vector2Int n)
    {
        return usedKey.Contains(n);
    }

    //匹配锁上的门
    public bool MatchLockedDoor(Vector2Int n)
    {
        return unlockedDoor.Contains(n);
    }

    //打开门锁
    public void OpenLockedDoor(Vector2Int keyId, Vector2Int doorId)
    {
        usedKey.Add(keyId);
        unlockedDoor.Add(doorId);
    }

    //获得燃料
    public void GetSource(int caveId)
    {
        usedSource.Add(caveId);
    }

    //获取打开的燃料罐数量
    public int CountUsedSource(int caveId)
    {
        return usedSource.Count(i => i==caveId);
    }

    public void OpenTreasureBox(int skillId, Vector2Int boxId)
    {
        openedBox.Add(boxId);
        if (skillId >= 0) skillState[skillId] = true;
        else spareNum++;
    }

    public bool IsBoxOpened(Vector2Int boxId)
    {
        return openedBox.Contains(boxId);
    }
    
    public bool IsSkillObtained(int skillId)
    {
        if (skillId < 0) return false;
        return skillState[skillId];
    }
    
    public bool UseSpare()
    {
        if (spareNum <= 0) return false;
        spareNum--;
        return true;
    }

    public bool GetGameDifficulty()
    {
        return isGameEasy;
    }
    
    public void ChangeGameDifficulty()
    {
        isGameEasy = !isGameEasy;
    }

}
