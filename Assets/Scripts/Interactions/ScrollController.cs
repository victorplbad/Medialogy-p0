using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollController : MonoBehaviour
{
    public enum ScrollState
    {
        None,
        Vertical,
        Horizontal
    }

    [SerializeField] private GameObject _content;
    private ScrollRect _scrollRect;
    private ScrollState _currentState = ScrollState.None;
    private bool _isDragging;
    private bool _locked = false;

    private Vector2 _previousPosition = Vector2.zero;

    private Vector2 _previousTouchPosition;

    private void Awake() {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private void Update() {
        if (Input.touchCount >= 1)
        {
            Touch touch = Input.GetTouch(Input.touchCount - 1);
            if (touch.phase == TouchPhase.Began) {
                _isDragging = true;
                Debug.Log("Touch Began");
            } else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                _isDragging = false;
                Debug.Log("Touch Ended");
            }

            if (_isDragging){
                if (_previousPosition == Vector2.zero) {
                    _previousPosition = touch.position;
                    return;
                }

                Vector2 touchDelta = touch.position - _previousTouchPosition;
                _previousTouchPosition = touch.position;

                if (_locked) return;

                Debug.Log($"Touch Delta: {touchDelta}, Magnitude: {touchDelta.magnitude}");
                if (touchDelta.magnitude < 100f) return;

                _locked = true;

                if (Mathf.Abs(touchDelta.x) > Mathf.Abs(touchDelta.y)) {
                    _currentState = ScrollState.Horizontal;
                    _scrollRect.horizontal = true;
                    _scrollRect.vertical = false;
                } else {
                    _currentState = ScrollState.Vertical;
                    _scrollRect.horizontal = false;
                    _scrollRect.vertical = true;
                }

            } else if (_currentState != ScrollState.None && !_isDragging) {
                _scrollRect.horizontal = true;
                _scrollRect.vertical = true;
                _currentState = ScrollState.None;
                _previousPosition = Vector2.zero;
                _locked = false;
            }
        }
    }
}
