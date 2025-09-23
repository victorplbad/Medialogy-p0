using UnityEngine;
using UnityEngine.UIElements;

public class ScrollController : MonoBehaviour
{
    private VisualElement _root;
    private UIDocument _document;

    private Vector3 _origin;
    private Vector3 _previousPos;
    private bool _isDragging;

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

            BindScroll(_root);
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
        BindScroll(_root);
    }

    private void BindScroll(VisualElement parent)
    {
        Debug.Log("Binding scrolls");

        // Find ALL VisualElements that should act as buttons
        var swipes = parent.Query<VisualElement>(className: "CSwipe").ToList();
        // ^ you can mark them with a USS class = "button" in UXML

        foreach (var swipe in swipes)
        {
            /*if (swipe.dataSource is not ScrollBehavior so)
            {
                continue;
            }*/

            swipe.RegisterCallback<PointerDownEvent>(evt =>
            {
                Debug.Log("Swipe started");

                _previousPos = evt.position;
                _isDragging = true;
            });

            swipe.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (!_isDragging)
                    return;

                var currentPos = evt.position;
                var delta = currentPos - _previousPos;

                _previousPos = currentPos;
                Debug.Log($"Swipe delta: {delta}");
            });

            swipe.RegisterCallback<PointerUpEvent>(evt =>
            {
                Debug.Log("Swipe ended");

                if (!_isDragging)
                    return;

                _isDragging = false;
            });

            Debug.Log($"Binding swipe: {swipe}");
        }
    }
}
