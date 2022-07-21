using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    
    private void Awake()
    {
        
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    private bool isRed;
    private bool isYellow;

    public enum BlockColorGroup
    {
        RedAndBlue,
        YellowAndPurple
    }

    public void ChangeColorState(BlockColorGroup colorGroup, bool state)
    {
        switch (colorGroup)
        {
            case BlockColorGroup.RedAndBlue:
                isRed = state;
                break;
            case BlockColorGroup.YellowAndPurple:
                isYellow = state;
                break;
            default:
                break;
        }
    }

    public bool GetColorState(BlockColorGroup colorGroup)
    {
        return colorGroup switch
        {
            BlockColorGroup.RedAndBlue => isRed,
            BlockColorGroup.YellowAndPurple => isYellow,
            _ => false
        };
    }
}
