using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skill", menuName = "Data/New Skill")]
public class SkillManager : ScriptableObject
{
    public int skillId;
    public string skillName;
    public string skillDescription;
    public Sprite skillImage;
}
