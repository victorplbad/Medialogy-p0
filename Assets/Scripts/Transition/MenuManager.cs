using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private SceneField _currentScene;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_currentScene == null)
        {
            Debug.LogWarning("No initial scene set in MenuManager.");
            return;
        }

        LoadMenuButtons();
    }

    public void HandleMenuChange(SceneField sceneToLoad)
    {
        if (_currentScene == sceneToLoad)
            return;

        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    public IEnumerator LoadSceneAsync(SceneField sceneToLoad)
    {
        Debug.Log("Loading Scene: " + sceneToLoad);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        yield return new WaitUntil(() => asyncLoad.isDone);

        asyncLoad.completed += (AsyncOperation obj) =>
        {
            Debug.Log("Scene Loaded: " + sceneToLoad);
        };

        if (_currentScene != null)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            StartCoroutine(UnloadSceneAsync(_currentScene));
        }

        LoadMenuButtons();
        _currentScene = sceneToLoad;
    }

    public IEnumerator UnloadSceneAsync(SceneField sceneToUnload)
    {
        Debug.Log("Unloading Scene: " + sceneToUnload);

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneToUnload);

        yield return new WaitUntil(() => asyncUnload.isDone);

        asyncUnload.completed += (AsyncOperation obj) =>
        {
            Debug.Log("Scene Unloaded: " + sceneToUnload);
        };
    }

    public void LoadMenuButtons()
    {
        TransitionToMenu[] transitionObjects = FindObjectsByType<TransitionToMenu>(FindObjectsSortMode.None);

        if (transitionObjects.Length < 1)
        {
            Debug.LogWarning("No menu buttons found in the scene.");
            return;
        }

        foreach (var transition in transitionObjects)
        {
            transition.OnMenuChange += HandleMenuChange;
        }

        Debug.Log("Menu buttons loaded.");
    }
}
