using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformOffcetMoveToAction : TransformAction
{
    [SerializeField] private Vector3 targetOffcet;
    [SerializeField] private float time;
    [SerializeField] private AnimationCurve curve;

    private Vector3 lastOffcet;

    public override void CallAction(Transform transform)
    {
        Vector3 startOffcet = Vector3.zero;

        DOTween.To(() => startOffcet, newOffcet => startOffcet = IterateInterpolation(transform, newOffcet), targetOffcet, time)
        .SetEase(curve).OnComplete(() => lastOffcet = default);
    }

    private Vector3 IterateInterpolation(Transform transform, Vector3 newOffcet)
    {
        transform.position -= lastOffcet;
        transform.position += newOffcet;
        lastOffcet = newOffcet;

        return newOffcet;
    }
}
