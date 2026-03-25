using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public delegate void MouseDownEvent (Vector2 screenposition, Vector3 worldPosition);
public delegate void MouseUpEvent   (Vector2 screenposition, Vector3 worldPosition);
public delegate void MouseMoveEvent (Vector2 screenposition, Vector3 worldPosition);

[RequireComponent(typeof(PlayerInput))]
public class InputManager : ManagerBase
{
    public static event MouseDownEvent OnLeftMouseDown;
    public static event MouseDownEvent OnRightMouseDown;
    public static event MouseUpEvent OnLeftMouseUp;
    public static event MouseUpEvent OnRightMouseUp;
    public static event MouseMoveEvent OnMouseMove;

    PlayerInput targetInput;
    Dictionary<string, InputAction> actionDictionary = new();
    List<RaycastResult> cursorHitList = new(); 
    Vector2 cursorScreenPosition;
    Vector3 cursorWorldPosition;

    public bool is2D = true;

    protected override IEnumerator OnConnected(GameManager newManager)
    {
        targetInput = GetComponent<PlayerInput>();
        LoadAllActions();
        InitializeAllAction();
        GameManager.OnUpdateManager -= UpdateEvent;
        GameManager.OnUpdateManager -= UpdateEvent;
        GameManager.OnUpdateManager += UpdateEvent;
        yield return null;
    }
    protected override void OnDisconnected()
    {
        GameManager.OnUpdateManager -= UpdateEvent;
    }
    public void UpdateEvent(float deltaTime)
    {
        Debug.Log(deltaTime);
    }
    public GameObject GetGameObjectUnderCursor()
    {
        if (cursorHitList.Count == 0) return null;

        return cursorHitList[0].gameObject;
    }
    public void UpdateEvent()
    {
        GameManager.Instance.Camera.GetRaycastResult2D(cursorScreenPosition, cursorHitList);
    }
    void LoadAllActions()
    {
        foreach (InputAction currentAction in targetInput.actions)
        {
            actionDictionary.TryAdd(currentAction.name, currentAction);
        }
    }
    void InitializeAllAction()
    {
        if (actionDictionary == null || actionDictionary.Count == 0) return;
        InitializeAction("CursorPositionChanged", CursorPositionChanged);
        InitializeAction("MouseLeftButtonDown", (context) => OnLeftMouseDown?.Invoke(cursorScreenPosition, cursorWorldPosition));
        InitializeAction("MouseRightButtonDown", (context) => OnRightMouseDown?.Invoke(cursorScreenPosition, cursorWorldPosition));
        InitializeAction("MouseLeftButtonUp", (context) => OnLeftMouseUp?.Invoke(cursorScreenPosition, cursorWorldPosition));
        InitializeAction("MouseRightButtonUp", (context) => OnRightMouseUp?.Invoke(cursorScreenPosition, cursorWorldPosition));
    }
    void InitializeAction(string actionName, Action<InputAction.CallbackContext> actionMethod)
    {
        if (actionDictionary == null) return;
        if (actionDictionary.TryGetValue("CursorPositionChanged", out InputAction cursorPositionChange))
        {
            cursorPositionChange.performed += actionMethod;
        }
    }
    void CursorPositionChanged(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        if (is2D)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 0;
        }
        else
        {
            worldPosition = Vector3.zero;
        }
        cursorScreenPosition = screenPosition;
        cursorWorldPosition = worldPosition;

        OnMouseMove?.Invoke(screenPosition, worldPosition);
    }
}