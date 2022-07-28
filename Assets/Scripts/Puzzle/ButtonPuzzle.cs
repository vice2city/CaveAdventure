using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonPuzzle : MonoBehaviour
{
    public GameObject blocks;
    
    private SpriteRenderer[] blocksImage;
    private BoxCollider2D[] blocksCoil;
    private ButtonManager[] buttons;

    private bool blockState;
    private void Start()
    {
        blocksImage = blocks.GetComponentsInChildren<SpriteRenderer>();
        blocksCoil = blocks.GetComponentsInChildren<BoxCollider2D>();
        buttons = GetComponentsInChildren<ButtonManager>();
        blockState = false;
    }

    private void Update()
    {
        var result = buttons.All(button => button.IsButtonPressed());
        if (result == blockState) return;
        blockState = result;
        ChangeBlockState(blockState);
    }

    //true:close block; false: open block;
    private void ChangeBlockState(bool state)
    {
        foreach (var block in blocksImage)
        {
            block.color = state ? new Color(0.7f, 0.7f, 0.7f) : Color.white;
            block.flipY = state;
        }

        foreach (var block in blocksCoil)
        {
            block.enabled = !state;
        }
    }
}
