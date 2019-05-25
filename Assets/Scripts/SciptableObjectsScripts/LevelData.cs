using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObject/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    //public string objectName = "New MyScriptableObject";
    public List<MapDuoData> subLevelList;
}