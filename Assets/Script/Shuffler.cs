using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
/// <summary>
/// 矢印の生成・整列・シャッフルを行う。
/// </summary>

public class Shuffler : MonoBehaviour
{
    [SerializeField, Header("横幅")]
    int _width = 4;
    [SerializeField, Header("縦幅")]
    int _height = 4;
    [SerializeField, Header("シャッフルする回数")]
    int _shuffleCount;
    [Header("シャッフルする速度")]
    float _shuffleSpeed = 1f;
    [SerializeField, Header("元となるPrefab")]
    GameObject _block;
    [SerializeField, Header("アイテム毎の間隔")]
    float _padding;
    [SerializeField, Header("シャッフル時の動き")]
    Ease _easing = Ease.InSine;
    [SerializeField, Header("シャッフルパターン")]
    List<UnityEvent> _actions;
    /// <summary>Tween再生中にシーンをリロードした際に出る警告を抑制する用。</summary>
    List<Tween> _tweens = new();
    /// <summary>
    /// シャッフルしているかどうか。
    /// OnCompleteにそのまま次のメソッドを書くと実行したときに警告が出る上に極度に重くなるためそれの回避用。
    /// </summary>
    bool _isShuffling;
    /// <summary>実行済のシャッフル回数。</summary>
    int _count;
    /// <summary>生成した矢印を保存する。</summary>
    GameObject[,] _objectArray;
    /// <summary>答えとなる矢印。</summary>
    GameObject _answer;
    /// <summary>シャッフル中に矢印を押させないためのブロッカー。</summary>
    GameObject _interactionBlocker;

    public float ShuffleSpeed { get => _shuffleSpeed; set => _shuffleSpeed = value; }
    public int ShuffleCount { get => _shuffleCount; set => _shuffleCount = value; }

    void Start()
    {
        SetUp();
    }
    /// <summary>
    /// 開始時に矢印の配置を行う。完了したらBeginShuffle()でシャッフルを開始させる。
    /// </summary>
    void SetUp()
    {
        _interactionBlocker = GameObject.Find("Blocker");
        _objectArray = new GameObject[_width, _height];
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _objectArray[i, j] = Instantiate(_block, transform);
                _objectArray[i, j].transform.localPosition = new Vector3(i * _padding - (_width - 1) * _padding / 2, j * _padding - (_height - 1) * _padding / 2);
            }
        }

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _objectArray[i, j].name = $"FakeArrow_{i}_{j}";
                _objectArray[i, j].GetComponent<ArrowButton>().Status = ArrowButton.ButtonStatus.Wrong;
            }
        }
        _answer = _objectArray[Random.Range(0, _width - 1), Random.Range(0, _height - 1)];
        _answer.name = "Answer";
        _answer.GetComponent<ArrowButton>().Status = ArrowButton.ButtonStatus.Correct;
        BeginShuffle();
    }
    /// <summary>
    /// シャッフルを開始する。
    /// </summary>
    public void BeginShuffle()
    {
        _interactionBlocker.SetActive(true);
        _answer.GetComponent<Animator>().SetTrigger("Pulse");
        StartCoroutine(Choose());
    }
    /// <summary>
    /// _shuffleCountの数、シャッフルのパターンをランダムに抽選する。
    /// </summary>
    IEnumerator Choose()
    {
        yield return new WaitForSeconds(2.5f);
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
        _count = 0;
        _interactionBlocker.SetActive(false);
    }
    /// <summary>
    /// 全体を回転させるパターン。時計回りか反時計回りかはランダムで選ぶ。
    /// </summary>
    public void Rotate()
    {
        Debug.Log("Rotate");
        int rand = Random.Range(0, 2) == 0 ? -1 : 1;
        _tweens.Add(transform.DORotate(180 * rand * Vector3.forward, ShuffleSpeed * 2, RotateMode.LocalAxisAdd).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _tweens.Add(_objectArray[i, j].transform.DOLocalRotate(180 * -rand * Vector3.forward, ShuffleSpeed * 2, RotateMode.LocalAxisAdd).
                    SetEase(_easing));
            }
        }
    }
    /// <summary>
    /// 全体を線対象に反転させるパターン。X軸Y軸はランダムで選ぶ。
    /// </summary>
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
                    _tweens.Add(rect.DOAnchorPos(new Vector2(-rect.anchoredPosition.x, rect.anchoredPosition.y), ShuffleSpeed).
                        SetEase(_easing).
                        OnComplete(() => _isShuffling = false));
                }
                else
                {
                    _tweens.Add(rect.DOAnchorPos(new Vector2(rect.anchoredPosition.x, -rect.anchoredPosition.y), ShuffleSpeed).
                        SetEase(_easing).
                        OnComplete(() => _isShuffling = false));
                }
            }
        }
    }
    /// <summary>
    /// 全体を半分に分けて、入れ替えるパターン。X軸Y軸はランダムで選ぶ。
    /// </summary>
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
                _tweens.Add(_objectArray[i, j].transform.DOMove(_objectArray[indexOfX, indexOfY].transform.position, ShuffleSpeed).
                    SetEase(_easing).
                    OnComplete(() => _isShuffling = false));
            }
        }
    }
    /// <summary>
    /// 全体を四分割し、それぞれの区画を時計回りに回転させるパターン。
    /// </summary>
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
                _tweens.Add(_objectArray[i, j].transform.DOMove(_objectArray[indexOfX, indexOfY].transform.position, ShuffleSpeed).
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
