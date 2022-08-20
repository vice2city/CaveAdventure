using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScene : MonoBehaviour
{
    public Dialogue startDialogue;
    public Dialogue endDialogue;
    public GameObject storyBoard;
    
    public void StartDialogue()
    {
        Instantiate(storyBoard, transform);
    }

}
