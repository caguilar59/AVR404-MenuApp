using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton behaviour class, used for components that should only have one instance
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set;}

    //Returns whether instance has been intiialized
    public static bool IsInitialized
    {
        get { return Instance != null; }
    }

    //Base awake method that sets singleton's unique instance
    protected virtual void Awake()
    {
        if (Instance != null)
            Debug.LogError($"Trying to instantiate a second instance of singleton class {GetType().Name}");
        else
            Instance = (T)this;
    }


    protected virtual void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}