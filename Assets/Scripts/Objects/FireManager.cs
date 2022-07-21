using System;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    private string title;
    private int caveID;

    private void Start()
    {
        var id = gameObject.name.Split(" ")[1];
        caveID = id == null ? 0 : Convert.ToInt32(id);
        title = GameManager.instance.caveInstance[caveID].fireInfo;
        
        if(GameManager.instance.IsCaveFinish(caveID)) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerManager>();
        if (!player) return;
        GameManager.instance.FinishCave(caveID);
        UIManager.instance.CreateToast("你获得了"+title, "可以开启新的洞穴");
        var destination = GameObject.FindGameObjectWithTag("Respawn");
        player.transform.position = destination.transform.position;
        player.PauseLightTime(false);
        Destroy(gameObject);
    }
}
