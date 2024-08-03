using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField] SpriteRenderer _backgroundSprite = null;
    [SerializeField] float _scrollSpeedY = -1f;
    /// <summary>�w�i���N���[���������̂����Ă����ϐ�</summary>
    SpriteRenderer _backgroundSpriteClone;
    /// <summary>�w�i���W�̏����l</summary>
    float _initPosX;

    void Start()
    {
        _initPosX = _backgroundSprite.transform.position.x;   // ���W�̏����l��ۑ����Ă���

        // �w�i�摜�𕡐����ď�ɕ��ׂ�
        _backgroundSpriteClone = Instantiate(_backgroundSprite);
        _backgroundSpriteClone.transform.Translate(_backgroundSprite.bounds.size.x, 0f, 0f);
    }

    void Update()
    {
        // �w�i�摜���X�N���[������
        _backgroundSprite.transform.Translate(_scrollSpeedY * Time.deltaTime, 0f, 0f);
        _backgroundSpriteClone.transform.Translate(_scrollSpeedY * Time.deltaTime, 0f, 0f);

        // �w�i�摜��������x���ɍ~�肽��A��ɖ߂�
        if (_backgroundSprite.transform.position.x < _initPosX - _backgroundSprite.bounds.size.x)
        {
            _backgroundSprite.transform.Translate(_backgroundSprite.bounds.size.x * 2, 0f, 0f);
        }

        // �w�i�摜�̃N���[����������x���ɍ~�肽��A��ɖ߂�
        if (_backgroundSpriteClone.transform.position.x < _initPosX - _backgroundSpriteClone.bounds.size.x)
        {
            _backgroundSpriteClone.transform.Translate(_backgroundSpriteClone.bounds.size.x * 2, 0f, 0f);
        }
    }
}
