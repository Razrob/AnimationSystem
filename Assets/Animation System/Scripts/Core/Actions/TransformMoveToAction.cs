using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Animation system/Create TrasnsformMoveTo action")]
public class TransformMoveToAction : TransformAction
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float time;
    [SerializeField] private AnimationCurve curve;

    public override void CallAction(Transform transform) =>
        DOTween.To(() => transform.position, newPosition => transform.position = newPosition, targetPosition, time)
        .SetEase(curve);
}
