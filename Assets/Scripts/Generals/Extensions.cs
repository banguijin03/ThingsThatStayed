using UnityEngine;

public static class Extensions
{
    public static float normalized(this float target)
    {
        if (target > 0)         return 1.0f;
        else if (target < 0)    return -1.0f;
        else                    return 0.0f;
    }

    public static T TryAddComponent<T>(this GameObject target) where T : Component
    {
        T result = null;
        if (target == null) return result;//RVO
        //result = target.GetComponent<T>(); 
        //if (result is null) result = target.AddComponent<T>();
        //=
        result = target.GetComponent<T>() ?? target.AddComponent<T>();
        //=
        //result= target.GetComponent<T>();
        //result ??= target.AddComponent<T>();

        return result;
    }

    public static T TryAddComponent<T>(this Component target)where T : Component
    {
        if(target==null) return null;
        else return target.gameObject.TryAddComponent<T>();//NRVO
    }
}