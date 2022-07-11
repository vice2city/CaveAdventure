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
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public CaveManager[] caveInstance;
    
    public bool[] caveState;           //对应洞穴的打开状态
    public List<Vector2Int> unlockedDoor;     //已开锁的门id列表
    public List<Vector2Int> usedKey;          //使用过的钥匙id列表
    
    private List<int> usedSource;       //使用过的燃料罐id列表
    
    private GameObject player;          //Player实例
    private PlayerManager controller;   //PlayerManager实例

    // Start is called before the first frame update
    private void Start()
    {
        usedSource = new List<int>();
        if (GameSettings.instance == null || !GameSettings.instance.isNeedLoadData) return;
        GameObject.Find("DataManager").GetComponent<DataManager>().LoadGameData();
        GameSettings.instance.isNeedLoadData = false;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateUI();
    }

    //更新主界面UI
    private void UpdateUI()
    {
        if (!player) return;
        var keyCount = controller.GetKeyCount();
        UIManager.instance.UpdateKeyText(keyCount);
        var lightTime = controller.GetLightTime();
        UIManager.instance.UpdateLightTimeText(lightTime);
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
        UIManager.instance.OpenCover();
        UIManager.instance.OpenGameOverPanel();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    public void LoadGameData(GameProcess data)
    {
        caveState = data.caveState;
        unlockedDoor = data.unlockedDoor;
        usedKey = data.usedKey;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
