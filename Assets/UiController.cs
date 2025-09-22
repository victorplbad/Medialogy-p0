using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class UiController : MonoBehaviour
{
    private VisualElement myButton;

    private VisualElement root;

    public UIDocument Document;

    public event Action<VisualTreeAsset> OnSceneChanged;

    public Stack<VisualTreeAsset> BackStack = new();

    private VisualTreeAsset CurrentScene;

    // Define a ButtonBehavior class (or use your existing one)
    [Serializable]
    public class ButtonBehavior
    {
        public string name;
        public Action<VisualElement> Execute;
    }

    public void OnEnable()
    {
        root = Document.rootVisualElement;

        // Don't bind immediately. Wait for the geometry to be calculated.
        root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        // Unregister the callback so this only runs once after the initial layout.
        root.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        CurrentScene = Document.visualTreeAsset;
        BindButtons(root);

        Debug.Log("Binding after geometry change:");
        Debug.Log(root);
    }

    public void ChangeSceneTo(VisualTreeAsset scene)
    {
        BackStack.Push(CurrentScene);
        CurrentScene = scene;
        HandleSceneChange(scene);
    }
    public void GoBack()
    {
        Debug.Log("go back to: " + BackStack.Peek());
        var scene = BackStack.Pop();
        HandleSceneChange(scene);
    }
    private void HandleSceneChange(VisualTreeAsset scene)
    {
        root.Clear();

        GetComponents<VideoPlayer>().ToList().ForEach(vp => Destroy(vp));
        var newScene = scene.Instantiate();
        scene.CloneTree(root);

        OnSceneChanged?.Invoke(scene);

        // rebind after loading new scene
        BindButtons(root);
    }

    private void BindButtons(VisualElement parent)
    {
        // Find ALL VisualElements that should act as buttons
        var buttons = parent.Query<VisualElement>(className: "CButton").ToList();
        // ^ you can mark them with a USS class = "button" in UXML

        foreach (var button in buttons)
        {
            if (button.dataSource is not button_behavior so)
            {
                Debug.Log("not button_behavior");
                continue;
            }
            button.RegisterCallback<ClickEvent>(evt =>
            {
                so.Execute(this); // Call a method on the ScriptableObject
            });
        }
    }
}
