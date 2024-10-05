using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// GameObjectのプールを管理するクラス
/// MonoBehaviourがついているがGameObjectとしての配置は不要
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private static ObjectPool<T> instance;
    public static ObjectPool<T> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool<T>();
            }
            return instance;
        }
    }

    private Queue<T> pool = new Queue<T>();
    private GameObject prefab;

    private ObjectPool()
    {
        string resourcePath = GetResourcePath(typeof(T));
        prefab = Resources.Load<GameObject>(resourcePath);
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found at path: {resourcePath}");
        }
    }

    private string GetResourcePath(System.Type type)
    {
        // 型に応じてリソースパスを設定します。必要に応じてパスを変更してください。
        if (type == typeof(DamageText))
        {
            return "Prefabs/DamageText";
        }
        // 他の型の場合は適宜追加
        return null;
    }

    public T GetObject()
    {
        T obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
        }
        else
        {
            GameObject newObj = Object.Instantiate(prefab);
            obj = newObj.GetComponent<T>();
            if (obj == null)
            {
                Debug.LogError("Prefab does not have the required component.");
            }
        }
        obj.OnObjectReuse();
        return obj;
    }

    public void ReturnObject(T obj)
    {
        obj.OnObjectReturn();
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
