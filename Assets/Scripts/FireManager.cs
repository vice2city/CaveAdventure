using UnityEngine;

public class FireManager : MonoBehaviour
{
    public string title;
    public int caveID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerManager player = collision.GetComponent<PlayerManager>();

        if (player)
        {
            bool result1 = GameManager.Instance.OpenGate(caveID+1);
            bool result2 = GameManager.Instance.FinishCave(caveID);
            if(result1&&result2) Destroy(gameObject);
            UIManager.instance.createToast("你获得了"+title);
            GameObject destination = GameObject.FindGameObjectWithTag("Respawn");
            player.transform.position = destination.transform.position;
            player.PauseLightTime(false);
        }
    }
}
