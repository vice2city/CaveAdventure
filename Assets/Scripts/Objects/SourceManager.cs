using System;
using UnityEngine;

public class SourceManager : MonoBehaviour
{
    public float add;
    public float addEasy;
    public Sprite sourceFull;
    public Sprite sourceEmpty;
    
    private GameObject light2D;
    private GameObject player;
    private PlayerManager controller;
    private SpriteRenderer image;

    private bool isReady;
    private bool isOpened;
    private int caveId;
    
    // Start is called before the first frame update
    private void Start()
    {
        light2D = transform.Find("Light").gameObject;
        image = this.GetComponent<SpriteRenderer>();
        light2D.SetActive(true);
        player = GameManager.instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();

        var id = gameObject.name.Split(" ")[1].Split("-")[0];
        caveId = Convert.ToInt32(id);
        isReady = false;
        isOpened = false;
        image.sprite = sourceFull;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isReady)
        {
            if (Input.GetKeyDown("e")) GetSource();
        }

        if (isOpened) return;
        if (GameManager.instance.IsCaveFinish(caveId)) ShutDownSource();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpened) return;
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        isReady = true;
        controller.ShowKeyboardToast();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        isReady = false;
        controller.ShutKeyboardToast();
    }

    private void ShutDownSource()
    {
        isOpened = true;
        isReady = false;
        light2D.SetActive(false);
        image.sprite = sourceEmpty;
    }

    private void GetSource()
    {
        controller.ShutKeyboardToast();
        AudioManager.instance.PlaySfx(AudioManager.instance.getSource);
        controller.ChangeLightTime(GameManager.instance.GetGameDifficulty() ? addEasy : add);
        GameManager.instance.GetSource(caveId);
        ShutDownSource();
    }
}
