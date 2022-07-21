using System;
using UnityEngine;

public class CheckGateManager : MonoBehaviour
{
    private int caveId;
    private GameObject destination;
    
    private GameObject player;
    private PlayerManager controller;
    
    private bool isReady;
    private int sourceGoal;
    // Start is called before the first frame update
    private void Start()
    {
        destination = transform.Find("Destination").gameObject;
        var id = gameObject.name.Split(" ")[1];
        caveId = Convert.ToInt32(id);
        
        isReady = false;
        player = GameManager.instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
        sourceGoal = GameManager.instance.caveInstance[caveId].sourceGoal;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isReady) return;
        if (!Input.GetKeyDown("e")) return;
        if (CheckAccessible()) OpenGate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        controller.ShowKeyboardToast();
        isReady = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        controller.ShutKeyboardToast();
        isReady = false;
    }

    private bool CheckAccessible()
    {
        var timeGoal = GameManager.instance.caveInstance[caveId].caveGoal;
        var surplus = controller.GetLightTime();
        if (timeGoal > surplus)
        {
            UIManager.instance.UpdateGoalHandle(timeGoal);
            UIManager.instance.CreateToast("没有足够剩余的燃料", "你需要收集足够的燃料以打开洞穴");
            return false;
        }
        var count = GameManager.instance.CountUsedSource(caveId);
        if (sourceGoal > count)
        {
            UIManager.instance.CreateToast("有剩余的燃料罐未打开", "你需要开启洞穴内存在的所有燃料罐");
            return false;
        }
        UIManager.instance.UpdateGoalHandle(0f);
        return true;
    }

    private void OpenGate()
    {
        var initial = controller.initialLightTime;
        controller.ChangeLightTime(initial, PlayerManager.TimeChangeType.Replace);
        player.transform.position = destination.transform.position;
        isReady = false;
        controller.PauseLightTime(true);
    }
}
