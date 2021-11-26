using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Animation system/Create animation sequence")]
public class AnimationSequence : ScriptableObject
{
    public SequenceState[] States;
}

[Serializable]
public class SequenceState
{
    public AnimationActionBase[] AnimationActions;
    public float StateDuration;
}