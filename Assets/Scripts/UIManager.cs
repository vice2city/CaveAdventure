using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    private void Awake()
    {
        
        if(instance != null)
        {
            instance.SceneIsLoaded();
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    public TextMeshProUGUI keyText, lightTimeText, toastText;
    public GameObject toast, gameOverPanel, pauseMenu;
    public Animator coverAnim;

    private bool isReady;
    private bool pauseMenuIsOpen = false;
    private int timer1Id;
    private Timer timer1;
    private PlayerManager controller; 
    private static readonly int IsFadeOut = Animator.StringToHash("isFadeOut");

    private void Start()
    {
        isReady = keyText!=null && lightTimeText!=null && toast!=null;
        gameOverPanel.SetActive(false);
        pauseMenu.SetActive(false);
        controller = GameManager.instance.GetPlayer().GetComponent<PlayerManager>();
        
        //Add listener to pause menu
        var button1 = pauseMenu.transform.Find("Panel").Find("Resume").GetComponent<Button>();
        button1.onClick.AddListener(ClosePauseMenu);
        var button2 = pauseMenu.transform.Find("Panel").Find("Restart").GetComponent<Button>();
        button2.onClick.AddListener(() => StartCoroutine(ReLoadScene()));
        var button3 = pauseMenu.transform.Find("Panel").Find("Load").GetComponent<Button>();
        button3.onClick.AddListener(() =>
        {
            GameObject.Find("DataManager").GetComponent<DataManager>().LoadGameData();
            StartCoroutine(ReLoadScene());
        });
        var button4 = pauseMenu.transform.Find("Panel").Find("Quit").GetComponent<Button>();
        button4.onClick.AddListener(() => StartCoroutine(BackToMainMenu()));
    }
    
    private void Update()
    {
        timer1?.Update();
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (!pauseMenuIsOpen) OpenPauseMenu();
        else ClosePauseMenu();
    }

    private void SceneIsLoaded()
    {
        controller = GameManager.instance.GetPlayer().GetComponent<PlayerManager>();
        coverAnim.gameObject.SetActive(false);
        coverAnim.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
        pauseMenu.SetActive(false);
        pauseMenuIsOpen = false;
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

    //Toast
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

    public void OpenCover()
    {
        coverAnim.SetBool(IsFadeOut, true);
    }

    public void OpenGameOverPanel()
    {
        gameOverPanel.transform.localScale = new Vector3(1, 0, 1);
        gameOverPanel.SetActive(true);
        var tween = gameOverPanel.transform.DOScaleY(1, 0.5f);
        tween.SetAutoKill(true);
    }

    public void CloseGameOverPanel()
    {
        var tween = gameOverPanel.transform.DOScaleY(0, 0.5f);
        tween.OnComplete(GameManager.instance.ReloadScene);
        tween.SetAutoKill(true);
    }

    //Pause Menu
    private void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        pauseMenuIsOpen = true;
        Time.timeScale = 0f;
    }

    private void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        pauseMenuIsOpen = false;
        Time.timeScale = 1f;
    }
    
    private IEnumerator ReLoadScene()
    {
        controller.PauseLightTime(true);
        Time.timeScale = 1f;
        coverAnim.SetBool(IsFadeOut, true);
        
        yield return new WaitForSeconds(2);
        GameManager.instance.ReloadScene();
    }
    
    private IEnumerator BackToMainMenu()
    {
        controller.PauseLightTime(true);
        Time.timeScale = 1f;
        coverAnim.SetBool(IsFadeOut, true);

        yield return new WaitForSeconds(2);
        GameManager.instance.BackToMainMenu();
    }

}
