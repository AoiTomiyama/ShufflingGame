using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEllipse : MonoBehaviour
{
    [SerializeField] float _radiusX;
    [SerializeField] float _radiusY;
    [SerializeField] float _rotateX;
    [SerializeField] float _rotateY;
    [SerializeField] float _speed;
    [SerializeField] GameObject _go;
    [SerializeField] int _count;
    float _theta;
    float _phi;
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
        _phi += Time.deltaTime;
        for (int i = 0; i < _count; i++)
        {
            _gos[i].transform.position = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * (_theta + i * 360 / _count)) * _radiusX * Mathf.Sin(_phi * _rotateX) + transform.position.x,
                Mathf.Sin(Mathf.Deg2Rad * (_theta + i * 360 / _count)) * _radiusY * Mathf.Cos(_phi * _rotateY) + transform.position.y,
                0
                );
        }
    }
}
