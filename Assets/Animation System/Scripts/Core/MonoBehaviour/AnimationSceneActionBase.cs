using UnityEngine;

public abstract class AnimationSceneActionBase : MonoBehaviour
{
    public abstract string ActionID { get; protected set; }

    private void Awake() =>
        Singleton<SceneActionsRepository>.Instance.AddSceneAction(this);

    public abstract void CallAction();
}
