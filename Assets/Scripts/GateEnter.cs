using UnityEngine;
using UnityEngine.Serialization;

public class GateEnter : MonoBehaviour
{
    [FormerlySerializedAs("GateID")] public int gateID;
    public GameObject buttonBox;
    public GameObject tipBox;
    public GameObject destination;

    private GameObject player;
    private PlayerManager controller;
    private bool isReady;
    // Start is called before the first frame update
    void Start()
    {
        buttonBox.SetActive(false);
        tipBox.SetActive(false);
        isReady = false;
        player = GameManager.Instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (Input.GetKeyDown("e"))
            {
                player.transform.position = destination.transform.position;
                controller.ChangePlayerState(gateID);
                isReady = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerManager other = collision.GetComponent<PlayerManager>();
        if (other != null)
        {
           if (GameManager.Instance.IsGateOpen(gateID))
           {
               buttonBox.SetActive(true);
               isReady = true;
           }
           else
           {
               tipBox.SetActive(true);
           }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        PlayerManager other = collision.GetComponent<PlayerManager>();
        if (other != null)
        {
            buttonBox.SetActive(false);
            tipBox.SetActive(false);
            isReady = false;
        }
    }
}
