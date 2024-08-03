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
    [Header("シャッフルする速度")]
    public float _shuffleSpeed = 1f;
    [SerializeField, Header("元となるPrefab")]
    GameObject _block;
    [SerializeField, Header("アイテム毎の間隔")]
    float _padding;
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
    GameObject _interactionBlocker;
    void Start()
    {
        _interactionBlocker = GameObject.Find("Blocker");
        BeginShuffle();
    }
    public void BeginShuffle()
    {
        _interactionBlocker.SetActive(true);
        _count = 0;
        if (_objectArray == null)
        {
            _objectArray = new GameObject[_width, _height];
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _objectArray[i, j] = Instantiate(_block, transform);
                    _objectArray[i, j].transform.localPosition = new Vector3(i * _padding - (_width - 1) * _padding / 2, j * _padding - (_height - 1) * _padding / 2);
                }
            }
        }
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _objectArray[i, j].name = $"FakeArrow_{i}_{j}";
                _objectArray[i, j].GetComponent<ArrowInitializer>().Status = ArrowInitializer.ButtonStatus.Wrong;
            }
        }
        if (!_answer)
        {
            _answer = _objectArray[Random.Range(0, _width - 1), Random.Range(0, _height - 1)];
            _answer.name = "Answer";
            _answer.GetComponent<ArrowInitializer>().Status = ArrowInitializer.ButtonStatus.Correct;
        }
        _answer.GetComponent<Animator>().SetTrigger("Pulse");
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
        _interactionBlocker.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Separate2x2RotateClockwise();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            HalfChange();
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
            Reverse();
        }
    }
    public void Shuffle()
    {
        Debug.Log("Shuffle");
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                var rect = _objectArray[i, j].GetComponent<RectTransform>();
                _tweens.Add(rect.DOAnchorPos(-rect.anchoredPosition, _shuffleSpeed * 2f).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
            }
        }
    }

    public void Rotate()
    {
        Debug.Log("Rotate");
        int rand = Random.Range(0, 2) == 0 ? -1 : 1;
        _tweens.Add(transform.DORotate(180 * rand * Vector3.forward, _shuffleSpeed * 2, RotateMode.LocalAxisAdd).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _tweens.Add(_objectArray[i, j].transform.DOLocalRotate(180 * -rand * Vector3.forward, _shuffleSpeed * 2, RotateMode.LocalAxisAdd).
                    SetEase(_easing));
            }
        }
    }
    public void Reverse()
    {
        string axis = (Random.Range(0, 2) == 0) ? "X" : "Y";
        Debug.Log("Reverse" + axis);
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                var rect = _objectArray[i, j].GetComponent<RectTransform>();
                if (axis == "X")
                {
                    _tweens.Add(rect.DOAnchorPos(new Vector2(-rect.anchoredPosition.x, rect.anchoredPosition.y), _shuffleSpeed).
                        SetEase(_easing).
                        OnComplete(() => _isShuffling = false));
                }
                else
                {
                    _tweens.Add(rect.DOAnchorPos(new Vector2(rect.anchoredPosition.x, -rect.anchoredPosition.y), _shuffleSpeed).
                        SetEase(_easing).
                        OnComplete(() => _isShuffling = false));
                }
            }
        }
    }
    public void HalfChange()
    {
        string axis = (Random.Range(0, 2) == 0) ? "X" : "Y";
        Debug.Log("HalfChange" + axis);
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                int indexOfX = i, indexOfY = j;
                if (axis == "X")
                {
                    indexOfX = (i < _width / 2) ? i + _width / 2 : i - _width / 2;
                }
                else
                {
                    indexOfY = (j < _height / 2) ? j + _height / 2 : j - _height / 2;
                }
                _tweens.Add(_objectArray[i, j].transform.DOMove(_objectArray[indexOfX, indexOfY].transform.position, _shuffleSpeed).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
            }
        }
    }
    public void Separate2x2RotateClockwise()
    {
        Debug.Log("Separate2x2RotateClockwise");
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
