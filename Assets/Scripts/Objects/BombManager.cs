using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public GameObject smoke;
    public float range = 1f;

    public void BombExplode()
    {
        Boom(Vector2.down);
        Boom(Vector2.up);
        Boom(Vector2.left);
        Boom(Vector2.right);
        Instantiate(smoke, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }

    private void Boom(Vector2 direction)
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.bombExplode);
        var hit = Physics2D.Raycast(transform.position + (Vector3)direction * 0.5f, direction, 
            range, LayerMask.GetMask("Default"));
        if (!hit) return;
        if (hit.collider.CompareTag("Trunk"))
        {
            hit.collider.GetComponent<TrunkManager>().Destroy();
        }
        else if (hit.collider.CompareTag("Switch"))
        {
            hit.collider.GetComponent<SwitchManager>().ChangeSwitchState();
        }
    }
}
