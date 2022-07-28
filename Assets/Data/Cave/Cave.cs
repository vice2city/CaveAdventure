using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cave", menuName = "Data/New Cave")]
public class Cave : ScriptableObject
{
    public int caveId;
    public string caveName;
    public Color caveColor;
    public string fireInfo;
    public int caveGoal;
    public int sourceGoal;
}
