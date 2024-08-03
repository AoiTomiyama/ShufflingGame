using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ゲーム画面全体にかけているエフェクトが再生完了した際に、AnimationEventを経由してUnityEventを呼ぶだけ。
/// AnimationEventはAnimationControllerがアタッチされているオブジェクトのメソッドしか呼べないためこうしている。
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
