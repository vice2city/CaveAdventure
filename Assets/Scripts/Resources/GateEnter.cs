using System;
using UnityEngine;

public class GateEnter : MonoBehaviour
{
    public GameObject destination;
    
    private GameObject player;
    private PlayerManager controller;
    
    private int destCaveID;
    private bool isReady;
    // Start is called before the first frame update
    private void Start()
    {
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
            controller.ShowKeyboardToast();
            isReady = true;
        }
        else
        {
            var info = GameManager.instance.caveInstance[destCaveID - 1].fireInfo;
            UIManager.instance.CreateToast("需要" + info);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        controller.ShutKeyboardToast();
        isReady = false;
    }
}
