using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerManager>();
        if (player == null) return;
        UIManager.instance.CreateToast("你找到了一棵植物","恭喜你，历险结束了。<br>这是一个早期版本，希望你玩的开心。");
        StartCoroutine(BackToMainMenu());
    }
    
    private IEnumerator BackToMainMenu()
    {
        yield return new WaitForSeconds(5);
        UIManager.instance.OpenCover();
        yield return new WaitForSeconds(2);
        GameManager.instance.LoadScene(0);
    }
}
