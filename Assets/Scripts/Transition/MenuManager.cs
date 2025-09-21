using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    private List<string> _loadedScenes = new List<string>();

    private void Awake()
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
    }

    private void Start()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && !_loadedScenes.Contains(scene.name))
            {
                _loadedScenes.Add(scene.name);
            }
        }

        UpdateLoadingEvents();
    }

    public IEnumerator HandleSceneActionRequest(SceneAction[] actions)
    {
        List<SceneField> scenesToLoad = new List<SceneField>();
        List<SceneField> scenesToUnload = new List<SceneField>();

        foreach (var action in actions)
        {
            if (action.Action == SceneAction.ActionType.Load && !_loadedScenes.Contains(action.Scene.SceneName))
            {
                scenesToLoad.Add(action.Scene);
            }
            else if (action.Action == SceneAction.ActionType.Unload && _loadedScenes.Contains(action.Scene.SceneName))
            {
                scenesToUnload.Add(action.Scene);
            }
        }

        foreach (var scene in scenesToLoad)
        {
            yield return StartCoroutine(LoadSceneAsync(scene));
        }

        //Play transition animation here

        foreach (var scene in scenesToUnload)
        {
            yield return StartCoroutine(UnloadSceneAsync(scene));
        }

        //Update Buttons Here
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

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad.SceneName));

        _loadedScenes.Add(sceneToLoad.SceneName);
    }

    public IEnumerator UnloadSceneAsync(SceneField sceneToUnload)
    {
        Debug.Log("Unloading Scene: " + sceneToUnload);

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneToUnload.SceneName);

        yield return new WaitUntil(() => asyncUnload.isDone);

        asyncUnload.completed += (AsyncOperation obj) =>
        {
            Debug.Log("Scene Unloaded: " + sceneToUnload);
        };

        _loadedScenes.Remove(sceneToUnload.SceneName);
    }

    public void UpdateLoadingEvents()
    {
        SceneChanger[] transitionObjects = FindObjectsByType<SceneChanger>(FindObjectsSortMode.None);

        if (transitionObjects.Length < 1)
        {
            Debug.LogWarning("No SceneChanger objects found in the scene.");
            return;
        }

        foreach (var transition in transitionObjects)
        {
            transition.OnSceneActionRequest += (actions) =>
            {
                StartCoroutine(HandleSceneActionRequest(actions));
            };
        }

        Debug.Log("Scene buttons loaded.");
    }
}
