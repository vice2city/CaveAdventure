using UnityEngine;

public class FireManager : MonoBehaviour
{
    public string title;
    public int caveID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerManager>();

        if (!player) return;
        var result = GameManager.instance.FinishCave(caveID);
        if(result) Destroy(gameObject);
        UIManager.instance.CreateToast("你获得了"+title);
        var destination = GameObject.FindGameObjectWithTag("Respawn");
        player.transform.position = destination.transform.position;
        player.PauseLightTime(false);
    }
}
