using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the singleton class which you can use it in another project > Does destroy after changing scene
public class SingletonLocal<T> : MonoBehaviour where T:MonoBehaviour
{
    static T m_instance;

    public static T instance
    {
        get
        {
            GameObject singleton_obj = GameObject.FindObjectOfType<T>().gameObject;
            m_instance = (singleton_obj == null) ? singleton_obj.AddComponent<T>() : singleton_obj.GetComponent<T>();
            return m_instance;
        }
    }
}