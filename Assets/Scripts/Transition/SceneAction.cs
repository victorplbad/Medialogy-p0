using System;
using UnityEngine;

[Serializable]
public class SceneAction
{
    public enum ActionType
    {
        Load,
        Unload
    }

    public SceneField Scene;
    public ActionType Action;
}
