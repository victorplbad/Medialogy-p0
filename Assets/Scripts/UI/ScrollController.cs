using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class ScrollController : MonoBehaviour
{
    public enum ScrollDirection
    {
        Horizontal,
        Vertical,
        Both
    }

    public enum OverBounds
    {
        None,
        Left,
        Right,
        Top,
        Bottom
    }

    private VisualElement _root;
    private UIDocument _document;

    private Dictionary<VisualElement, Vector3> _previousPosMap = new Dictionary<VisualElement, Vector3>();
    private Dictionary<VisualElement, bool> _isDraggingMap = new Dictionary<VisualElement, bool>();
    private Dictionary<VisualElement, bool> _canDragMap = new Dictionary<VisualElement, bool>();

    private Dictionary<VisualElement, VisualElement> _pictureMap = new Dictionary<VisualElement, VisualElement>();
    private Dictionary<VisualElement, Vector2> _pictureSizeMap = new Dictionary<VisualElement, Vector2>();
    private Dictionary<VisualElement, VisualElement> _containerMap = new Dictionary<VisualElement, VisualElement>();
    private Dictionary<VisualElement, Vector3> _originMap = new Dictionary<VisualElement, Vector3>();

    [Header("Scroll Settings")]
    [Range(0, 100)][SerializeField] private float _sensitivity = 1.0f;
    [SerializeField] private ScrollDirection _scrollDirection = ScrollDirection.Horizontal;
    [SerializeField] private float _elasticity = 0.1f; // How much to "bounce back" when over-scrolled
    [SerializeField] private bool _clampToBounds = true; // Whether to clamp scrolling within bounds
    private Dictionary<VisualElement, Vector2> _scrollBoundsMap = new Dictionary<VisualElement, Vector2>();
    private Dictionary<VisualElement, Vector2> _scrollSizeMap = new Dictionary<VisualElement, Vector2>();

    #region Initialization
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void Start()
    {
        GetComponent<UiController>().OnSceneChanged += (scene) =>
        {
            StartCoroutine(SceneChanged());
        };
    }

    private IEnumerator SceneChanged()
    {
        yield return null; // Wait a frame for the scene to load
        ClearScrolls();
        BindScroll(_root);
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
        // Find ALL VisualElements that should act as buttons
        var swipes = parent.Query<VisualElement>(className: "CSwipe").ToList();
        // ^ you can mark them with a USS class = "button" in UXML

        if (swipes.Count == 0)
        {
            Debug.Log("No swipes found");
            return;
        }

        foreach (var swipe in swipes)
        {
            InitScroll(swipe);
        }
    }

    private void InitScroll(VisualElement swipe)
    {
        var picture = swipe.Query<VisualElement>(className: "SwipePicture").First();
        var container = swipe.Query<VisualElement>(className: "SwipeContent").First();

        _pictureMap.Add(swipe, picture);
        _containerMap.Add(swipe, container);
        _originMap.Add(swipe, container.transform.position);
        _pictureSizeMap.Add(swipe, new Vector2(picture.resolvedStyle.width, picture.resolvedStyle.height));
        _scrollBoundsMap.Add(swipe, new Vector2(picture.resolvedStyle.width, picture.resolvedStyle.height));
        _scrollSizeMap.Add(swipe, new Vector2(swipe.resolvedStyle.width, swipe.resolvedStyle.height));

        Debug.Log($"Scroll bounds for {swipe}: {_scrollBoundsMap[swipe]}");
        Debug.Log($"{picture}: {picture.resolvedStyle.width}x{picture.resolvedStyle.height}");
        Debug.Log($"Origin for {swipe}: {_originMap[swipe]}");

        _previousPosMap.Add(swipe, Vector3.zero);
        _isDraggingMap.Add(swipe, false);

        // Register pointer events
        swipe.RegisterCallback<PointerDownEvent>(evt => PointerDown(swipe, evt));
        swipe.RegisterCallback<PointerMoveEvent>(evt => PointerMove(swipe, evt));
        swipe.RegisterCallback<PointerUpEvent>(evt => PointerUp(swipe));

        _canDragMap.Add(swipe, true);
        Debug.Log($"Bound swipe: {swipe}");
    }
    #endregion

    #region Pointer Events
    private void PointerDown(VisualElement swipe, PointerDownEvent evt)
    {
        if (!_canDragMap[swipe])
            return;

        Debug.Log("Swipe started");

        _previousPosMap[swipe] = evt.position;
        _isDraggingMap[swipe] = true;

        Debug.Log($"Swipe origin: {_previousPosMap[swipe]}");
    }

    private void PointerMove(VisualElement swipe, PointerMoveEvent evt)
    {
        if (!_isDraggingMap[swipe])
            return;

        var currentPos = evt.position;
        var delta = currentPos - _previousPosMap[swipe];

        OnScroll(swipe, delta);

        _previousPosMap[swipe] = currentPos;
        Debug.Log($"Swipe delta: {delta}");
    }

    private void PointerUp(VisualElement swipe)
    {
        if (!_isDraggingMap[swipe])
            return;

        var endPos = _containerMap[swipe].transform.position;
        var delta = endPos - _originMap[swipe];
        var endBounds = _scrollBoundsMap[swipe];
        var outerBounds = _scrollSizeMap[swipe] * 0.5f;
        outerBounds.x -= _pictureSizeMap[swipe].x * 0.5f;
        outerBounds.y -= _pictureSizeMap[swipe].y * 0.5f;
        Debug.Log($"Swipe total delta: {delta}");

        var overBounds = IsOverBounds(swipe, endPos, endBounds);

        if (overBounds != OverBounds.None && !IsOverOuterBounds(endPos, outerBounds))
        {
            Debug.Log("Over bounds!");
            ChangeToPicture(swipe, overBounds);
            return;
        }

        //Elastic return
        StartCoroutine(ElasticReturn(swipe, _originMap[swipe]));

        _isDraggingMap[swipe] = false;
        Debug.Log("Swipe ended");
    }
    #endregion

    #region Scroll Logic
    private OverBounds IsOverBounds(VisualElement swipe, Vector2 endPos, Vector2 bounds)
    {
        if (endPos.x < bounds.x - 1.5f * _pictureSizeMap[swipe].x)
            return OverBounds.Left;
        if (endPos.x > bounds.x - 0.5f * _pictureSizeMap[swipe].x)
            return OverBounds.Right;
        if (endPos.y < -bounds.y + 0.5f * _pictureSizeMap[swipe].y)
            return OverBounds.Top;
        if (endPos.y > bounds.y - 0.5f * _pictureSizeMap[swipe].y)
            return OverBounds.Bottom;

        return OverBounds.None;
    }

    private bool IsOverOuterBounds(Vector2 endPos, Vector2 bounds)
    {
        if (endPos.x < -bounds.x || endPos.x > bounds.x)
            return true;
        if (endPos.y < -bounds.y || endPos.y > bounds.y)
            return true;

        return false;
    }

    private void ChangeToPicture(VisualElement swipe, OverBounds direction)
    {
        var newOrigin = _originMap[swipe];
        newOrigin.x = direction == OverBounds.Left ? newOrigin.x - _pictureSizeMap[swipe].x :
                      direction == OverBounds.Right ? newOrigin.x + _pictureSizeMap[swipe].x : newOrigin.x;
        _originMap[swipe] = newOrigin;

        var newBounds = _scrollBoundsMap[swipe];
        newBounds.x = direction == OverBounds.Left ? newBounds.x - _pictureSizeMap[swipe].x :
                      direction == OverBounds.Right ? newBounds.x + _pictureSizeMap[swipe].x : newBounds.x;
        _scrollBoundsMap[swipe] = newBounds;

        StartCoroutine(ElasticReturn(swipe, newOrigin));

        _isDraggingMap[swipe] = false;
        Debug.Log("Swipe ended");
    }

    private void OnScroll(VisualElement swipe, Vector2 delta)
    {
        // Implement scroll logic here
        var newPos = _containerMap[swipe].transform.position;
        if (_scrollDirection == ScrollDirection.Horizontal || _scrollDirection == ScrollDirection.Both)
        {
            newPos.x += delta.x * _sensitivity * 100 * Time.deltaTime;
        }
        if (_scrollDirection == ScrollDirection.Vertical || _scrollDirection == ScrollDirection.Both)
        {
            newPos.y += delta.y * _sensitivity * 100 * Time.deltaTime;
        }

        if (_clampToBounds)
        {
            newPos.x = Mathf.Clamp(newPos.x, _scrollBoundsMap[swipe].x - 2 * _pictureSizeMap[swipe].x, _scrollBoundsMap[swipe].x);
            //newPos.y = Mathf.Clamp(newPos.y, _scrollBoundsMap[swipe].y - _pictureSizeMap[swipe].y, _scrollBoundsMap[swipe].y + _pictureSizeMap[swipe].y);
        }

        _containerMap[swipe].transform.position = newPos;
        Debug.Log($"New position: {newPos}");
    }

    private IEnumerator ElasticReturn(VisualElement swipe, Vector3 targetPos)
    {
        _canDragMap[swipe] = false;

        while (Vector3.Distance(_containerMap[swipe].transform.position, targetPos) > 0.1f)
        {
            _containerMap[swipe].transform.position = Vector3.Lerp(_containerMap[swipe].transform.position, targetPos, _elasticity);
            yield return null;
        }

        _containerMap[swipe].transform.position = targetPos;
        _canDragMap[swipe] = true;
    }
    #endregion

    #region Utility
    private void ClearScrolls()
    {
        _previousPosMap.Clear();
        _isDraggingMap.Clear();
        _canDragMap.Clear();
        _pictureMap.Clear();
        _containerMap.Clear();
        _originMap.Clear();
        _scrollBoundsMap.Clear();
        _pictureSizeMap.Clear();
    }
    #endregion
}
