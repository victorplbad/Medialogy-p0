using System;
using UnityEngine;

public class TransitionToMenu : MonoBehaviour
{
    [SerializeField] private SceneField _sceneToLoad;

    public event Action<SceneField> OnMenuChange;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMenu();
        }
    }

    public void GoToMenu()
    {
        OnMenuChange?.Invoke(_sceneToLoad);
    }
}
