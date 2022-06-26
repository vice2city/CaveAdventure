using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public int keyID;
    
    private void Update()
    {
        if (GameManager.instance.MatchKey(keyID)) gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerManager>();

        if (player == null) return;
        player.GetKey(keyID);
        Destroy(gameObject);
    }
}
