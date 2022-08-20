using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogue", menuName = "Data/New Dialogue")]
public class Dialogue : ScriptableObject
{
    public enum Operation
    {
        NextSentence,
        OpenMenu,
        CloseMenu,
        CloseDialogue,
        ChangeFuel
    }
    
    [Serializable]
    public struct Sentence
    {
        public string[] text;
        public Event[] events;
    }
    
    [Serializable]
    public struct Menu
    {
        public Option[] options;
    }
    
    [Serializable]
    public struct Event
    {
        public Operation op;
        public int opInfo;
    }

    [Serializable]
    public struct Option
    {
        public string text;
        public int nextSentence;
    }

    public Sentence[] sentences;
    public Menu[] menus;
}

