using System.Collections;
using UnityEngine;

public class DataManager : ManagerBase
{
    public override int LoadCount => 100;
    protected override IEnumerator OnConnected(GameManager newManager)
    {
        UIBase loading = UIManager.ClaimGetUI(UIType.Loading);
        IProgress<int> progressUI = loading as IProgress<int>;
        IStatus<string> statusUI= loading as IStatus<string>;

        for(int i=0; i<LoadCount; i+=7)
        {
            progressUI.AddCurrent(7);
            statusUI?.SetCurrentStatus($"Load Data({i+1}/{LoadCount})");
            yield return new WaitForSeconds(0.5f);
        }
        
        yield return null;
    }
    protected override void OnDisconnected()
    {

    }

    bool TryGetFileFromResources<T>(string path, out T result) where T : Object
    {
        result=Resources.Load<T>(path);
        return result != null;
    }

    bool TryGetFileFromAssetBundle()
    {
        return false;
    }
}