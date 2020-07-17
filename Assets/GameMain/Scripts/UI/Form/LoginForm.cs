/*
 *		Description: 尝试用 MVC 去写这个界面的逻辑
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: #DATE#
 *
 */
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
namespace guoShuai
{
    // View
    public class LoginForm : UGuiForm
    {

        public InputField input_Account;
        public InputField input_Password;
        public Button btn_Login;
        public Button btn_Regist;

        private LoginModel model;
        private LoginLogic logic;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            model = new LoginModel();
            logic = new LoginLogic(this, model);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            logic.AddListenerWebRequest();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            logic.RemoveListenerWebRequest();
           
            base.OnClose(isShutdown, userData);
        }


        #region 按钮和点击事件

        public override void OnClick(string str)
        {
            base.OnClick(str);
            switch (str)
            {
                case "btn_Login":
                    OnClickLogin();
                    break;

                case "btn_Regist":
                    OnClickRegist();
                    break;
                case "btn_FindPsd":
                    OnClickFindPsd();
                    break;
                default:
                    Log.Error("不存在该按钮 " + str);
                    break;
            }
        }

     
        private void OnClickRegist()
        {
            Game.UI.OpenUIForm(FormEnum.MenuForm, this);
        }

        private void OnClickLogin()
        {
            model.SetValue(input_Account.text, input_Password.text);
            logic.Login();
        }

        private void OnClickFindPsd()
        {
            Game.UI.OpenUIForm(FormEnum.FindPsdForm, this);
        }


        #endregion
    }

    // Logic
    public class LoginLogic
    {
        private LoginForm form;
        private LoginModel model; 

        public LoginLogic(LoginForm form, LoginModel model)
        {
            this.form = form;
            this.model = model;
        }

        // 添加web请求监听事件
        public void AddListenerWebRequest()
        {
            Game.Event.Subscribe(WebRequestSuccessEventArgs.EventId, WebRequestSuccess);
            Game.Event.Subscribe(WebRequestFailureEventArgs.EventId, WebRequestFailure);
        }
        // 移除web请求监听事件
        public void RemoveListenerWebRequest()
        {
            Game.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, WebRequestSuccess);
            Game.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, WebRequestFailure);
        }

        // 登陆成功回调
        private void WebRequestSuccess(object sender, GameEventArgs e)
        {          
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
            if (ne.UserData != this) return;

            // TODO解析服务端返回的数据
            LoginData tmp = Utility.Json.ToObject<LoginData>(ne.GetWebResponseBytes());
            if (tmp.message == "1")
            {
                Log.Info("登陆成功");
                Game.UI.OpenUIForm(FormEnum.RegistForm);
            }
           
        }
        // 登陆失败回调
        private void WebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
            if (ne.UserData != this) return;

            Log.Error("登陆请求出错 : " + ne.ErrorMessage);
        }


        public void Login()
        {
            if (string.IsNullOrEmpty(model.Account))
            {
                Game.UI.PushDialog(new DialogParams()
                {
                    Mode = 2,
                    Title = "提示",
                    Message = "账号为空,请重新输入",
                    ConfirmText = "确定",
                    //OnClickConfirm = delegate (object userData) {  },
                    CancelText = "取消",
                   //OnClickCancel = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
                });
                return;
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                Game.UI.PushDialog(new DialogParams()
                {
                    Mode = 2,
                    Title = "提示",
                    Message = "密码为空,请重新输入",
                    ConfirmText = "确定",
                    //OnClickConfirm = delegate (object userData) {  },
                    CancelText = "取消",
                    //OnClickCancel = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
                });
                return;
            }

            string url = ServerUtility.GetLoginURL(model.Account, model.Password);
            Game.WebRequest.AddWebRequest(url, this);

        }

    }

    // Model
    public class LoginModel
    {
        public string Account { get; set; }

        public string Password { get; set; }

        public void SetValue(string acc,string psd)
        {
            this.Account = acc;
            this.Password = psd;
        }
    }
}