using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StoryBoard : MonoBehaviour
{
    public Dialogue dialogue;
    public Transform content;
    public GameObject text;
    public GameObject nextButton;
    public UnityEvent boardClosed;

    private int index;
    private bool isMenuOpen;
    private bool[] isOptionClicked;
    private Dialogue.Menu menu;
    private GameObject buttonInstance;
    private GameObject menuInstance;
    private static readonly int IsOut = Animator.StringToHash("IsOut");

    private void Start()
    {
        index = 0;
        isMenuOpen = false;
        CreateDialogue();
    }

    private void Update()
    {
        if (isMenuOpen)
        {
            var i = -1;
            if (Input.GetKeyDown(KeyCode.Alpha1)) i = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2)) i = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3)) i = 2;
            if (Input.GetKeyDown(KeyCode.Alpha4)) i = 3;
            if (Input.GetKeyDown(KeyCode.Alpha5)) i = 4;
            if (i < 0 || i > menu.options.Length-1) return;
            ClickMenu(i);
        }
        
        if (buttonInstance)
        {
            if (Input.GetKeyDown(KeyCode.Return)) ClickButton();
        }
        
    }

    private void CreateDialogue()
    {
        if (index>dialogue.sentences.Length-1) return;
        var sentence = dialogue.sentences[index];
        CreateText(sentence);
        foreach (var e in sentence.events)
        {
            switch (e.op)
            {
                case Dialogue.Operation.NextSentence:
                    index++;
                    CreateButton();
                    break;
                case Dialogue.Operation.OpenMenu:
                    menu = dialogue.menus[e.opInfo];
                    CreateMenu();
                    break;
                case Dialogue.Operation.CloseMenu:
                    isOptionClicked = null;
                    isMenuOpen = false;
                    break;
                case Dialogue.Operation.CloseDialogue:
                    index = dialogue.sentences.Length;
                    CreateButton();
                    break;
            }
        }
    }

    private void CreateText(Dialogue.Sentence sentence)
    {
        foreach (var t in sentence.text)
        {
            var textInstance = Instantiate(text, content);
            var texts = textInstance.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var ui in texts) ui.text = t;
        }
    }
    
    private void CreateButton()
    {
        buttonInstance = Instantiate(nextButton, content);
        buttonInstance.GetComponentInChildren<Button>().onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        if (index > dialogue.sentences.Length - 1) StartCoroutine(ShutDownBoard());
        else
        {
            Destroy(buttonInstance);
            CreateDialogue();
        }
    }

    private void CreateMenu()
    {
        var textInstance = Instantiate(text, content);
        var options = "";
        for (var i = 0; i < menu.options.Length; i++)
        {
            var sprite = "<sprite=\"Tips\" index=" + (6 + i) + ">";
            string option;
            if (isOptionClicked != null && isOptionClicked[i])
            {
                option = sprite + "<s>" + menu.options[i].text + "</s><br>";
            }
            else
            {
                option = sprite + menu.options[i].text + "<br>";
            }
            options += option;
        }
        var texts = textInstance.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var ui in texts) ui.text = options;
        menuInstance = textInstance;
        isOptionClicked ??= new bool[menu.options.Length];
        isMenuOpen = true;
    }

    private void ClickMenu(int i)
    {
        if (i < 0) return;
        index = menu.options[i].nextSentence;
        isOptionClicked[i] = true;
        isMenuOpen = false;
        Destroy(menuInstance);
        CreateDialogue();
    }

    private IEnumerator ShutDownBoard()
    {
        GetComponent<Animator>().SetBool(IsOut, true);
        yield return new WaitForSeconds(0.5f);
        boardClosed.Invoke();
        Destroy(gameObject);
    }
}
