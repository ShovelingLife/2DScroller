using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonGlobal<T> : MonoBehaviour where T : MonoBehaviour
{
    static T          _instance;
    static GameObject singletonObj;

    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    singletonObj = new GameObject();
                    singletonObj.name = "(Singleton) " + typeof(T).ToString();
                    _instance = singletonObj.AddComponent<T>();
                    //Singleton_global<T> tt = _instance as Singleton_global<T>;
                    //tt._InitSetting();
                }
                else 
                    singletonObj = _instance.gameObject;

                DontDestroyOnLoad(singletonObj);
            }
            return _instance;
        }
    }
}