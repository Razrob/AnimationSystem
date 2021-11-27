using UnityEngine;

public class SceneAction : AnimationActionBase
{
    [SerializeField] private string actionIdentifier;

    public override void CallAction(Transform transform) => 
        Singleton<SceneActionsRepository>.Instance.CallAction(actionIdentifier);
}
