/*
 *		Description: 
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace guoShuai
{
    public static class ServerUtility
    {
        private static string head;
        static ServerUtility()
        {
            if (GameConfig.IsLocalServer)
                head = "http://192.168.2.150:8090/";
            else
                head = "47.105.137.129:8090/";
        }

        public static string GetLoginURL(string account,string psd)
        {
            string url = Utility.Text.Format("cms/user/landing?username={0}&password={1}", account, psd);
            return head + url;           
        }


        public static string GetRegistURL()
        {
            return head + Constant.WebURL.RegistURL;
        }


        public static string GetFindPsdURL(string acc,string psd,string code)
        {
            string url = Utility.Text.Format("cms/user/ForgetPwd?username={0}&password={1}&registereds={2}",acc,psd,code);
            return head + url;
        }

        public static string GetCodeURL()
        {
            return head + Constant.WebURL.GetCodeURL;
        }
    }
}
