using System;
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
    public VisualTreeAsset lastScene{ get; private set; }
    private VisualTreeAsset currentScene{ get; set; }

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

        // Now that the layout is ready, bind the buttons.
        BindButtons(root);

        Debug.Log("Binding after geometry change:");
        Debug.Log(root);
    }

    public void ChangeSceneTo(VisualTreeAsset scene)
    {
        if (lastScene is null)
        {
            lastScene = scene;
            currentScene = scene;
        }
        else
        {
            lastScene = currentScene;
            currentScene = scene;
        }
        root.Clear();

        //Remove all video players from object
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
        Debug.Log("number of buttons: " + buttons.Count);
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
                Debug.Log($"Clicked on element bound to: {so.name}");
                so.Execute(this); // Call a method on the ScriptableObject
            });
        }
    }
}
