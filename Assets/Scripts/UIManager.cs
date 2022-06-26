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
    
    private bool isReady;
    private int timer1Id;
    private Timer timer1;
    private void Start()
    {
        toast.SetActive(false);
        isReady = keyText!=null && lightTimeText!=null && toast!=null;
        
    }
    private void Update()
    {
        timer1?.Update();
    }

    public void UpdateKeyText(int n)
    {
        if (!isReady) return;
        keyText.text = n.ToString();
    }

    public void UpdateLightTimeText(float n)
    {
        if (!isReady) return;
        lightTimeText.text = n.ToString("N");
    }

    public void CreateToast(string info)
    {
        if (toast.activeSelf) return;
        toastText.text = info;
        toast.SetActive(true);

        timer1 = new Timer();
        timer1.Init();
        timer1Id = timer1.Schedule(ShutDownToast, 2, 0, 1);
    }

    private void ShutDownToast()
    {
        toast.SetActive(false);
        timer1.Unschedule(timer1Id);
    }
}
