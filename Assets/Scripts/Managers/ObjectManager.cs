using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static UnityEditor.FilePathAttribute;
[System.Serializable]
public struct PoolSetting
{
    public string poolName;
    public GameObject target;
    public int countInitial;
    public int countAdditional;
}

public class ObjectManager:ManagerBase
{
    [SerializeField] PoolSetting[] testSettings;
    protected override IEnumerator OnConnected(GameManager newManager)
    {
        yield return null;
    }
    protected override void OnDisconnected()
    {
        
    }

    public static GameObject CreateObject(GameObject prefab, Transform parent=null)
    {
        if (prefab == null) return null;

        GameObject result = Instantiate(prefab, parent);
        RegistrationObject(result);
        return result;
    }
    public static GameObject CreateObject(GameObject prefab, Vector3 position)
    {

        GameObject result =  CreateObject(prefab);
        if (result) result.transform.position = position;
        return result;
    }
    public static GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        {
            GameObject result = CreateObject(prefab);
            if (result)
            {
                result.transform.position = position;
                result.transform.rotation = rotation;
            }
            return result;
        }
    }

    public static GameObject CreateObject(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, Space space = Space.Self)
    {
        {
            GameObject result = CreateObject(prefab, parent);
        if (result)
        {
            switch (space)
            {
                case Space.World: 
                    result.transform.position = position;
                    result.transform.rotation = rotation;
                    break;
                case Space.Self:
                    result.transform.localPosition = position;
                    result.transform.localRotation = rotation;
                    break;
            }
            result.transform.position = position;
        }
            return result;
        }
    }
    public static GameObject CreateObject(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        GameObject result = CreateObject(prefab);
        if (result)
        {
            result.transform.localPosition = position;
            result.transform.localRotation = rotation;
            result.transform.localScale = scale;
        }
        return result;
    }
    public static GameObject CreateObject(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale, Space space = Space.Self)
    {
        {
            GameObject result = CreateObject(prefab, parent);
        if (result)
        {
            switch (space)
            {
                case Space.World: 
                    result.transform.position = position;
                    result.transform.rotation = rotation;
                    result.transform.localScale = scale;
                    //float scaledScaleX = scale.x * (result.transform.localScale.x / result.transform.lossyScale.x);
                    //float scaledScaleY = scale.y * (result.transform.localScale.y / result.transform.lossyScale.y);
                    //float scaledScaleZ = scale.z * (result.transform.localScale.z / result.transform.lossyScale.z);
                    //result.transform.localScale = new Vector3(scaledScaleX, scaledScaleY, scaledScaleZ);
                    break;
                case Space.Self:
                    result.transform.localPosition = position;
                    result.transform.localRotation = rotation;
                    result.transform.localScale = scale;
                    break;
            }
            result.transform.position = position;
        }
            return result;
        }
    }
    public static void RegistrationObject(GameObject target)
    {

        if (target)
        {
            foreach (var current in target.GetComponentsInChildren<IFunctionable>())
            {
                current.RegistrationFunctions();
            }
        }
    }
    public static void DestroyObject(GameObject target)
    {
        if (!target) return;
        UnRegistrationObject(target);
        Destroy(target);
    }
    public static void UnRegistrationObject(GameObject target)
    {
        if (!target)
        {
            foreach (var current in target.GetComponentsInChildren<IFunctionable>())
            {
                current.UnRegistrationFunctions();
            }
        }
    }
}
