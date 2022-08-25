using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private TextMeshProUGUI difficulty;
    // Start is called before the first frame update
    void Start()
    {
        //Add listener to pause menu
        var button1 = transform.Find("Panel").Find("Resume").GetComponent<Button>();
        button1.onClick.AddListener(ClosePauseMenu);
        var button2 = transform.Find("Panel").Find("Restart").GetComponent<Button>();
        button2.onClick.AddListener(() => StartCoroutine(ReLoadScene()));
        var button3 = transform.Find("Panel").Find("Store").GetComponent<Button>();
        button3.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySfx(AudioManager.instance.uiButtonClick);
            GameObject.Find("DataManager").GetComponent<DataManager>().SaveGameData();
        });
        var button4 = transform.Find("Panel").Find("Load").GetComponent<Button>();
        button4.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySfx(AudioManager.instance.uiButtonClick);
            GameObject.Find("DataManager").GetComponent<DataManager>().LoadGameData();
            StartCoroutine(ReLoadScene());
        });
        var button5 = transform.Find("Panel").Find("Quit").GetComponent<Button>();
        button5.onClick.AddListener(() => StartCoroutine(BackToMainMenu()));
        var button6 = transform.Find("Panel").Find("Switch").GetComponent<Button>();
        button6.onClick.AddListener(SwitchGameDifficulty);
        difficulty = button6.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDifficultyText();
        if (!Input.GetKeyDown(KeyCode.Escape)) return; 
        ClosePauseMenu();
    }
    
    private IEnumerator ReLoadScene()
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.uiButtonClick);
        UIManager.instance.OpenCover();
        GameManager.instance.GamePause(false);
        yield return new WaitForSeconds(2);
        GameManager.instance.ReloadScene();
    }
    
    private IEnumerator BackToMainMenu()
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.uiButtonClick);
        UIManager.instance.OpenCover();
        yield return new WaitForSeconds(2);
        GameManager.instance.LoadScene(0);
    }
    
    private void ClosePauseMenu()
    {
        Cursor.visible = false;
        AudioManager.instance.PlaySfx(AudioManager.instance.uiButtonClick);
        GameManager.instance.GamePause(false);
        gameObject.SetActive(false);
    }

    private void SwitchGameDifficulty()
    {
        GameManager.instance.ChangeGameDifficulty();
    }

    private void UpdateDifficultyText()
    {
        var diff = GameManager.instance.GetGameDifficulty();
        difficulty.text = diff switch
        {
            true => "难度切换  <size=80%>简单",
            false => "难度切换  <size=80%>普通"
        };
    }
}
