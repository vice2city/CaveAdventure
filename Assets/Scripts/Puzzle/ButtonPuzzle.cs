using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPuzzle : MonoBehaviour
{
    private SpriteRenderer[] blocks;
    private BoxCollider2D[] blocksCoil;
    private ButtonManager button;

    private bool blockState;
    private void Start()
    {
        blocks = transform.Find("Blocks").GetComponentsInChildren<SpriteRenderer>();
        blocksCoil = transform.Find("Blocks").GetComponentsInChildren<BoxCollider2D>();
        button = transform.Find("Button").GetComponent<ButtonManager>();
        blockState = false;
    }

    private void Update()
    {
        if (button.IsButtonPressed() == blockState) return;
        blockState = button.IsButtonPressed();
        ChangeBlockState(blockState);
    }

    //true:close block; false: open block;
    private void ChangeBlockState(bool state)
    {
        foreach (var block in blocks)
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
