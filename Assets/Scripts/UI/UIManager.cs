using System;
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
    public GameObject[] skillGroup;
    
    private TextMeshProUGUI keyText, lightTimeText, spareText;
    private GameObject gameOverPanel, pauseMenu, toastInstance;
    private RectTransform lightTimeSlider, lightTimeGoalHandle;
    private Animator coverAnim;
    private static readonly int IsFadeOut = Animator.StringToHash("isFadeOut");

    private void Start()
    {
        //注册组件变量
        gameOverPanel = transform.Find("GameOverPanel").gameObject;
        pauseMenu = transform.Find("PauseMenu").gameObject;

        keyText = transform.Find("KeyCount").Find("KeyText").GetComponent<TextMeshProUGUI>();
        lightTimeText = transform.Find("TimeCount").Find("LightTimeText").GetComponent<TextMeshProUGUI>();
        spareText = transform.Find("SpareCount").Find("SpareText").GetComponent<TextMeshProUGUI>();

        lightTimeSlider = transform.Find("TimeCount").Find("TimeSlider").Find("Fill").GetComponent<RectTransform>();
        lightTimeGoalHandle = transform.Find("TimeCount").Find("TimeSlider").Find("Goal").GetComponent<RectTransform>();

        coverAnim = transform.Find("Cover").GetComponent<Animator>();
        //初始化组件状态
        gameOverPanel.SetActive(false);
        pauseMenu.SetActive(false);
        lightTimeGoalHandle.gameObject.SetActive(false);
        Cursor.visible = false;
    }
    
    private void Update()
    {
        if (GameManager.instance.IsGamePause()) return;
        if (!Input.GetKeyDown(KeyCode.Escape)) return;  
        if (!pauseMenu.activeSelf) OpenPauseMenu();
    }

    private void SceneIsLoaded()
    {
        coverAnim.gameObject.SetActive(false);
        coverAnim.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void UpdateSpareText(int n)
    {
        spareText.text = n.ToString();
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

    public void UpdateSkillBoard(bool[] state)
    {
        for (var i = 0; i < state.Length; i++)
        {
            skillGroup[i].SetActive(state[i]);
        }
    }

    //Toast
    public void CreateToast(string title, string text, Sprite image=null)
    {
        if(toastInstance) return;
        AudioManager.instance.PlaySfx(AudioManager.instance.uiToast);
        toastInstance = Instantiate(toast, transform);
        var manager = toastInstance.GetComponent<ToastManager>();
        manager.ChangeToastInfo(title, text);
        if (image) manager.ChangeToastImage(image);
    }

    public void OpenCover()
    {
        coverAnim.SetBool(IsFadeOut, true);
    }

    public void OpenGameOverPanel()
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.bgmGameOver);
        gameOverPanel.transform.localScale = new Vector3(1, 0, 1);
        gameOverPanel.SetActive(true);
        var tween = gameOverPanel.transform.DOScaleY(1, 0.2f);
        tween.SetAutoKill(true);
        Cursor.visible = true;
    }

    public void CloseGameOverPanel()
    {
        var tween = gameOverPanel.transform.DOScaleY(0, 0.2f);
        tween.OnComplete(GameManager.instance.ReloadScene);
        tween.SetAutoKill(true);
        Cursor.visible = false;
    }

    //Pause Menu
    private void OpenPauseMenu()
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.uiButtonClick);
        GameManager.instance.GamePause(true);
        pauseMenu.SetActive(true);
        Cursor.visible = true;
    }
    
}
