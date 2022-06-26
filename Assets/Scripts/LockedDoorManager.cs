using UnityEngine;

public class LockedDoorManager : MonoBehaviour
{
    public int doorID;
    
    private GameObject buttonBox;
    private bool isReady;
    private GameObject player;
    // Start is called before the first frame update
    private void Start()
    {
        buttonBox = transform.Find("Canvas").Find("Button").gameObject;
        buttonBox.SetActive(false);
        isReady = false;
        player = GameManager.instance.GetPlayer();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isReady)
        {
            if (Input.GetKeyDown("e"))
            {
                var keyCount = player.GetComponent<PlayerManager>().GetKeyCount();
                if (keyCount <= 0)
                {
                    UIManager.instance.CreateToast("需要钥匙");
                    return;
                }
                var keyID = player.GetComponent<PlayerManager>().UseKey();
                GameManager.instance.OpenLockedDoor(keyID, doorID);
                foreach(var door in GameObject.FindGameObjectsWithTag("LockedDoor"))
                {
                    if(!door.GetComponent<LockedDoorManager>()) continue;
                    var id = door.GetComponent<LockedDoorManager>().doorID;
                    if (door == this.gameObject) continue;
                    if (doorID == id) Destroy(door);
                }
                isReady = false;
                Destroy(gameObject);
            }
        }
        if(GameManager.instance.MatchLockedDoor(doorID)) gameObject.SetActive(false);
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
}
