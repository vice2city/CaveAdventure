using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
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
            GameObject.Find("DataManager").GetComponent<DataManager>().SaveGameData());
        var button4 = transform.Find("Panel").Find("Load").GetComponent<Button>();
        button4.onClick.AddListener(() =>
        {
            GameObject.Find("DataManager").GetComponent<DataManager>().LoadGameData();
            StartCoroutine(ReLoadScene());
        });
        var button5 = transform.Find("Panel").Find("Quit").GetComponent<Button>();
        button5.onClick.AddListener(() => StartCoroutine(BackToMainMenu()));
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return; 
        ClosePauseMenu();
    }
    
    private IEnumerator ReLoadScene()
    {
        UIManager.instance.OpenCover();
        GameManager.instance.GamePause(false);
        yield return new WaitForSeconds(2);
        GameManager.instance.ReloadScene();
    }
    
    private IEnumerator BackToMainMenu()
    {
        UIManager.instance.OpenCover();
        yield return new WaitForSeconds(2);
        GameManager.instance.LoadScene(0);
    }
    
    private void ClosePauseMenu()
    {
        GameManager.instance.GamePause(false);
        gameObject.SetActive(false);
    }
}
