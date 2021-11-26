using UnityEngine;

public class SceneAction : AnimationActionBase
{
    [SerializeField] private string actionIdentifier;

    public override void CallAction(Animator animator) => 
        Singleton<SceneActionsRepository>.Instance.CallAction(actionIdentifier);
}
