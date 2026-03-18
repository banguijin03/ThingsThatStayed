using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    None, Loading, Title,
    _Length
}

public class UIManager : ManagerBase
{
    Canvas _maincanvas;
    public Canvas MainCanvas => _maincanvas;
    Dictionary<UIType, UIBase> uiDictionary=new();
    protected override IEnumerator OnConnected(GameManager newManager)
    {
        _maincanvas = GetComponentInChildren<Canvas>();
        //GameObject.FindGameObjectWithTag("MainCanvas");
        yield return null;
    }
    protected override void OnDisconnected()
    {

    }
    public UIBase SetUI(UIType wantType, UIBase wantUI)
    {
        if (wantUI == null) return null;

        if (uiDictionary.TryGetValue(wantType, out UIBase origin)) return origin;

        uiDictionary.Add(wantType, wantUI);
        return wantUI; 
    }
    public UIBase GetUI(UIType wantType)
    {
        if(uiDictionary.TryGetValue(wantType, out UIBase result)) return result;
        else return null;
    }
    public UIBase OpenUI(UIType wantType)
    {
        UIBase result = GetUI(wantType);
        if (result is IOpenable asOpenable) asOpenable.Open();
        return result;
    }      
    public UIBase CloseUI(UIType wantType)
    {
        UIBase result = GetUI(wantType);
        if (result is IOpenable asOpenable) asOpenable.Close();
        return result;
    }     
    public UIBase ToggleUI(UIType wantType)
    {
        UIBase result = GetUI(wantType);
        if (result is IOpenable asOpenable) asOpenable.Toggle();
        return result;
    }


}