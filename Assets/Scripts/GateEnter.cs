using System;
using UnityEngine;

public class GateEnter : MonoBehaviour
{
    public GameObject destination;
    
    private GameObject buttonBox;
    private GameObject tipBox;
    
    private GameObject player;
    private PlayerManager controller;
    
    private int destCaveID;
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

        var id = gameObject.name.Split('-')[1];
        destCaveID = id == null ? 0 : Convert.ToInt32(id);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isReady) return;
        if (!Input.GetKeyDown("e")) return;
        player.transform.position = destination.transform.position;
        controller.ChangePlayerState(destCaveID);
        isReady = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        if (GameManager.instance.IsCaveFinish(destCaveID-1 < 0 ? 0 : destCaveID-1))
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
