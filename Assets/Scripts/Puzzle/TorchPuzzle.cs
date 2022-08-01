using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzle : MonoBehaviour
{
    public GameObject blocks;
    
    private SpriteRenderer[] blocksImage;
    private BoxCollider2D[] blocksCoil;
    private TorchManager[] torches;
    
    private int keyGoalNum;
    private int keyNum;
    private bool currentState;
    // Start is called before the first frame update
    private void Start()
    {
        torches = GetComponentsInChildren<TorchManager>();
        keyGoalNum = torches.Length;
        currentState = false;

        blocksImage = blocks.GetComponentsInChildren<SpriteRenderer>();
        blocksCoil = blocks.GetComponentsInChildren<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        keyNum = 0;
        foreach (var torch in torches) if (torch.GetTorchState()) keyNum++;
        ChangeBlockState(keyNum == keyGoalNum);
    }

    //true:close block; false: open block;
    private void ChangeBlockState(bool state)
    {
        if (state == currentState) return;
        currentState = state;
        if (state) StartCoroutine(AudioManager.instance.PlayPuzzleSolved());
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
