using UnityEngine;

public class CheckGateManager : MonoBehaviour
{
    public GameObject destination;
    public int checkGateID;
    public GameObject buttonBox;

    private bool isReady;
    private GameObject player;
    private PlayerManager controller;
    private int sourceGoal;
    // Start is called before the first frame update
    void Start()
    {
        buttonBox.SetActive(false);
        isReady = false;
        player = GameManager.Instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
        sourceGoal = 0;
        foreach (var source in GameObject.FindGameObjectsWithTag("Source"))
        {
            if (source.GetComponent<SourceManager>().caveId == checkGateID) sourceGoal++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (Input.GetKeyDown("e"))
            {
                if (CheckAccessible()) OpenGate();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerManager other = collision.GetComponent<PlayerManager>();

        if (other != null)
        {
            buttonBox.SetActive(true);
            isReady = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        PlayerManager other = collision.GetComponent<PlayerManager>();
        if (other != null)
        {
            buttonBox.SetActive(false);
            isReady = false;
        }
    }

    private bool CheckAccessible()
    {
        var timeGoal = GameManager.Instance.GetGoal(checkGateID);
        var surplus = controller.GetLightTime();
        if (timeGoal > surplus)
        {
            UIManager.instance.createToast("没有足够剩余的燃料");
            return false;
        }
        var count = GameManager.Instance.CountUsedSource(checkGateID);
        if (sourceGoal > count)
        {
            UIManager.instance.createToast("有剩余的燃料罐未打开");
            return false;
        }
        return true;
    }

    private void OpenGate()
    {
        float initial = controller.initialLightTime;
        controller.ChangeLightTime(initial, PlayerManager.TimeChangeType.Replace);
        player.transform.position = destination.transform.position;
        isReady = false;
        controller.PauseLightTime(true);
    }
}
