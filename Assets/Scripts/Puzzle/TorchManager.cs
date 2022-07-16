using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchManager : MonoBehaviour
{
    public bool isKeyLit;
    
    private GameObject light2D;
    private Animator anim;
    
    private GameObject player;
    private PlayerManager controller;
    
    private bool isReady;
    private bool isLit;

    private static readonly int IsLit = Animator.StringToHash("IsLit");

    // Start is called before the first frame update
    void Start()
    {
        light2D = transform.Find("Light").gameObject;
        anim = GetComponent<Animator>();
        player = GameManager.instance.GetPlayer();
        controller = player.GetComponent<PlayerManager>();
        
        isReady = false;
        isLit = false;
        light2D.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady) return;
        if (Input.GetKeyDown("e")) ChangeTorchState();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        isReady = true;
        controller.ShowKeyboardToast();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        isReady = false;
        controller.ShutKeyboardToast();
    }

    private void ChangeTorchState()
    {
        isLit = !isLit;
        light2D.SetActive(isLit);
        anim.SetBool(IsLit, isLit);
    }
}
