using UnityEngine;

public class AnimatorSetBoolAction : AnimatorAction
{
    [SerializeField] private bool value;

    public override void CallAction(Animator animator) => animator.SetBool(PropertyHash, value);
}
