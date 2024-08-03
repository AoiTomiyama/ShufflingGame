using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// スコア・リトライ機能の管理を行う。
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; set => _instance = value; }

    /// <summary>正解回数を記録。失敗時、0に戻る。</summary>
    int _score;
    /// <summary>スコアをUI上に表示させる為のText。</summary>
    [SerializeField]
    Text _scoreText;
    /// <summary>画面全体にエフェクトをかけるAnimator。正誤判定を可視化するため。</summary>
    [SerializeField]
    Animator _sceneEffector;
    /// <summary>矢印をシャッフルするスクリプト。GameManagerからシャッフル速度とシャッフル回数を変えている。</summary>
    Shuffler _shuffler;
    /// <summary>シャッフル速度の初期値。失敗時、シャッフル速度を初期値に戻すため記録。</summary>
    float _defaultShuffleSpeed;
    /// <summary>シャッフル回数の初期値。失敗時、シャッフル回数を初期値に戻すため記録。</summary>
    int _defaultShuffleCount;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        _shuffler = FindObjectOfType<Shuffler>();
        _defaultShuffleSpeed = _shuffler.ShuffleSpeed;
        _defaultShuffleCount = _shuffler.ShuffleCount;
    }
    /// <summary>
    /// スコアを加算し、シャッフル速度を上げ、シャッフル回数を1増やす。
    /// </summary>
    /// <param name="score">スコアがどれだけ増加するか</param>
    public void AddScore(int score)
    {
        const float shuffleSpeedReduction = 0.05f;
        _scoreText.DOCounter(_score, _score + score, 1);
        _score += score;
        _shuffler.ShuffleSpeed -= shuffleSpeedReduction;
        _shuffler.ShuffleCount++;
        _sceneEffector.Play("Correct");
    }
    /// <summary>
    /// スコアを0にし、シャッフル速度とシャッフル回数を初期値にする。
    /// </summary>
    public void ResetScore()
    {
        _scoreText.DOCounter(_score, 0, 1);
        _score = 0;
        _shuffler.ShuffleSpeed = _defaultShuffleSpeed;
        _shuffler.ShuffleCount = _defaultShuffleCount;
        _sceneEffector.Play("Mistake");
    }
    /// <summary>
    /// ゲームを初期状態に戻す。
    /// </summary>
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
