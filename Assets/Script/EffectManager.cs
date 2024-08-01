using UnityEngine;
using UnityEngine.Events;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    UnityEvent _onEffectComplete;
    public void OnEffectComplete()
    {
        _onEffectComplete.Invoke();
    }
}
