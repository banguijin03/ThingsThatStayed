using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataManager : ManagerBase
{
    static Dictionary<System.Type, Dictionary<string,Object>> dataDictionary = new();
    public override int LoadCount
    {
        get
        {
            var task = Addressables.LoadResourceLocationsAsync("Global");
            var result = task.WaitForCompletion();
            int count = result.Count; 
            task.Release();
            return count; 
        }
    }

    protected override IEnumerator OnConnected(GameManager newManager)
    {
        UIBase loading = UIManager.ClaimGetUI(UIType.Loading);
        IProgress<int> progressUI = loading as IProgress<int>;
        IStatus<string> statusUI = loading as IStatus<string>;

        int loaded = 0;
        int total = LoadCount;

        System.Action ProgressOnLoad = () =>
        {
            loaded++;
            progressUI?.AddCurrent(1);
            statusUI?.SetCurrentStatus($"Load Data ({loaded}/{total})");
        };
        LoadAllFromAssetBundle<GameObject>("Global", ProgressOnLoad);
        yield return null;
    }

    protected override void OnDisconnected()
    {

    }
    bool TryGetFileFromResources<T>(string path, out T result) where T : Object
    {
        //Resources.LoadAll<T>(path);
        result = Resources.Load<T>(path);
        return result != null;
    }
    public static void SaveDataFile<T>(T target) where T : Object
    {
        if (target == null) return;
        Dictionary<string, Object> innerDictionary;

        if (!dataDictionary.TryGetValue(typeof(T), out innerDictionary))
        {
            innerDictionary = new();
            dataDictionary.Add(typeof(T), innerDictionary);
        }
        innerDictionary.TryAdd(target.name, target);
    }
    public static T LoadDataFile<T>(string fileName) where T : Object
    {
        if (dataDictionary.TryGetValue(typeof(T), out Dictionary<string, Object> innerDictionary))
        {
            if (innerDictionary.TryGetValue(fileName, out Object result))
            {
                return result as T;
            }
        }
        return null;
    }
    public async void LoadAllFromAssetBundle<T>(string label, System.Action actionForEachLoad) where T : Object
    {
        var finder = Addressables.LoadAssetsAsync<T>(label, (T loaded) =>
        {
            SaveDataFile(loaded); 
            actionForEachLoad();  
        });
        await finder.Task;
        finder.Release();
    }

    public async void LoadFileFromAssetBundle<T>(string address) where T : Object
    {
        var finder = Addressables.LoadAssetAsync<T>(address);
        SaveDataFile(finder.Result);
        finder.Release();
    }
}