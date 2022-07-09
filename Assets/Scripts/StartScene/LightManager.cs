using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Math.Abs(mainCamera.transform.position.z);
        var mouseScreenPos = mainCamera.ScreenToWorldPoint(mousePos);
        gameObject.transform.position = mouseScreenPos;
    }
}
