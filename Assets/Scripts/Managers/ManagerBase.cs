using UnityEngine;

public abstract class ManagerBase : MonoBehaviour
{
    GameManager _connectedManager;

    public void Connect(GameManager newManager)
    {
        if (_connectedManager != null) DisConnected();
        _connectedManager = newManager;
        OnConnected(newManager);
    }
    protected void DisConnected()
    {
        _connectedManager = null;
        OnDisconnected();
    }
    protected abstract void OnConnected(GameManager newManager);
    protected abstract void OnDisconnected();
}
