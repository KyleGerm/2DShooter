using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable Unity.PerformanceAnalysis
public class Pooler : ScriptableObject
{
    public Dictionary<Enum, List<GameObject>> Pool;
    private Dictionary<Enum, GameObject> referenceList;
    
    public Pooler()
    {
        Pool = new Dictionary<Enum, List<GameObject>>();
        referenceList = new Dictionary<Enum, GameObject>();
    }
    /// <summary>
    /// Returns a single available Gameobject
    /// </summary>
    /// <param name="prefabType">Which Pool to search</param>
    /// <param name="resizable">Can a new GameObject be created if none can be found?</param>
    /// <returns></returns>
    public GameObject GetNewObject(PrefabList prefabType, bool resizable)
    {
        List<GameObject> list = Pool[prefabType];
        for (int i = 0;i < list.Count; i++)
        {
            if (!list[i].activeInHierarchy)
            {
                return list[i];
            }
        }

        if (resizable)
        {
            GameObject obj = Instantiate(referenceList[prefabType]);
            obj.SetActive(false);
            Pool[prefabType].Add(obj);
            return obj;
        }
        return null;
    }

 /// <summary>
 /// Returns an array of Gameobjects from the Specified Pool.
 /// If the specified amount cannot be created, returns null.
 /// </summary>
 /// <param name="amount">Number of GameObjects to return</param>
 /// <param name="prefabType">Which pool to search</param>
 /// <param name="resizable">Can more objects be made to return the amount specified?</param>
 /// <returns></returns>
    public GameObject[] RequestMultiple(int amount, PrefabList prefabType, bool resizable)
    {
        List<GameObject> objectList = Pool[prefabType];
        int count = 0;
        GameObject[] list = new GameObject[amount];
        for (int i = 0; i < objectList.Count; i++)
        {   
            if (!objectList[i].activeInHierarchy)
            {
                list[count] = objectList[i];
                count++;
            }
            if(count== amount) break; 
        }

        if (count < amount && resizable)
        {
            for (int i = count - 1; i < amount; i++)
            {
                GameObject obj = Instantiate(referenceList[prefabType]);
                obj.SetActive(false);
                Pool[prefabType].Add(obj);
                list[count] = obj;
                count++;
                if (count == amount) break;

            }
        }

        return count == amount? list : null;
    } 

 /// <summary>
    /// Returns an array of Components other than GameObject. 
    /// If the Component is not part of the PrefabType specified, returns null.
    /// </summary>
    /// <typeparam name="T">Type of Component to search for</typeparam>
    /// <param name="amount">Number of Components to return</param>
    /// <param name="prefabType">Which pool to search</param>
    /// <returns></returns>
    public T[] RequestMultiple<T>(int amount, PrefabList prefabType) where T : Component
    {
        T[] objectList = new T[amount];
        List<GameObject> pool = Pool[prefabType];
        int count = 0;
        for(int i = 0;i < pool.Count; i++)
        {
            try
            {
                objectList[i] = pool[i].GetComponent<T>();
                count++;
                if (count == amount) break;
            }
            catch
            {
                continue;
            }
            
        }
        return count == amount? objectList : null;

    }
    /// <summary>
    /// Takes a Key value enum, an object to pool, and a number of objects to make.
    /// if the object does not inherit the IPoolable interface, the pool will not be made. 
    /// </summary>
    /// <param name="listType"></param>
    /// <param name="obj"></param>
    /// <param name="size"></param>
    public void GenerateList(PrefabList listType,GameObject obj, int size)
    {
        if (Pool.ContainsKey(listType)) 
        {
            return;
        }
        IPoolable test = obj.GetComponent<IPoolable>();
        if (test == null)
        {
            return;
        }

        List<GameObject> list = new List<GameObject>();
        referenceList.Add(listType, obj);
        for(int i = 0;i<size;i++)
        {
            GameObject objectToPool = Instantiate(obj);
            objectToPool.SetActive(false);
            list.Add(objectToPool);
        }

        Pool.Add(listType, list);

    }
    /// <summary>
    /// Returns Everything in the Pool Dictionary back to the pool.
    /// </summary>
    public void ReturnAll()
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            foreach (GameObject obj in Pool[(PrefabList)i])
            {
                if(obj.TryGetComponent(out IPoolable returnable))
                {
                    returnable.ReturnToPool();
                }
            }
        }
    }
}
