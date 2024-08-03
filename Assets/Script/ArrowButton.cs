using UnityEngine;

/// <summary>
/// 正誤判定をするスクリプト。
/// </summary>
public class ArrowButton : MonoBehaviour
{
    /// <summary>
    /// 矢印の正誤状態。
    /// </summary>
    ButtonStatus _status;
    public ButtonStatus Status { get => _status; set => _status = value; }
    /// <summary>
    /// 矢印がクリックされた時、自身の正誤状態に応じて正解か不正解の処理をGameManagerに要求する。
    /// </summary>
    public void OnClicked()
    {
        if (_status == ButtonStatus.Correct)
        {
            GameManager.Instance.AddScore(1);
        }
        else
        {
            GameManager.Instance.ResetScore();
        }
    }
    public enum ButtonStatus
    {
        Wrong,
        Correct
    }
}
