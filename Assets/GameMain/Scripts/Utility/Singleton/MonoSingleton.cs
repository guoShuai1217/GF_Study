/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoSingleton<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject oo = new GameObject(typeof(T).Name);
                instance = oo.AddComponent<T>();
            }
            return instance;
        }
    }
    
    protected virtual void Awake()
    {
        instance = this as T;
    }
}