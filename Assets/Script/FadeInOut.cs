using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    Image _panel;
    [SerializeField]
    UnityEvent _onStart;
    [SerializeField]
    string _name;
    private void Start()
    {
        _panel = GetComponent<Image>();
        gameObject.SetActive(false);
        _onStart.Invoke();
    }
    public void FadeIn(float duration)
    {
        gameObject.SetActive(true);
        var c = new Color(0, 0, 0, 0);
        _panel.color = c;
        _panel.DOFade(1, duration).
            OnComplete(() => SceneManager.LoadScene(_name));
    }
    public void FadeOut(float duration)
    {
        gameObject.SetActive(true);
        var c = new Color(0, 0, 0, 1);
        _panel.color = c;
        _panel.DOFade(0, duration).
            OnComplete(() => gameObject.SetActive(false));
    }
}
