using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public void SetDifficulty(int diff)
    {
        GameManager._difficulty = diff;
    }
}
