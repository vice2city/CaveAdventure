using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keymanager : MonoBehaviour
{
    public int keyID;

    void Start()
    {
        
    }
    void Update()
    {
        if (GameManager.Instance.MatchKey(keyID)) gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player != null)
        {
            player.GetKey(keyID);
            Destroy(gameObject);
        }
    }
}
