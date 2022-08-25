using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Plant : Board
{
    protected override void BoardClosed()
    {
        DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 
            lightIntensity, 1f);
        GameManager.instance.GamePause(false);
        StartCoroutine(BackToMainMenu());
    }
    
    private IEnumerator BackToMainMenu()
    {
        UIManager.instance.OpenCover();
        yield return new WaitForSeconds(2);
        GameManager.instance.LoadScene(0);
    }
}
