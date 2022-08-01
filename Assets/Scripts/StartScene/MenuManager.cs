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
        if(PuzzleManager.instance != null) Destroy(PuzzleManager.instance.gameObject);
        AudioManager.instance.PlayBGM(AudioManager.instance.bgmBeginning);
    }

    public void StartGame()
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.uiButtonClick);
        StartCoroutine(LoadScene());
    }

    public void LoadGame()
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.uiButtonClick);
        GameSettings.instance.isNeedLoadData = true;
        StartCoroutine(LoadScene());
    }

    public void QuitGame()
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.uiButtonClick);
        Application.Quit();
    }

    private IEnumerator LoadScene()
    {
        coverAnim.SetBool(IsFadeOut, true);

        yield return new WaitForSeconds(2);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
