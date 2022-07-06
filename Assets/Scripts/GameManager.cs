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
        _areaLights = GameObject.FindGameObjectsWithTag("AreaLight");
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public float[] checkGateGoal;       //打开检查门需要的燃料量
    public bool[] caveState;            //对应洞穴的打开状态
    
    private GameObject player;          //Player实例
    private PlayerManager controller;   //PlayerManager实例
    private List<int> unlockedDoor;     //上锁的门id列表
    private List<int> usedKey;          //使用过的钥匙id列表
    private List<int> usedSource;       //使用过的燃料罐id列表
    private static GameObject[] _areaLights;    //区域灯光实例列表

    // Start is called before the first frame update
    private void Start()
    {      
        unlockedDoor = new List<int>();
        usedKey = new List<int>();
        usedSource = new List<int>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateUI();
        UpdateAreaLight(); 
    }

    //更新区域灯光状态
    private void UpdateAreaLight()
    {
        foreach (var o in _areaLights)
        {
            var id = o.name.Split(" ")[1];
            var caveId = id == null ? 0 : Convert.ToInt32(id); 
            o.SetActive(caveState[caveId]);
        }
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
        return caveState[n];
    }

    //完成洞穴
    public bool FinishCave(int n)
    {
        if (caveState[n]) return false;
        caveState[n] = true;
        return true;
    }

    //游戏回合结束
    public void GameEnd()
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

    //获取检查门开启条件
    public float GetGoal(int n)
    {
        return checkGateGoal[n];
    }

    //匹配钥匙
    public bool MatchKey(int n)
    {
        return usedKey.Contains(n);
    }

    //匹配锁上的门
    public bool MatchLockedDoor(int n)
    {
        return unlockedDoor.Contains(n);
    }

    //打开门锁
    public void OpenLockedDoor(int keyId, int doorId)
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
}
