using System;
using UnityEngine;

public class LockedDoorManager : MonoBehaviour
{
    private GameObject player;
    private PlayerManager controller;

    private Vector2Int doorId;
    private bool isReady;
    // Start is called before the first frame update
    private void Start()
    {
        var id = gameObject.name.Split(" ")[1].Split("-");
        doorId.x = Convert.ToInt32(id[0]);
        doorId.y = Convert.ToInt32(id[1]);

        player = GameManager.instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
        
        isReady = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.instance.MatchLockedDoor(doorId)) Destroy(gameObject);
        if (!isReady) return;
        if (Input.GetKeyDown("e"))
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        var keyCount = controller.GetKeyCount();
        if (keyCount <= 0)
        {
            UIManager.instance.CreateToast("需要钥匙", "打开这扇门需要钥匙");
            return;
        }
        var keyID = controller.UseKey();
        GameManager.instance.OpenLockedDoor(keyID, doorId);
        AudioManager.instance.PlaySfx(AudioManager.instance.openLock);
        isReady = false;
        Destroy(gameObject);
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
}
