using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSequenceHandler
{
    public static void PlaySequence(AnimationSequence animationSequence, Animator animator)
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < animationSequence.States.Length; i++)
            AddSequenceElements(sequence, animator, animationSequence.States[i]);
        sequence.Play();
    }

    private static void AddSequenceElements(Sequence sequence, Animator animator, SequenceState animationState)
    {
        foreach (AnimationActionBase action in animationState.AnimationActions)
            sequence.InsertCallback(sequence.Duration() + animationState.StateDuration * action.CallTimeOffcet, 
                () => action.CallAction(animator));

        sequence.AppendInterval(animationState.StateDuration);
    }
}