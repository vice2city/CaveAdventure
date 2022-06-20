using UnityEngine;

public class SourceManager : MonoBehaviour
{
    public GameObject buttonBox;
    public GameObject light2D;
    public float add;
    public Sprite sourceFull;
    public Sprite sourceEmpty;
    public int caveId;

    private GameObject player;
    private PlayerManager controller;
    private SpriteRenderer image;
    private bool isReady;
    private bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        buttonBox.SetActive(false);
        image = this.GetComponent<SpriteRenderer>();
        light2D.SetActive(true);
        isReady = false;
        isOpen = true;
        player = GameManager.Instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
        image.sprite = sourceFull;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (Input.GetKeyDown("e")) GetSource();
        }

        if (isOpen)
        {
            if (GameManager.Instance.IsCaveFinish(caveId)) ShutDownSource();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerManager other = collision.GetComponent<PlayerManager>();
        if (other != null)
        {
            if (isOpen)
            {
                isReady = true;
                buttonBox.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        PlayerManager other = collision.GetComponent<PlayerManager>();
        if (other != null)
        {
            isReady = false;
            buttonBox.SetActive(false);
        }
    }

    private void ShutDownSource()
    {
        isOpen = false;
        isReady = false;
        light2D.SetActive(false);
        buttonBox.SetActive(false);
        image.sprite = sourceEmpty;
    }

    private void GetSource()
    {
        controller.ChangeLightTime(add);
        GameManager.Instance.GetSource(caveId);
        ShutDownSource();
    }
}
