using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SpriteColorChange : MonoBehaviour
{
    [SerializeField]
    Gradient _gradient;
    [SerializeField]
    float _speed;
    float _currentValue;
    SpriteRenderer _sr;
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        _currentValue += Time.deltaTime * _speed;
        _sr.color = _gradient.Evaluate(Mathf.PingPong(_currentValue, 1f));
    }
}
