using UnityEngine;

public class AnimatorSetIntegerAction : AnimatorAction
{
    [SerializeField] private int value;

    public override void CallAction(Animator animator) => animator.SetInteger(PropertyHash, value);
}
