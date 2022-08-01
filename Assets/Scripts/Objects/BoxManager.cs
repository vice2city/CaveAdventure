using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public Skill item;
    public Sprite openedImage;
    
    private GameObject player;
    private PlayerManager controller;

    private Vector2Int boxId;
    private SpriteRenderer image;
    private GameObject light2D;

    private bool isReady;
    private bool isOpened;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
        image = GetComponent<SpriteRenderer>();
        light2D = transform.Find("Light").gameObject;

        var id = gameObject.name.Split(" ")[1].Split("-");
        boxId.x = Convert.ToInt32(id[0]);
        boxId.y = Convert.ToInt32(id[1]);
        
        isReady = false;
        isOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpened) return;
        if (GameManager.instance.IsBoxOpened(boxId)) ShutDownBox(); 
        if (!isReady) return;
        if (!Input.GetKeyDown("e")) return;
        OpenBox();
    }

    private void OpenBox()
    {
        controller.ShutKeyboardToast();
        AudioManager.instance.PlaySfx(AudioManager.instance.openBox);
        GameManager.instance.OpenTreasureBox(item.skillId, boxId);
        UIManager.instance.CreateToast(item.skillName, item.skillDescription, item.skillImage);
        ShutDownBox();
    }

    private void ShutDownBox()
    {
        isOpened = true;
        isReady = false;
        light2D.SetActive(false);
        image.sprite = openedImage;
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
}
