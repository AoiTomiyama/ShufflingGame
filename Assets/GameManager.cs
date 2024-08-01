using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; set => _instance = value; }
    int _score;
    [SerializeField]
    Text _scoreText;
    Shuffler _shuffler;
    float _defaultShuffleSpeed;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        _shuffler = FindObjectOfType<Shuffler>();
        _defaultShuffleSpeed = _shuffler._shuffleSpeed;
    }
    public void AddScore(int score)
    {
        _scoreText.DOCounter(_score, _score + score, 1);
        _score += score;
        _shuffler._shuffleSpeed -= 0.06f;
        _shuffler.BeginShuffle();
    }
    public void ResetScore()
    {
        _scoreText.DOCounter(_score, 0, 1);
        _score = 0;
        _shuffler._shuffleSpeed = _defaultShuffleSpeed;
        _shuffler.BeginShuffle();
    }
}
