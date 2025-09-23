using UnityEngine;
using UnityEngine.UIElements;

public class VideoController : MonoBehaviour
{
    private VisualElement _root;

    private UIDocument _document;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void Start()
    {
        GetComponent<UiController>().OnSceneChanged += (scene) =>
        {
            _root.Clear();
            var newScene = scene.Instantiate();
            scene.CloneTree(_root);

            BindButtons(_root);
        };
    }

    private void OnEnable()
    {
        _root = _document.rootVisualElement;

        _root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        // Unregister the callback so this only runs once after the initial layout.
        _root.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);

        // Now that the layout is ready, bind the buttons.
        BindButtons(_root);
    }

    private void BindButtons(VisualElement parent)
    {
        // Find ALL VisualElements that should act as buttons
        var buttons = parent.Query<VisualElement>(className: "VideoButton").ToList();
        // ^ you can mark them with a USS class = "button" in UXML

        foreach (var button in buttons)
        {
            if (button.dataSource is not VideoBehavior so)
            {
                continue;
            }

            so.SetVideo(this);

            button.RegisterCallback<ClickEvent>(evt =>
            {
                so.OnClick(this); // Call a method on the ScriptableObject
            });
        }
    }
}
