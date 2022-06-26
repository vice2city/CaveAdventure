using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class CheckGateManager : MonoBehaviour
{
    public int checkGateID;

    private GameObject destination;
    private GameObject buttonBox;
    private bool isReady;
    private GameObject player;
    private PlayerManager controller;
    private int sourceGoal;
    // Start is called before the first frame update
    private void Start()
    {
        destination = transform.Find("Destination").gameObject;
        buttonBox = transform.Find("Canvas").Find("Button").gameObject;
        buttonBox.SetActive(false);
        isReady = false;
        player = GameManager.instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
        sourceGoal = 0;
        foreach (var source in GameObject.FindGameObjectsWithTag("Source"))
        {
            if (source.GetComponent<SourceManager>().caveId == checkGateID) sourceGoal++;
        }
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
        buttonBox.SetActive(true);
        isReady = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        buttonBox.SetActive(false);
        isReady = false;
    }

    private bool CheckAccessible()
    {
        var timeGoal = GameManager.instance.GetGoal(checkGateID);
        var surplus = controller.GetLightTime();
        if (timeGoal > surplus)
        {
            UIManager.instance.CreateToast("没有足够剩余的燃料");
            return false;
        }
        var count = GameManager.instance.CountUsedSource(checkGateID);
        if (sourceGoal <= count) return true;
        UIManager.instance.CreateToast("有剩余的燃料罐未打开");
        return false;
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
