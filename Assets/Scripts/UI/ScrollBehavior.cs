using UnityEngine;
using UnityEngine.UIElements;

public class ScrollBehavior : ScriptableObject
{
    public enum ScrollDirection
    {
        Horizontal,
        Vertical,
        Both
    }

    private VisualElement _picture;
    private Vector3 _origin;

    [Header("Scroll Settings")]
    [Range(0, 100)] [SerializeField] private float _sensitivity = 1.0f;
    [SerializeField] private ScrollDirection _scrollDirection = ScrollDirection.Horizontal;
    [SerializeField] private float _elasticity = 0.1f; // How much to "bounce back" when over-scrolled
    [SerializeField] private float _inertia = 0.9f; // How much to continue moving after drag ends
    [SerializeField] private float _decelerationRate = 0.135f; // Rate of deceleration when inertia is applied
    [SerializeField] private float _scrollThreshold = 5.0f; // Minimum drag distance to start scrolling
    [SerializeField] private bool _clampToBounds = true; // Whether to clamp scrolling within bounds
    [SerializeField] private Rect _scrollBounds = new Rect(0, 0, 1000, 1000); // Define the scrollable area


    public void InitiateScroll(VisualElement picture)
    {
        _picture = picture;

        _origin = _picture.transform.position;
    }

    public void OnScroll(Vector2 delta)
    {
        if (_picture == null)
        {
            Debug.LogError("ScrollBehavior not initialized with a VisualElement. Call InitiateScroll first.");
            return;
        }


    }
}
