using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.OnLeftMouseUp += CreateToMouse;
        InputManager.OnRightMouseDown += DestroyOnMouse;
    }
    void DestroyOnMouse(Vector2 screenPosition, Vector3 worldPosition)
    {
        Debug.Log(GameManager.Instance.Input.GetGameObjectUnderCursor());
        Debug.Log($"{screenPosition}:{worldPosition}");
    }
    void CreateToMouse(Vector2 screenPosition, Vector3 worldPosition)
    {
        Instantiate(DataManager.LoadDataFile<GameObject>("Square 3"), worldPosition, Quaternion.identity);
    }

    void MoveToMouse(Vector2 screenPosition, Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }
}