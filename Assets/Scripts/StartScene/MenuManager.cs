using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        var cover = transform.Find("Cover").gameObject;
        cover.SetActive(false);
        if(GameManager.instance != null) Destroy(GameManager.instance.gameObject);
        if(UIManager.instance != null) Destroy(UIManager.instance.gameObject);
    }

    public void StartGame()
    {
        PlayTransition();
    }

    public void LoadGame()
    {
        GameSettings.instance.isNeedLoadData = true;
        PlayTransition();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void PlayTransition()
    {
        var cover = transform.Find("Cover").gameObject;
        cover.SetActive(true);
        var coverAnim = gameObject.GetComponent<Animation>();
        coverAnim.Play();
    }
}
