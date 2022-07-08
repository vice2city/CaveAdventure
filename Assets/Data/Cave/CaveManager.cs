using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new cave", menuName = "Data/New Cave")]
public class CaveManager : ScriptableObject
{
    public int caveId;
    public string caveName;
    public Color caveColor;
    public string fireInfo;
    public int caveGoal;
}