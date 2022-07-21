using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzle : MonoBehaviour
{
    private SpriteRenderer[] blocks;
    private BoxCollider2D[] blocksCoil;
    private TorchManager[] torches;
    
    private int keyGoalNum;
    private int keyNum;
    // Start is called before the first frame update
    private void Start()
    {
        torches = GetComponentsInChildren<TorchManager>();
        keyGoalNum = torches.Length;

        blocks = transform.Find("Blocks").GetComponentsInChildren<SpriteRenderer>();
        blocksCoil = transform.Find("Blocks").GetComponentsInChildren<BoxCollider2D>();
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
