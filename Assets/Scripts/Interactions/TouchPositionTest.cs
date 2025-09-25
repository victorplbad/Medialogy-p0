using UnityEngine;

public class TouchPositionTest : MonoBehaviour
{
    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Debug.Log($"Mouse Position: {mousePosition}");
    }
}
