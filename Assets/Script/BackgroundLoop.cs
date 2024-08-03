using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField] SpriteRenderer _backgroundSprite = null;
    [SerializeField] float _scrollSpeedY = -1f;
    /// <summary>”wŒi‚ğƒNƒ[ƒ“‚µ‚½‚à‚Ì‚ğ“ü‚ê‚Ä‚¨‚­•Ï”</summary>
    SpriteRenderer _backgroundSpriteClone;
    /// <summary>”wŒiÀ•W‚Ì‰Šú’l</summary>
    float _initPosX;

    void Start()
    {
        _initPosX = _backgroundSprite.transform.position.x;   // À•W‚Ì‰Šú’l‚ğ•Û‘¶‚µ‚Ä‚¨‚­

        // ”wŒi‰æ‘œ‚ğ•¡»‚µ‚Äã‚É•À‚×‚é
        _backgroundSpriteClone = Instantiate(_backgroundSprite);
        _backgroundSpriteClone.transform.Translate(_backgroundSprite.bounds.size.x, 0f, 0f);
    }

    void Update()
    {
        // ”wŒi‰æ‘œ‚ğƒXƒNƒ[ƒ‹‚·‚é
        _backgroundSprite.transform.Translate(_scrollSpeedY * Time.deltaTime, 0f, 0f);
        _backgroundSpriteClone.transform.Translate(_scrollSpeedY * Time.deltaTime, 0f, 0f);

        // ”wŒi‰æ‘œ‚ª‚ ‚é’ö“x‰º‚É~‚è‚½‚çAã‚É–ß‚·
        if (_backgroundSprite.transform.position.x < _initPosX - _backgroundSprite.bounds.size.x)
        {
            _backgroundSprite.transform.Translate(_backgroundSprite.bounds.size.x * 2, 0f, 0f);
        }

        // ”wŒi‰æ‘œ‚ÌƒNƒ[ƒ“‚ª‚ ‚é’ö“x‰º‚É~‚è‚½‚çAã‚É–ß‚·
        if (_backgroundSpriteClone.transform.position.x < _initPosX - _backgroundSpriteClone.bounds.size.x)
        {
            _backgroundSpriteClone.transform.Translate(_backgroundSpriteClone.bounds.size.x * 2, 0f, 0f);
        }
    }
}
