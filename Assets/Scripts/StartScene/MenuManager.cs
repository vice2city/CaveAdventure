using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator coverAnim;
    private static readonly int IsFadeOut = Animator.StringToHash("isFadeOut");

    private void Start()
    {
        if(GameManager.instance != null) Destroy(GameManager.instance.gameObject);
        if(UIManager.instance != null) Destroy(UIManager.instance.gameObject);
    }

    public void StartGame()
    {
        StartCoroutine(LoadScene());
    }

    public void LoadGame()
    {
        GameSettings.instance.isNeedLoadData = true;
        StartCoroutine(LoadScene());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadScene()
    {
        coverAnim.SetBool(IsFadeOut, true);

        yield return new WaitForSeconds(2);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
