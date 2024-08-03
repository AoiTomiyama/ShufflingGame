using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEllipse : MonoBehaviour
{
    float _theta;
    [SerializeField]
    float _radiusX;
    [SerializeField]
    float _radiusY;
    [SerializeField]
    float _speed;
    [SerializeField]
    GameObject _go;
    [SerializeField]
    int _count;
    GameObject[] _gos;
    private void Start()
    {
        _gos = new GameObject[_count];
        for (int i = 0; i < _count; i++)
        {
            _gos[i] = Instantiate(_go, transform);
        }
    }
    void Update()
    {
        _theta += Time.deltaTime * _speed;
        for (int i = 0; i < _count; i++)
        {
            _gos[i].transform.position = new Vector2(
                Mathf.Cos(Mathf.Deg2Rad * (_theta + i * 360 / _count)) * _radiusX * Mathf.Sin(_theta / _speed),
                Mathf.Sin(Mathf.Deg2Rad * (_theta + i * 360 / _count)) * _radiusY * -Mathf.Cos(_theta / _speed)
                );
        }
    }
}
