using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkManager : MonoBehaviour
{
    public GameObject smoke;
    public void Destroy()
    {
        Instantiate(smoke, transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }
}
