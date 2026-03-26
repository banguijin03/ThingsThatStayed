using Unity.VisualScripting;
using UnityEngine;

public class MouseFollower : MonoBehaviour, IFunctionable
{
    void Start()
    {
        RegistrationFunctions();
    }
    void OnDestroy()
    {
        UnRegistrationFunctions();
    }

    public void RegistrationFunctions()
    {
        InputManager.OnMouseLeftUp += CreateToMouse;
        InputManager.OnMouseRightDown += DestroyOnMouse;
    }
    public void UnRegistrationFunctions()
    {
        InputManager.OnMouseLeftUp -= CreateToMouse;
        InputManager.OnMouseRightDown -= DestroyOnMouse;
    }

    void DestroyOnMouse(Vector2 screenPosition, Vector3 worldPosition)
    {
        ObjectManager.Destroy(GameManager.Instance.Input.GetGameObjectUnderCursor());
    }

    void CreateToMouse(Vector2 screenPosition, Vector3 worldPosition)
    {
        GameObject inst = ObjectManager.CreateObject(DataManager.LoadDataFile<GameObject>("Square 14"));
        inst.transform.position = worldPosition;
    }

    void MoveToMouse(Vector2 screenPosition, Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }
}