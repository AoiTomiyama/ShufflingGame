using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shuffler : MonoBehaviour
{
    [SerializeField, Header("横幅")]
    int _width = 4;
    [SerializeField, Header("縦幅")]
    int _height = 4;
    [SerializeField, Header("シャッフルする回数")]
    int _shuffleCount;
    [SerializeField, Header("元となるPrefab")]
    GameObject _block;
    [SerializeField, Header("アイテム毎の間隔")]
    float _padding;
    [SerializeField, Header("シャッフルする速度")]
    float _shuffleSpeed = 0.5f;
    [SerializeField, Header("シャッフル時の動き")]
    Ease _easing = Ease.InSine;
    [SerializeField, Header("シャッフルパターン")]
    List<UnityEvent> _actions;
    List<Tween> _tweens = new();
    bool _isShuffling;
    int _count;
    float _fadeTime = 1.1f;
    GameObject[,] _objectArray;
    GameObject _answer;

    // Start is called before the first frame update
    void Start()
    {
        BeginShuffle();
    }
    void BeginShuffle()
    {
        _count = 0;
        _shuffleSpeed -= 0.07f;
        if (_objectArray == null)
        {
            _objectArray = new GameObject[_width, _height];
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _objectArray[i, j] = Instantiate(_block, FindObjectOfType<Canvas>().transform);
                    //_objectArray[i, j].GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                    _objectArray[i, j].transform.localPosition = new Vector3(i * _padding - (_width - 1) * _padding / 2, j * _padding - (_height - 1) * _padding / 2);
                }
            }
        }
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _objectArray[i, j].name = $"FakeArrow_{i}_{j}";
            }
        }
        _answer = _objectArray[Random.Range(0, _width - 1), Random.Range(0, _height - 1)];
        _answer.name = "Answer";
        _tweens.Add(_answer.GetComponent<Image>().DOColor(Color.red, _fadeTime).OnComplete(() =>
        {
            _tweens.Add(_answer.GetComponent<Image>().DOColor(Color.white, _fadeTime));
        }));
        StartCoroutine(Choose());
    }
    IEnumerator Choose()
    {
        yield return new WaitForSeconds(_fadeTime * 2);
        while (true)
        {
            while (_isShuffling) yield return null;
            _isShuffling = true;
            _count++;
            if (_count > _shuffleCount)
            {
                _isShuffling = false;
                break;
            }
            _actions[Random.Range(0, _actions.Count)].Invoke();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Separate2x2RotateClockwise();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            HalfChangeX();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            HalfChangeY();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Shuffle();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Rotate();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SwapX();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SwapY();
        }
    }
    public void Shuffle()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _tweens.Add(_objectArray[i, j].transform.DOMove(-_objectArray[i, j].transform.position, _shuffleSpeed * 1.5f).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
            }
        }
    }

    public void Rotate()
    {
        int rand = Random.Range(0, 2) == 0 ? -1 : 1;
        _tweens.Add(transform.DORotate(Vector3.forward * 180 * rand, _shuffleSpeed * 2, RotateMode.LocalAxisAdd).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _tweens.Add(_objectArray[i, j].transform.DOLocalRotate(Vector3.forward * 180 * -rand, _shuffleSpeed * 2, RotateMode.LocalAxisAdd).
                    SetEase(_easing));
            }
        }
    }
    public void SwapX()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _tweens.Add(_objectArray[i, j].transform.DOMove(new Vector2(-_objectArray[i, j].transform.position.x, _objectArray[i, j].transform.position.y), _shuffleSpeed).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
            }
        }
    }
    public void SwapY()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _tweens.Add(_objectArray[i, j].transform.DOMove(new Vector2(_objectArray[i, j].transform.position.x, -_objectArray[i, j].transform.position.y), _shuffleSpeed).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
            }
        }
    }
    public void HalfChangeX()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                int indexOfX = (i < _width / 2) ? i + _width / 2 : i - _width / 2;
                _tweens.Add(_objectArray[i, j].transform.DOMove(_objectArray[indexOfX, j].transform.position, _shuffleSpeed).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
            }
        }
    }
    public void HalfChangeY()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                int indexOfY = (j < _height / 2) ? j + _height / 2 : j - _height / 2;
                _tweens.Add(_objectArray[i, j].transform.DOMove(_objectArray[i, indexOfY].transform.position, _shuffleSpeed).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
            }
        }
    }
    public void Separate2x2RotateClockwise()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                int indexOfX = i, indexOfY = j;
                if ((i + 1) % 2 == 0)
                {
                    if ((j + 1) % 2 == 0)
                    {
                        //横＝偶数かつ縦＝偶数の時、下に動かす
                        indexOfY = j - 1;
                    }
                    else
                    {
                        //横＝偶数かつ縦＝奇数の時、左に動かす
                        indexOfX = i - 1;
                    }
                }
                else
                {
                    if ((j + 1) % 2 == 0)
                    {
                        //横＝奇数かつ縦＝偶数の時、右に動かす
                        indexOfX = i + 1;
                    }
                    else
                    {
                        //横＝奇数かつ縦＝奇数の時、上に動かす
                        indexOfY = j + 1;
                    }
                }
                _tweens.Add(_objectArray[i, j].transform.DOMove(_objectArray[indexOfX, indexOfY].transform.position, _shuffleSpeed).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
            }
        }

    }
    private void OnDisable()
    {
        if (_tweens.Count > 0)
        {
            foreach (var t in _tweens)
            {
                t.Kill();
            }
        }
    }
}
