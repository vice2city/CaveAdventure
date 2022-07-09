using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AreaLightManager : MonoBehaviour
{
    private int caveId;
    private Light2D[] children;
    // Start is called before the first frame update
    private void Start()
    {
        var id = gameObject.name.Split(" ")[1];
        caveId = Convert.ToInt32(id);
        children = GetComponentsInChildren<Light2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        ChangeLightsState(GameManager.instance.IsCaveFinish(caveId));
    }

    private void ChangeLightsState(bool state)
    {
        foreach (var light2D in children)
        {
            light2D.gameObject.SetActive(state);
        }
    }
}
