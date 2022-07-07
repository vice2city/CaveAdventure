using System;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    private string title;
    private int caveID;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerManager>();
        var id = gameObject.name.Split(" ")[1];
        caveID = id == null ? 0 : Convert.ToInt32(id);
        title = GameManager.instance.fireInfo[caveID];

        if (!player) return;
        var result = GameManager.instance.FinishCave(caveID);
        if(result) Destroy(gameObject);
        UIManager.instance.CreateToast("你获得了"+title);
        var destination = GameObject.FindGameObjectWithTag("Respawn");
        player.transform.position = destination.transform.position;
        player.PauseLightTime(false);
    }
}
