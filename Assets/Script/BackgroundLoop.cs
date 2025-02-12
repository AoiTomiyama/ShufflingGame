using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField] SpriteRenderer _backgroundSprite = null;
    [SerializeField] float _scrollSpeedY = -1f;
    /// <summary>背景をクローンしたものを入れておく変数</summary>
    SpriteRenderer _backgroundSpriteClone;
    /// <summary>背景座標の初期値</summary>
    float _initPosX;

    void Start()
    {
        _initPosX = _backgroundSprite.transform.position.x;   // 座標の初期値を保存しておく

        // 背景画像を複製して上に並べる
        _backgroundSpriteClone = Instantiate(_backgroundSprite);
        _backgroundSpriteClone.transform.Translate(_backgroundSprite.bounds.size.x, 0f, 0f);
    }

    void Update()
    {
        // 背景画像をスクロールする
        _backgroundSprite.transform.Translate(_scrollSpeedY * Time.deltaTime, 0f, 0f);
        _backgroundSpriteClone.transform.Translate(_scrollSpeedY * Time.deltaTime, 0f, 0f);

        // 背景画像がある程度下に降りたら、上に戻す
        if (_backgroundSprite.transform.position.x < _initPosX - _backgroundSprite.bounds.size.x)
        {
            _backgroundSprite.transform.Translate(_backgroundSprite.bounds.size.x * 2, 0f, 0f);
        }

        // 背景画像のクローンがある程度下に降りたら、上に戻す
        if (_backgroundSpriteClone.transform.position.x < _initPosX - _backgroundSpriteClone.bounds.size.x)
        {
            _backgroundSpriteClone.transform.Translate(_backgroundSpriteClone.bounds.size.x * 2, 0f, 0f);
        }
    }
}
