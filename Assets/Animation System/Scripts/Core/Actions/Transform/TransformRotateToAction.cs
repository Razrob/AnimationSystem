using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRotateToAction : TransformAction
{
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float time;
    [SerializeField] private AnimationCurve curve;

    public override void CallAction(Transform transform) =>
        DOTween.To(() => transform.eulerAngles, newRotation => transform.eulerAngles = newRotation, targetRotation, time)
        .SetEase(curve);
}
