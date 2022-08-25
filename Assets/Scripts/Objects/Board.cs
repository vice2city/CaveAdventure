using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Board : MonoBehaviour
{
    public GameObject storyBoard;
    public Dialogue dialogue;

    private StoryBoard board;
    private PlayerManager controller;
    protected Light2D globalLight;
    protected float lightIntensity;
    private bool isReady;
    protected TweenerCore<Color, Color, ColorOptions> tween;

    private void Start()
    {
        controller = GameManager.instance.GetPlayer().GetComponent<PlayerManager>();
        globalLight = GameObject.FindWithTag("GlobalLight").GetComponent<Light2D>();

        tween = gameObject.GetComponent<SpriteRenderer>().DOFade(0.5f, 1f);
        tween.SetLoops(-1, LoopType.Yoyo);
    }
    
    private void Update()
    {
        if (!isReady) return;
        if (!Input.GetKeyDown("e")) return;
        OpenStoryBoard();
    }

    private void OpenStoryBoard()
    {
        var instance = Instantiate(storyBoard, UIManager.instance.transform);
        instance.SetActive(false);
        board = instance.GetComponent<StoryBoard>();
        board.dialogue = dialogue;
        board.boardClosed.AddListener(BoardClosed);
        lightIntensity = globalLight.intensity;
        DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 
            0.1f, 1f);
        GameManager.instance.GamePause(true);
        board.gameObject.SetActive(true);
    }

    protected virtual void BoardClosed()
    {
        board.gameObject.SetActive(false);
        DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 
            lightIntensity, 1f);
        GameManager.instance.GamePause(false);
        tween.Kill();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        isReady = true;
        controller.ShowKeyboardToast();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var other = collision.GetComponent<PlayerManager>();
        if (other == null) return;
        isReady = false;
        controller.ShutKeyboardToast();
    }
}
