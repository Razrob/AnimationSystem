using DG.Tweening;
using UnityEngine;

public class AnimatorFloatInterpolateAction : AnimatorAction
{
    [SerializeField] private float targetValue;
    [SerializeField] private float time;
    [SerializeField] private AnimationCurve curve;

    public override void CallAction(Animator animator) =>
        DOTween.To(() => animator.GetFloat(PropertyHash), intermediateValue => animator.SetFloat(PropertyHash, intermediateValue), targetValue, time)
        .SetEase(curve);
}
