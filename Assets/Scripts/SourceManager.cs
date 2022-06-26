using UnityEngine;

public class SourceManager : MonoBehaviour
{
    public float add;
    public Sprite sourceFull;
    public Sprite sourceEmpty;
    public int caveId;

    private GameObject buttonBox;
    private GameObject light2D;
    private GameObject player;
    private PlayerManager controller;
    private SpriteRenderer image;
    private bool isReady;
    private bool isOpen;
    // Start is called before the first frame update
    private void Start()
    {
        buttonBox = transform.Find("Canvas").Find("Button").gameObject;
        light2D = transform.Find("Light").gameObject;
        buttonBox.SetActive(false);
        image = this.GetComponent<SpriteRenderer>();
        light2D.SetActive(true);
        isReady = false;
        isOpen = true;
        player = GameManager.instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
        image.sprite = sourceFull;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isReady)
        {
            if (Input.GetKeyDown("e")) GetSource();
        }

        if (!isOpen) return;
        if (GameManager.instance.IsCaveFinish(caveId)) ShutDownSource();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        if (!isOpen) return;
        isReady = true;
        buttonBox.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        isReady = false;
        buttonBox.SetActive(false);
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
        GameManager.instance.GetSource(caveId);
        ShutDownSource();
    }
}
