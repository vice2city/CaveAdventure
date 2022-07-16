using System.Collections;
using TMPro;
using UnityEngine;

public class ToastManager : MonoBehaviour
{
    private TextMeshProUGUI toastText;
    private Animator toastAnim;
    private static readonly int IsFadeOut = Animator.StringToHash("isFadeOut");

    private void Awake()
    {
        toastText = transform.Find("ToastText").GetComponent<TextMeshProUGUI>();
        toastAnim = GetComponent<Animator>();
        GameManager.instance.GamePause(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        StartCoroutine(CloseToast());
    }
    
    public void ChangeToastInfo(string info)
    {
        toastText.text = info;
    }
    
    private IEnumerator CloseToast()
    {
        GameManager.instance.GamePause(false);
        toastAnim.SetBool(IsFadeOut, true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
