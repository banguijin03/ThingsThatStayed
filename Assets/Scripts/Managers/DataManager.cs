using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : ManagerBase
{
    public override int LoadCount
    {
        get
        {
            var task = Addressables.LoadResourceLocationsAsync("Global");
            var result  = task.WaitForCompletion();
            int count = result.Count;

            task.Release();
            return count;
        }
    }
    
    protected override IEnumerator OnConnected(GameManager newManager)
    {
        UIBase loading = UIManager.ClaimGetUI(UIType.Loading);
        IProgress<int> progressUI = loading as IProgress<int>;
        IStatus<string> statusUI= loading as IStatus<string>;
        //LoadFileFromAssetBundle<GameObject>("Origin/Prefabs/Square.prefab");
        int loaded= 0;
        int total= LoadCount;
        System.Action ProgressOnLoad = () => 
        {
            loaded++;
            progressUI.AddCurrent(1);
            statusUI?.SetCurrentStatus($"Load Data({loaded}/{total})");
        };

        LoadAllFromAssetBundle<GameObject>("Global", ProgressOnLoad);

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

    public void SaveDataFile<T>(T target) where T : Object
    {
        if (target == null) return;
        Debug.Log(target);
    }

    public async void LoadAllFromAssetBundle<T>(string label, System.Action actionforEachLoad) where T : Object
    {
        var finder = Addressables.LoadAssetsAsync<T>(label, (T loaded) => 
        { 
            SaveDataFile(loaded);
            actionforEachLoad();
        });
        await finder.Task;
    }

    public async void LoadFileFromAssetBundle<T>(string address) where T : Object
    {
        var finder = Addressables.LoadAssetAsync<GameObject>(address);
        await finder.Task;
        SaveDataFile(finder.Result);

    }
}