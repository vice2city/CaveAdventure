using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "Data/New Skill")]
public class Skill : ScriptableObject
{
    public int skillId;
    public string skillName;
    public string skillDescription;
    public Sprite skillImage;
}
