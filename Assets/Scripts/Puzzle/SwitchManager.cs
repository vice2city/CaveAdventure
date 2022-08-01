using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    public PuzzleManager.BlockColorGroup colorGroup;
    
    private GameObject player;
    private PlayerManager controller;
    
    private Animator anim;
    
    private bool isReady;
    private bool isFormer;
    private static readonly int IsRed = Animator.StringToHash("IsRed");
    private static readonly int IsYellow = Animator.StringToHash("IsYellow");
    // Start is called before the first frame update
    private void Start()
    {
        player = GameManager.instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
        anim = GetComponent<Animator>();
        isFormer = false;
    }

    // Update is called once per frame
    private void Update()
    {
        var state = PuzzleManager.instance.GetColorState(colorGroup);
        if (state!=isFormer) ChangeSwitchState();

        if (!isReady) return;
        if (Input.GetKeyDown("e")) ChangeSwitchState();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
    
    public void ChangeSwitchState()
    {
        isFormer = !isFormer;
        AudioManager.instance.PlaySfx(AudioManager.instance.useSwitch);
        switch (colorGroup)
        {
            case PuzzleManager.BlockColorGroup.RedAndBlue:
                anim.SetBool(IsRed, isFormer);
                break;
            case PuzzleManager.BlockColorGroup.YellowAndPurple:
                anim.SetBool(IsYellow, isFormer);
                break;
            default:
                break;
        }
        PuzzleManager.instance.ChangeColorState(colorGroup, isFormer);
    }
}
