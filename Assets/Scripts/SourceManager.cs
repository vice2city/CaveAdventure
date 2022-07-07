using System;
using UnityEngine;

public class SourceManager : MonoBehaviour
{
    public float add;
    public Sprite sourceFull;
    public Sprite sourceEmpty;
    
    private GameObject light2D;
    private GameObject player;
    private PlayerManager controller;
    private SpriteRenderer image;
    
    private bool isReady;
    private bool isOpen;
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
        isOpen = true;
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
        isOpen = false;
        isReady = false;
        light2D.SetActive(false);
        controller.ShutKeyboardToast();
        image.sprite = sourceEmpty;
    }

    private void GetSource()
    {
        controller.ChangeLightTime(add);
        GameManager.instance.GetSource(caveId);
        ShutDownSource();
    }
}
