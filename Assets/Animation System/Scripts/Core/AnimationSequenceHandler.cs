using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSequenceHandler
{
    public static void PlaySequence(AnimationSequence animationSequence, Transform transform)
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < animationSequence.States.Length; i++)
            AddSequenceElements(sequence, transform, animationSequence.States[i]);
        sequence.Play();
    }

    private static void AddSequenceElements(Sequence sequence, Transform transform, SequenceState animationState)
    {
        sequence.AppendInterval(animationState.StateDelay);

        foreach (AnimationActionBase action in animationState.AnimationActions)
            sequence.InsertCallback(sequence.Duration() + animationState.StateDuration * action.CallTimeOffcet, 
                () => action.CallAction(transform));

        sequence.AppendInterval(animationState.StateDuration);
    }
}