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

    public float[] checkGateGoal;
    public bool[] gateState;
    public bool[] caveState;

    private GameObject player;
    private PlayerManager controller;
    private List<int> unlockedDoor;
    private List<int> usedKey;
    private List<int> usedSource;
    
    // Start is called before the first frame update
    private void Start()
    {      
        unlockedDoor = new List<int>();
        usedKey = new List<int>();
        usedSource = new List<int>();
        UpdateAreaLight();
        GetPlayer();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateUI();
    }

    private void UpdateAreaLight()
    {
        foreach (var o in GameObject.FindGameObjectsWithTag("AreaLight"))
        {
            var m = o.GetComponent<AreaLightManager>();
            m.ChangeAreaLightState(caveState[m.caveId]);
        }
    }

    private void UpdateUI()
    {
        if (!player) return;
        var keyCount = controller.GetKeyCount();
        UIManager.instance.UpdateKeyText(keyCount);
        var lightTime = controller.GetLightTime();
        UIManager.instance.UpdateLightTimeText(lightTime);
    }

    public bool IsGateOpen(int n)
    {
        return gateState[n];
    }

    public bool IsCaveFinish(int n)
    {
        return caveState[n];
    }

    public bool OpenGate(int n)
    {
        if (gateState[n]) return false;
        gateState[n] = true;
        return true;
    }
    public bool FinishCave(int n)
    {
        if (caveState[n]) return false;
        caveState[n] = true;
        UpdateAreaLight();
        return true;
    }

    public void GameEnd()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public GameObject GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerManager>();
        return player;
    }

    public float GetGoal(int n)
    {
        return checkGateGoal[n];
    }

    public bool MatchKey(int n)
    {
        return usedKey.Contains(n);
    }

    public bool MatchLockedDoor(int n)
    {
        return unlockedDoor.Contains(n);
    }

    public void OpenLockedDoor(int keyId, int doorId)
    {
        usedKey.Add(keyId);
        unlockedDoor.Add(doorId);
    }

    public void GetSource(int caveId)
    {
        usedSource.Add(caveId);
    }

    public int CountUsedSource(int caveId)
    {
        return usedSource.Count(i => i==caveId);
    }
}
