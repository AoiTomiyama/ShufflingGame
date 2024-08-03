using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// �Q�[����ʑS�̂ɂ����Ă���G�t�F�N�g���Đ����������ۂɁAAnimationEvent���o�R����UnityEvent���ĂԂ����B
/// AnimationEvent��AnimationController���A�^�b�`����Ă���I�u�W�F�N�g�̃��\�b�h�����ĂׂȂ����߂������Ă���B
/// </summary>

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    UnityEvent _onEffectComplete;
    public void OnEffectComplete()
    {
        _onEffectComplete.Invoke();
    }
}
