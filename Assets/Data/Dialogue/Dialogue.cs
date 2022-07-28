using System;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogue", menuName = "Data/New Dialogue")]
public class Dialogue : ScriptableObject
{
    public enum Type
    {
        Text,
        Menu
    }
    
    [Serializable]
    public struct Sentence
    {
        public Type type;
        public string text;
        public int menuNum;
        public string[] options;
        public int[] nextSentenceId;
    }
    
    public Sentence[] sentences;
}
