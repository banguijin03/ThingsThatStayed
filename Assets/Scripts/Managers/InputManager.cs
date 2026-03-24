using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public delegate void MouseDownEvent(Vector3 position);
public delegate void MouseUpEvent(Vector3 position);
public delegate void MouseMoveEvent(Vector2 screenposition, Vector3 worldPosition);

[RequireComponent(typeof(PlayerInput))]
public class InputManager : ManagerBase
{
    public static event MouseDownEvent  OnLeftMouseDown;
    public static event MouseDownEvent  OnRightMouseDown;
    public static event MouseUpEvent    OnLefttMouseUp;
    public static event MouseUpEvent    OnRightMouseUp;
    public static event MouseMoveEvent  OnMouseMove;

    PlayerInput targetInput;
    Dictionary<string, InputAction> actionDictionary = new();

    public bool is2D = true;

    protected override IEnumerator OnConnected(GameManager newManager)
    {
        targetInput=GetComponent<PlayerInput>();
        LoadAllActions();
        InitializeAllAction();
        yield return null;
    }
    protected override void OnDisconnected()
    {

    }
    void LoadAllActions()
    {
        foreach(InputAction currentAction in targetInput.actions)
        {
            actionDictionary.TryAdd(currentAction.name, currentAction);
        }
    }
    void InitializeAllAction()
    {
        if (actionDictionary == null || actionDictionary.Count == 0) return;
        if(actionDictionary.TryGetValue("CursorPositionChanged", out InputAction cursorPositionChange))
        {
            cursorPositionChange.performed += CursorPositionChanged;
        }
    }
    
    void CursorPositionChanged(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector3 worldPosition=Camera.main.ScreenToWorldPoint(screenPosition);
        if (is2D)
        {
            worldPosition=Camera.main.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 0;
        }
        else
        {
            worldPosition = Vector3.zero;
        }
        OnMouseMove?.Invoke(screenPosition, worldPosition);
    }
}