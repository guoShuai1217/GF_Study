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
namespace guoShuai
{
    public class LoadingForm : UGuiForm
    {
 
        [SerializeField]
        private Transform tra;
 
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            tra.Rotate(-Vector3.forward * Time.deltaTime * 720);
        }
    
    }
}
