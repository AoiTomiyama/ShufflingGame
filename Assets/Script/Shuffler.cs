using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
/// <summary>
/// ���̐����E����E�V���b�t�����s���B
/// </summary>

public class Shuffler : MonoBehaviour
{
    [SerializeField, Header("����")]
    int _width = 4;
    [SerializeField, Header("�c��")]
    int _height = 4;
    [SerializeField, Header("�V���b�t�������")]
    int _shuffleCount;
    [Header("�V���b�t�����鑬�x")]
    float _shuffleSpeed = 1f;
    [SerializeField, Header("���ƂȂ�Prefab")]
    GameObject _block;
    [SerializeField, Header("�A�C�e�����̊Ԋu")]
    float _padding;
    [SerializeField, Header("�V���b�t�����̓���")]
    Ease _easing = Ease.InSine;
    [SerializeField, Header("�V���b�t���p�^�[��")]
    List<UnityEvent> _actions;
    /// <summary>Tween�Đ����ɃV�[���������[�h�����ۂɏo��x����}������p�B</summary>
    List<Tween> _tweens = new();
    /// <summary>
    /// �V���b�t�����Ă��邩�ǂ����B
    /// OnComplete�ɂ��̂܂܎��̃��\�b�h�������Ǝ��s�����Ƃ��Ɍx�����o���ɋɓx�ɏd���Ȃ邽�߂���̉��p�B
    /// </summary>
    bool _isShuffling;
    /// <summary>���s�ς̃V���b�t���񐔁B</summary>
    int _count;
    /// <summary>������������ۑ�����B</summary>
    GameObject[,] _objectArray;
    /// <summary>�����ƂȂ���B</summary>
    GameObject _answer;
    /// <summary>�V���b�t�����ɖ����������Ȃ����߂̃u���b�J�[�B</summary>
    GameObject _interactionBlocker;

    public float ShuffleSpeed { get => _shuffleSpeed; set => _shuffleSpeed = value; }
    public int ShuffleCount { get => _shuffleCount; set => _shuffleCount = value; }

    void Start()
    {
        SetUp();
    }
    /// <summary>
    /// �J�n���ɖ��̔z�u���s���B����������BeginShuffle()�ŃV���b�t�����J�n������B
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
    /// �V���b�t�����J�n����B
    /// </summary>
    public void BeginShuffle()
    {
        _interactionBlocker.SetActive(true);
        _answer.GetComponent<Animator>().SetTrigger("Pulse");
        StartCoroutine(Choose());
    }
    /// <summary>
    /// _shuffleCount�̐��A�V���b�t���̃p�^�[���������_���ɒ��I����B
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
    /// �S�̂���]������p�^�[���B���v��肩�����v��肩�̓����_���őI�ԁB
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
    /// �S�̂���Ώۂɔ��]������p�^�[���BX��Y���̓����_���őI�ԁB
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
    /// �S�̂𔼕��ɕ����āA����ւ���p�^�[���BX��Y���̓����_���őI�ԁB
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
    /// �S�̂��l�������A���ꂼ��̋������v���ɉ�]������p�^�[���B
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
                        //�����������c�������̎��A���ɓ�����
                        indexOfY = j - 1;
                    }
                    else
                    {
                        //�����������c����̎��A���ɓ�����
                        indexOfX = i - 1;
                    }
                }
                else
                {
                    if ((j + 1) % 2 == 0)
                    {
                        //��������c�������̎��A�E�ɓ�����
                        indexOfX = i + 1;
                    }
                    else
                    {
                        //��������c����̎��A��ɓ�����
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
