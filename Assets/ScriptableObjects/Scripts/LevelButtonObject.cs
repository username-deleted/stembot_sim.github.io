using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelButton", menuName = "ScriptableObjects/Menu/Buttons/LevelButton", order = 1)]
public class LevelButtonObject : ScriptableObject
{
    public Sprite levelImage;
    public int levelNumber;
}
