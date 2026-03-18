using System.Collections;
using UnityEngine;

public abstract class ManagerBase : MonoBehaviour
{
    GameManager _connectedManager;

    public IEnumerator Connect(GameManager newManager)
    {
        if (_connectedManager != null) DisConnect();
        _connectedManager = newManager;
        yield return OnConnected(newManager);
    }
    protected void DisConnect()
    {
        _connectedManager = null;
        OnDisconnected();
    }
    protected abstract IEnumerator OnConnected(GameManager newManager);
    protected abstract void OnDisconnected();
}
