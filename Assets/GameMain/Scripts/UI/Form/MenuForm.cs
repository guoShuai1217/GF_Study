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
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace guoShuai
{
    public class MenuForm : UGuiForm 
    {





        #region 按钮和点击事件
   
        public override void OnClick(string str)
        {
            base.OnClick(str);

            switch (str)
            {
                case "btn_NewCase":

                    break;

                case "btn_OpenCase":

                    break;
                default:
                    Log.Error("按钮不存在 " + str);
                    break;
            }
        }


    










        #endregion
 




    }
}
