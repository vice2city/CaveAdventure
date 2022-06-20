using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public TextMeshProUGUI keyText, lightTimeText, toastText;
    public GameObject toast;

    private bool played;
    private void Start()
    {
        toast.SetActive(false);
        played = false;
    }
    private void Update()
    {
        if (played)
        {
            Invoke("method", 2.0f);
        }
    }

    public void updateKeyText(int n)
    {
        if (keyText == null) return;
        keyText.text = n.ToString();
    }

    public void updateLightTimeText(float n)
    {
        if (lightTimeText == null) return;
        lightTimeText.text = n.ToString("N");
    }

    public void createToast(string info)
    {
        if (toast.activeSelf == true) return;
        toastText.text = info;
        toast.SetActive(true);
        played = true;
    }

    public void method()
    {
        toast.SetActive(false);
        played = false;
    }
}
