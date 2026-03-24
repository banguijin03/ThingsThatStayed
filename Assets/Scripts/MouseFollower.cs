using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.OnMouseMove += MoveToMouse;
    }

    void MoveToMouse(Vector2 screenPosition, Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }
}