using UnityEngine;

/// <summary>
/// ���딻�������X�N���v�g�B
/// </summary>
public class ArrowButton : MonoBehaviour
{
    /// <summary>
    /// ���̐����ԁB
    /// </summary>
    ButtonStatus _status;
    public ButtonStatus Status { get => _status; set => _status = value; }
    /// <summary>
    /// ��󂪃N���b�N���ꂽ���A���g�̐����Ԃɉ����Đ������s�����̏�����GameManager�ɗv������B
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
