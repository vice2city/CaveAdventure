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
    
    public float sliderMaxValue;
    public GameObject toast;
    
    private TextMeshProUGUI keyText, lightTimeText;
    private GameObject gameOverPanel, pauseMenu, toastInstance;
    private RectTransform lightTimeSlider, lightTimeGoalHandle;
    private Animator coverAnim;
    private PlayerManager controller; 
    private static readonly int IsFadeOut = Animator.StringToHash("isFadeOut");

    private void Start()
    {
        //注册组件变量
        gameOverPanel = transform.Find("GameOverPanel").gameObject;
        pauseMenu = transform.Find("PauseMenu").gameObject;
        
        controller = GameManager.instance.GetPlayer().GetComponent<PlayerManager>();
        
        keyText = transform.Find("KeyCount").Find("KeyText").GetComponent<TextMeshProUGUI>();
        lightTimeText = transform.Find("TimeCount").Find("LightTimeText").GetComponent<TextMeshProUGUI>();

        lightTimeSlider = transform.Find("TimeCount").Find("TimeSlider").Find("Fill").GetComponent<RectTransform>();
        lightTimeGoalHandle = transform.Find("TimeCount").Find("TimeSlider").Find("Goal").GetComponent<RectTransform>();

        coverAnim = transform.Find("Cover").GetComponent<Animator>();
        //初始化组件状态
        gameOverPanel.SetActive(false);
        pauseMenu.SetActive(false);
        lightTimeGoalHandle.gameObject.SetActive(false);
        
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
        if (GameManager.instance.IsGamePause()) return;
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (!pauseMenu.activeSelf) OpenPauseMenu();
        else ClosePauseMenu();

    }

    private void SceneIsLoaded()
    {
        controller = GameManager.instance.GetPlayer().GetComponent<PlayerManager>();
        coverAnim.gameObject.SetActive(false);
        coverAnim.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void UpdateKeyText(int n)
    {
        keyText.text = n.ToString();
    }

    public void UpdateLightTime(float n)
    {
        lightTimeText.text = n.ToString("N");
        var scale = n > sliderMaxValue ? 1 : n / sliderMaxValue;
        lightTimeSlider.sizeDelta = new Vector2(scale * 1000, 10);
    }

    public void UpdateGoalHandle(float goal)
    {
        if (goal == 0f)
        {
            lightTimeGoalHandle.gameObject.SetActive(false);
        }
        else
        {
            var scale = goal / sliderMaxValue;
            lightTimeGoalHandle.sizeDelta = new Vector2(scale *1000, 10);
            lightTimeGoalHandle.gameObject.SetActive(true);
        }
    }

    //Toast
    public void CreateToast(string info)
    {
        if(toastInstance) return;
        toastInstance = Instantiate(toast, transform);
        toastInstance.GetComponent<ToastManager>().ChangeToastInfo(info);
    }

    public void OpenCover()
    {
        coverAnim.SetBool(IsFadeOut, true);
    }

    public void OpenGameOverPanel()
    {
        gameOverPanel.transform.localScale = new Vector3(1, 0, 1);
        gameOverPanel.SetActive(true);
        var tween = gameOverPanel.transform.DOScaleY(1, 0.2f);
        tween.SetAutoKill(true);
    }

    public void CloseGameOverPanel()
    {
        var tween = gameOverPanel.transform.DOScaleY(0, 0.2f);
        tween.OnComplete(GameManager.instance.ReloadScene);
        tween.SetAutoKill(true);
    }

    //Pause Menu
    private void OpenPauseMenu()
    {
        GameManager.instance.GamePause(true);
        pauseMenu.SetActive(true);
    }

    private void ClosePauseMenu()
    {
        GameManager.instance.GamePause(false);
        pauseMenu.SetActive(false);
    }
    
    private IEnumerator ReLoadScene()
    {
        coverAnim.SetBool(IsFadeOut, true);
        
        yield return new WaitForSeconds(2);
        GameManager.instance.ReloadScene();
    }
    
    private IEnumerator BackToMainMenu()
    {
        coverAnim.SetBool(IsFadeOut, true);

        yield return new WaitForSeconds(2);
        GameManager.instance.LoadScene(0);
    }
    
    //todo word on wall ui
    //todo new scene: mind , with scripts & performance
    //todo puzzle
}
