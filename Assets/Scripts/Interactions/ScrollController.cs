using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollController_Old : MonoBehaviour
{
    public enum ScrollState
    {
        None,
        Vertical,
        Horizontal
    }

    [SerializeField] private GameObject _content;
    [Range(0, 50)] [SerializeField] private int _magnitudeThreshold = 5;
    private ScrollRect _scrollRect;
    private ScrollState _currentState = ScrollState.None;
    private bool _isDragging;
    private bool _locked = false;

    private Vector2 _previousPosition = Vector2.zero;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private void Update()
    {
        if (Input.touchCount < 1)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _isDragging = true;
            Debug.Log("Touch Began");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            Debug.Log("Touch Ended");
        }

        if (_isDragging && !_locked)
        {
            GetScrollDirection();
        }
        else if (_currentState != ScrollState.None && !_isDragging)
        {
            _scrollRect.horizontal = true;
            _scrollRect.vertical = true;
            _currentState = ScrollState.None;
            _previousPosition = Vector2.positiveInfinity;
            _locked = false;
        }
    }

    private void GetScrollDirection()
    {
        Vector2 mousePosition = Input.mousePosition;

        if (_previousPosition == Vector2.positiveInfinity)
        {
            _previousPosition = mousePosition;
            return;
        }

        Vector2 touchDelta = mousePosition - _previousPosition;


        Debug.Log($"Touch Delta: {touchDelta}, Magnitude: {touchDelta.magnitude}");
        if (touchDelta.magnitude < _magnitudeThreshold)
            return;

        _locked = true;

        SetScrollState(Mathf.Abs(touchDelta.x) > Mathf.Abs(touchDelta.y) ? ScrollState.Horizontal : ScrollState.Vertical);
    }

    private void SetScrollState(ScrollState state)
    {
        _currentState = state;

        _scrollRect.horizontal = state == ScrollState.Horizontal;
        _scrollRect.vertical = state == ScrollState.Vertical;
        _scrollRect.scrollSensitivity = state == ScrollState.Horizontal ? 10f : 5f;
    }
}