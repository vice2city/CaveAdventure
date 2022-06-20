using UnityEngine;

public class LockedDoorManager : MonoBehaviour
{
    public int doorID;
    public GameObject buttonBox;

    private bool isReady;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        buttonBox.SetActive(false);
        isReady = false;
        player = GameManager.Instance.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (Input.GetKeyDown("e"))
            {
                int keyCount = player.GetComponent<PlayerManager>().GetKeyCount();
                if (keyCount <= 0)
                {
                    UIManager.instance.createToast("??????");
                    return;
                }
                int keyID = player.GetComponent<PlayerManager>().UseKey();
                GameManager.Instance.OpenLockedDoor(keyID, doorID);
                foreach(var door in GameObject.FindGameObjectsWithTag("LockedDoor"))
                {
                    if(!door.GetComponent<LockedDoorManager>()) continue;
                    int id = door.GetComponent<LockedDoorManager>().doorID;
                    if (door == this.gameObject) continue;
                    if (doorID == id) Destroy(door);
                }
                isReady = false;
                Destroy(gameObject);
            }
        }
        if(GameManager.Instance.MatchLockedDoor(doorID)) gameObject.SetActive(false);
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
}
