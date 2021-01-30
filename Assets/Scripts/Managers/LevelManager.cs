using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public int currentlySelectedLevel = 1;
    public void SetSelectedLevel(int levelNumber)
    {
        currentlySelectedLevel = levelNumber;
    }
}
