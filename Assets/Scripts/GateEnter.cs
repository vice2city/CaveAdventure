using UnityEngine;
using UnityEngine.Serialization;

public class GateEnter : MonoBehaviour
{
    public int gateID;
    public GameObject destination;
    
    private GameObject buttonBox;
    private GameObject tipBox;
    
    private GameObject player;
    private PlayerManager controller;
    private bool isReady;
    // Start is called before the first frame update
    private void Start()
    {
        buttonBox = transform.Find("Canvas").Find("Button").gameObject;
        tipBox = transform.Find("Canvas").Find("Tip").gameObject;
        buttonBox.SetActive(false);
        tipBox.SetActive(false);
        isReady = false;
        player = GameManager.instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isReady) return;
        if (!Input.GetKeyDown("e")) return;
        player.transform.position = destination.transform.position;
        controller.ChangePlayerState(gateID);
        isReady = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        if (GameManager.instance.IsGateOpen(gateID))
        {
            buttonBox.SetActive(true);
            isReady = true;
        }
        else
        {
            tipBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        buttonBox.SetActive(false);
        tipBox.SetActive(false);
        isReady = false;
    }
}
