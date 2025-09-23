using UnityEngine;
using UnityEngine.UIElements;

public class ScrollController : MonoBehaviour
{
    public enum ScrollDirection
    {
        Horizontal,
        Vertical,
        Both
    }

    private VisualElement _root;
    private UIDocument _document;

    private Vector3 _previousPos;
    private bool _isDragging;

    private VisualElement _picture;
    private Vector3 _origin;

    [Header("Scroll Settings")]
    [Range(0, 100)][SerializeField] private float _sensitivity = 1.0f;
    [SerializeField] private ScrollDirection _scrollDirection = ScrollDirection.Horizontal;
    [SerializeField] private float _elasticity = 0.1f; // How much to "bounce back" when over-scrolled
    [SerializeField] private float _inertia = 0.9f; // How much to continue moving after drag ends
    [SerializeField] private float _decelerationRate = 0.135f; // Rate of deceleration when inertia is applied
    [SerializeField] private float _scrollThreshold = 5.0f; // Minimum drag distance to start scrolling
    [SerializeField] private bool _clampToBounds = true; // Whether to clamp scrolling within bounds
    [SerializeField] private Rect _scrollBounds = new Rect(0, 0, 1000, 1000); // Define the scrollable area

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
            //INITIATE SCROLL

            swipe.RegisterCallback<PointerDownEvent>(evt =>
            {
                Debug.Log("Swipe started");

                _previousPos = evt.position;
                _isDragging = true;

                Debug.Log($"Swipe origin: {_previousPos}");
            });

            swipe.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (!_isDragging)
                    return;

                var currentPos = evt.position;
                var delta = currentPos - _previousPos;

                OnScroll();

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

    private void OnScroll()
    {
        // Implement scroll logic here
    }

    private void Update()
    {
        // Implement inertia and elasticity logic here
    }
}
