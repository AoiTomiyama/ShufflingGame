using UnityEngine;

public class ArrowInitializer : MonoBehaviour
{
    ButtonStatus _status;
    public ButtonStatus Status { get => _status; set => _status = value; }

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
