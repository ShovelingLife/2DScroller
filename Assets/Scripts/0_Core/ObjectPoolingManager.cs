using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// To use after changing the game scene
[Serializable]
public class ObjectPoolData
{
    // Object type and quantity
    public GameObject objPool = null;
    public int        count    = 0;

    // Initialize with objects used before changing scene
    public ObjectPoolData(GameObject _obj, int _count)
    {
        objPool = _obj;
        count    = _count;
    }
}

public class ObjectPoolingManager : SingletonLocal<ObjectPoolingManager>
{
    // Class Type - Each class type reflection
    protected Dictionary<Type, Stack<GameObject>> mDictObjPool = new Dictionary<Type, Stack<GameObject>>();

    // Used objects
    public List<ObjectPoolData> listPrevObj = new List<ObjectPoolData>();


    private void Awake()
    {
        InitSettingPrevObj();
    }

    // Resets the list that contains all the objects used before
    protected void InitSettingPrevObj()
    {
        // Enforce to add objects
        foreach (var item in listPrevObj)
        {
            Stack<GameObject> tmp_stack = new Stack<GameObject>();
            mDictObjPool.Add(item.objPool.GetType(), tmp_stack);

            // Creates objects and add to the stack
            for (int i = 0; i < item.count; ++i)
            {
                GameObject copyobj = GameObject.Instantiate(item.objPool);
                mDictObjPool.Add(copyobj.GetType(), tmp_stack);
            }
        }
    }

    // Initializes all the objects
    public void InitPrevObj(GameObject _obj, int _count)
    {
        Stack<GameObject> tmp_stack = new Stack<GameObject>();
        mDictObjPool.Add(_obj.GetType(), tmp_stack);

        // Creates objects and add to the stack
        for (int i = 0; i < _count; ++i)
        {
            GameObject copy_obj = GameObject.Instantiate(_obj);
            copy_obj.SetActive(false);
            mDictObjPool.Add(copy_obj.GetType(), tmp_stack);
        }
    }

    // Add as child to the object which has the container
    public Transform CreateObj(Type _type, Transform _trans, Transform _trans_parent = null)
    {
        Transform         trans_out = null;
        Stack<GameObject> tmp_stack = null;

        // ------- Stack initialization -------

        // If Type key exists then get it from Dictionary
        if (mDictObjPool.ContainsKey(_type))
            tmp_stack = mDictObjPool[_type];

        // If not creates another stack and add it to Dictionary
        else
        {
            tmp_stack = new Stack<GameObject>();
            mDictObjPool.Add(_type, tmp_stack);
        }
        // ------- Transform Initialization -------

        // Gets an object from the stack and if it's turned off then gets it's Transform
        if (tmp_stack.Count > 0 &&
            !tmp_stack.Peek().activeInHierarchy)
            trans_out = tmp_stack.Pop().transform;

        // If not creates another and add it
        {
            trans_out = GameObject.Instantiate(_trans);
            mDictObjPool[_type].Push(trans_out.gameObject);
        }
        // Setting parent
        if (_trans_parent != null)
            trans_out.SetParent(_trans_parent);

        trans_out.gameObject.SetActive(true);
        return trans_out;
    }

    // Takes whatever component to create the object (Can be used in many components)
    public T CreateObj<T>(Type _type, T _clone_obj, Transform _trans_parent = null) where T : Component
    {
        T                 out_result = null;
        Stack<GameObject> tmp_stack  = null;

        // ------- Stack initialization -------

        // If Type exists take it from dictionary
        if (mDictObjPool.ContainsKey(_type))
            tmp_stack = mDictObjPool[_type];

        // If not then creates a new one and add it onto stack
        else
        {
            tmp_stack = new Stack<GameObject>();
            mDictObjPool.Add(_type, tmp_stack);
        }

        // ------- Transform Initializaiton -------

        // If turned off take it from stack
        if (tmp_stack.Count > 0 &&
            !tmp_stack.Peek().activeInHierarchy)
            out_result = tmp_stack.Pop().GetComponent<T>();

        // If not add new and add it
        else
        {
            out_result = GameObject.Instantiate<T>(_clone_obj);
            mDictObjPool[_type].Push(out_result.gameObject);
        }
        // Sets the object parent
        if (_trans_parent != null)
            out_result.transform.SetParent(_trans_parent);

        out_result.gameObject.SetActive(true);
        return out_result;
    }

    //void RemoveObject()

    // A coroutine which deletes the object
    IEnumerator IE_RemoveObj(Type _type, Transform _trans, Transform _trans_parent = null, float _remove_sec = 0.0f)
    {
        yield return new WaitForSeconds(_remove_sec);
        RemoveObj(_type, _trans, _trans_parent);
    }

    // Deleted after certain amount of time. Ex) VFX
    public Transform CreateRemoveDelayedObj(Type _type,
        Transform _trans,
        float     _remove_sec,
        Transform _trans_parent        = null,
        Transform _trans_remove_parent = null)
    {
        Transform copy_obj = null;

        // If time passed deletes
        if (_remove_sec > 0f)
        {
            copy_obj = CreateObj(_type, _trans, _trans_parent);
            StartCoroutine(IE_RemoveObj(_type, copy_obj, _trans_remove_parent, _remove_sec));
        }
        return copy_obj;
    }

    // Deletes the object
    public void RemoveObj(Type _type, Transform _trans, Transform _trans_parent = null)
    {
        // If container transform not null then add
        if (_trans_parent != null)
            _trans.SetParent(_trans_parent);

        // If the class type it's inside of the hash table then add it
        if (mDictObjPool.ContainsKey(_type))
        {
            mDictObjPool[_type].Push(_trans.gameObject);
            _trans.gameObject.SetActive(false);
        }
        // If not then delete
        else
        {
            Debug.LogErrorFormat("Not data in the pool check it : {0}", _trans.name);
            GameObject.Destroy(_trans.gameObject);
        }
    }

    // Deletes the object based on the object type
    public void RemoveObj<T>(Type _type, T _obj, Transform _trans_parent = null) where T : Component
    {
        // If container transform not null then add
        if (_trans_parent != null)
            _obj.transform.SetParent(_trans_parent);

        // If the class type it's inside of the hash table then add it
        if (mDictObjPool.ContainsKey(_type))
        {
            mDictObjPool[_type].Push(_obj.gameObject);
            _obj.gameObject.SetActive(false);
        }
        // If not then delete
        else
        {
            Debug.LogErrorFormat("Not data in the pool check it 2 : {0}", _obj.name);
            GameObject.Destroy(_obj.gameObject);
        }
    }
}