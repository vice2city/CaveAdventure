using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateManager : MonoBehaviour
{
    private bool isPressed;
    private Collider2D plateCoil;

    private void Start()
    {
        plateCoil = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerManager>()==null) return;
        if (isPressed) transform.parent.GetComponent<PlatePuzzle>().ChangePlatesState(false);
        else ChangePlateState(true);
    }

    //true: plate pressed; false:plate back;
    public void ChangePlateState(bool state)
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.pressPlate);
        var block = GetComponent<SpriteRenderer>();
        block.color = state ? new Color(0.7f, 0.7f, 0.7f) : Color.white;
        block.flipY = state;
        isPressed = state;
    }

    public bool IsPlatePressed()
    {
        return isPressed;
    }

    public void FinishPlate()
    {
       plateCoil.enabled = false;
    }
    
}
