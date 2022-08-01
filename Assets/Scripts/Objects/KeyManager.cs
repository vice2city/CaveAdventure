using System;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    private Vector2Int keyId;

    private void Start()
    {
        var id = gameObject.name.Split(" ")[1].Split("-");
        keyId.x = Convert.ToInt32(id[0]);
        keyId.y = Convert.ToInt32(id[1]);
    }

    private void Update()
    {
        if (GameManager.instance.MatchKey(keyId)) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerManager>();

        if (player == null) return;
        player.GetKey(keyId);
        AudioManager.instance.PlaySfx(AudioManager.instance.getKey);
        Destroy(gameObject);
    }
}
