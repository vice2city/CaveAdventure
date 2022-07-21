using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastManager : MonoBehaviour
{
    private TextMeshProUGUI toastTitle;
    private TextMeshProUGUI toastText;
    private Image toastImage;
    private Animator toastAnim;
    private static readonly int IsFadeOut = Animator.StringToHash("isFadeOut");

    private void Awake()
    {
        toastTitle = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        toastText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        toastImage = transform.Find("Image").GetComponent<Image>();
        toastAnim = GetComponent<Animator>();
        GameManager.instance.GamePause(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        StartCoroutine(CloseToast());
    }
    
    public void ChangeToastInfo(string title, string text)
    {
        toastTitle.text = title;
        toastText.text = text;
    }

    public void ChangeToastImage(Sprite image)
    {
        toastImage.sprite = image;
    }
    
    private IEnumerator CloseToast()
    {
        GameManager.instance.GamePause(false);
        toastAnim.SetBool(IsFadeOut, true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
