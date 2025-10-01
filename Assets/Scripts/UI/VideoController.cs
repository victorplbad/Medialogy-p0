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
        _root.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        BindButtons(_root);
    }

    private void BindButtons(VisualElement parent)
    {
        var buttons = parent.Query<VisualElement>(className: "VideoButton").ToList();

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
