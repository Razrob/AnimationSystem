using DG.Tweening;
using UnityEngine;

public class AnimatorIntegerInterpolateAction : AnimatorAction
{
    [SerializeField] private int targetValue;
    [SerializeField] private float time;
    [SerializeField] private AnimationCurve curve;

    public override void CallAction(Animator animator) =>
        DOTween.To(() => animator.GetInteger(PropertyHash), intermediateValue => animator.SetInteger(PropertyHash, intermediateValue), targetValue, time)
        .SetEase(curve);
}
