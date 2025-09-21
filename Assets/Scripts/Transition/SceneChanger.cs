using UnityEngine;
using System;

public class SceneChanger : MonoBehaviour
{
    [Header("Scene Actions to Perform")]
    [SerializeField] private protected SceneAction[] _sceneActions;

    public event Action<SceneAction[]> OnSceneActionRequest;

    public void RequestSceneAction()
    {
        OnSceneActionRequest?.Invoke(_sceneActions);
    }
}
