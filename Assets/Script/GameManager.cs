using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// �X�R�A�E���g���C�@�\�̊Ǘ����s���B
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; set => _instance = value; }

    /// <summary>�����񐔂��L�^�B���s���A0�ɖ߂�B</summary>
    int _score;
    /// <summary>�X�R�A��UI��ɕ\��������ׂ�Text�B</summary>
    [SerializeField]
    Text _scoreText;
    /// <summary>��ʑS�̂ɃG�t�F�N�g��������Animator�B���딻����������邽�߁B</summary>
    [SerializeField]
    Animator _sceneEffector;
    /// <summary>�����V���b�t������X�N���v�g�BGameManager����V���b�t�����x�ƃV���b�t���񐔂�ς��Ă���B</summary>
    Shuffler _shuffler;
    /// <summary>�V���b�t�����x�̏����l�B���s���A�V���b�t�����x�������l�ɖ߂����ߋL�^�B</summary>
    float _defaultShuffleSpeed;
    /// <summary>�V���b�t���񐔂̏����l�B���s���A�V���b�t���񐔂������l�ɖ߂����ߋL�^�B</summary>
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
    /// �X�R�A�����Z���A�V���b�t�����x���グ�A�V���b�t���񐔂�1���₷�B
    /// </summary>
    /// <param name="score">�X�R�A���ǂꂾ���������邩</param>
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
    /// �X�R�A��0�ɂ��A�V���b�t�����x�ƃV���b�t���񐔂������l�ɂ���B
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
    /// �Q�[����������Ԃɖ߂��B
    /// </summary>
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
