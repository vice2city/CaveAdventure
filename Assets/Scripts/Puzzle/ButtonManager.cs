using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public Sprite defaultImage;
    public Sprite pressedImage;

    private bool isPressed;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerManager>()==null && other.GetComponent<TrunkManager>()==null) return;
        AudioManager.instance.PlaySfx(AudioManager.instance.pressButton);
        ChangeButtonState(true);
        isPressed = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerManager>()==null && other.GetComponent<TrunkManager>()==null) return;
        ChangeButtonState(false);
        isPressed = false;
    }
    
    //true: button pressed; false:button back;
    private void ChangeButtonState(bool state)
    {
        GetComponent<SpriteRenderer>().sprite = state ? pressedImage : defaultImage;
    }

    public bool IsButtonPressed()
    {
        return isPressed;
    }
}
