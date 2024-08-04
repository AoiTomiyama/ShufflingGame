using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DistractionController : MonoBehaviour
{
    [SerializeField] UnityEvent _onHardMode;
    void Start()
    {
        if (GameManager._difficulty == 1)
        {
            _onHardMode.Invoke();
        }
    }
}
