﻿using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : class
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this as T)
        {
            Destroy(gameObject);
        }
    }
}