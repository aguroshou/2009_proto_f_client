using UnityEngine.UI;
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ポップに押されるボタン
/// </summary>
public class PopButton : MonoBehaviour
{
    Tweener tweener = null;
    private Action _onButtonUpAction;

    public void Init(Action onButtonUpAction)
    {
        _onButtonUpAction = onButtonUpAction;
        if (tweener != null)
        {
            tweener.Kill();
            tweener = null;
            transform.localScale = Vector3.one;
        }
    }

    public void OnButtonDownAction()
    {
        tweener = transform.DOPunchScale(
            punch: Vector3.one * 0.1f,
            duration: 0.2f,
            vibrato: 1
        ).SetEase(Ease.OutExpo);
    }

    public void OnButtonUpAction()
    {
        _onButtonUpAction();
    }
}