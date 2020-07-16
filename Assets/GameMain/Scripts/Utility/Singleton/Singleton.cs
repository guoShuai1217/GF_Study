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

public class Singleton<T> where T : class,new()
{
    private static readonly object oo = new object();
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                lock (oo)
                {
                    if(instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
  
}