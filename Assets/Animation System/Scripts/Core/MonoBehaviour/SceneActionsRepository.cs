using System.Collections.Generic;
using UnityEngine;

public class SceneActionsRepository : MonoBehaviour
{
    private List<AnimationSceneActionBase> sceneActions = new List<AnimationSceneActionBase>();
    
    public void AddSceneAction(AnimationSceneActionBase sceneActionBase)
    {
        if (sceneActionBase != null) sceneActions.Add(sceneActionBase);
    }

    public void CallAction(string actionID) =>
        sceneActions.Find(action => action.ActionID == actionID).CallAction();
}
