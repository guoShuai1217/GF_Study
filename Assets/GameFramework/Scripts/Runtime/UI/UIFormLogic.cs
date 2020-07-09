//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 界面逻辑基类。
    /// </summary>
    public abstract class UIFormLogic : MonoBehaviour
    {
        private bool m_Available = false;
        private bool m_Visible = false;
        private UIForm m_UIForm = null;
        private Transform m_CachedTransform = null;
        private int m_OriginalLayer = 0;

        /// <summary>
        /// 获取界面。
        /// </summary>
        public UIForm UIForm
        {
            get
            {
                return m_UIForm;
            }
        }

        /// <summary>
        /// 获取或设置界面名称。
        /// </summary>
        public string Name
        {
            get
            {
                return gameObject.name;
            }
            set
            {
                gameObject.name = value;
            }
        }

        /// <summary>
        /// 获取界面是否可用。
        /// </summary>
        public bool Available
        {
            get
            {
                return m_Available;
            }
        }

        /// <summary>
        /// 获取或设置界面是否可见。
        /// </summary>
        public bool Visible
        {
            get
            {
                return m_Available && m_Visible;
            }
            set
            {
                if (!m_Available)
                {
                    Log.Warning("UI form '{0}' is not available.", Name);
                    return;
                }

                if (m_Visible == value)
                {
                    return;
                }

                m_Visible = value;
                InternalSetVisible(value);
            }
        }

        /// <summary>
        /// 获取已缓存的 Transform。
        /// </summary>
        public Transform CachedTransform
        {
            get
            {
                return m_CachedTransform;
            }
        }

        /// <summary>
        /// 界面初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnInit(object userData)
        {
            initProperty();

            if (m_CachedTransform == null)
            {
                m_CachedTransform = transform;
            }

            m_UIForm = GetComponent<UIForm>();
            m_OriginalLayer = gameObject.layer;
        }

        /// <summary>
        /// 界面回收。
        /// </summary>
        protected internal virtual void OnRecycle()
        {
        }

        /// <summary>
        /// 界面打开。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnOpen(object userData)
        {
            m_Available = true;
            Visible = true;
        }

        /// <summary>
        /// 界面关闭。
        /// </summary>
        /// <param name="isShutdown">是否是关闭界面管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnClose(bool isShutdown, object userData)
        {
            gameObject.SetLayerRecursively(m_OriginalLayer);
            Visible = false;
            m_Available = false;
        }

        /// <summary>
        /// 界面暂停。
        /// </summary>
        protected internal virtual void OnPause()
        {
            Visible = false;
        }

        /// <summary>
        /// 界面暂停恢复。
        /// </summary>
        protected internal virtual void OnResume()
        {
            Visible = true;
        }

        /// <summary>
        /// 界面遮挡。
        /// </summary>
        protected internal virtual void OnCover()
        {
        }

        /// <summary>
        /// 界面遮挡恢复。
        /// </summary>
        protected internal virtual void OnReveal()
        {
        }

        /// <summary>
        /// 界面激活。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnRefocus(object userData)
        {
        }

        /// <summary>
        /// 界面轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected internal virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 界面深度改变。
        /// </summary>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="depthInUIGroup">界面在界面组中的深度。</param>
        protected internal virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
        }

        /// <summary>
        /// 设置界面的可见性。
        /// </summary>
        /// <param name="visible">界面的可见性。</param>
        protected virtual void InternalSetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }



        #region Reflection

        public Dictionary<string, ArrayList> _dic;

        void initDic()
        {
            if (_dic == null)
            {
                _dic = new Dictionary<string, ArrayList>();
                var arr = GetComponentsInChildren<Transform>(true);
                foreach (var item in arr)
                {
                    ArrayList list = null;
                    _dic.TryGetValue(item.name, out list);
                    if (list == null)
                    {
                        list = new ArrayList();
                    }
                    list.Add(item);
                    _dic[item.name] = list;
                }
            }
        }

        public void initProperty()
        {
            initDic();

            var arrBtn = GetComponentsInChildren<Button>(true);
            if (arrBtn != null)
            {
                foreach (var item in arrBtn)
                {
                    var btnTemp = item;
                    item.onClick.AddListener(() =>
                    {
                        this.OnClick(btnTemp);
                    });
                }
            }

            var type = this.GetType();
            var arrField = type.GetFields();
            foreach (var item in arrField)
            {
                var child = getChildByName(item.Name);

                // checkName(child, item);

                if (child != null)
                {
                    var typeItem = item.FieldType;
                    if (item.FieldType == typeof(GameObject))
                    {
                        item.SetValue(this, child.gameObject);
                    }
                    else if (item.FieldType == typeof(Transform))
                    {
                        item.SetValue(this, child);
                    }
                    else if (isUIClass(typeItem))
                    {
                        try
                        {
                            var value = child.GetComponent(typeItem);
                            if (value != null)
                            {
                                item.SetValue(this, value);
                            }
                            else
                            {
                                if (isKindOfPPPUIBase(typeItem))
                                {
                                    var obj = addScript(child.gameObject, typeItem.FullName);
                                    if (obj != null)
                                    {
                                        item.SetValue(this, obj);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    else if (typeItem.BaseType == typeof(Array))
                    {
                        var strTypeOne = typeItem.FullName.Replace("[]", "");

                        var typeOne = GetType(strTypeOne);

                        var list = getChildsByName(item.Name);
                        if (list == null || typeOne == null)
                        {
                            continue;
                        }
                        if (isUIClass(typeOne))
                        {
                            try
                            {
                                var arr = Array.CreateInstance(typeOne, list.Count);
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var childTemp = list[i] as Transform;
                                    var value = childTemp.GetComponent(typeOne);
                                    if (value == null)
                                    {
                                        if (isKindOfPPPUIBase(typeOne))
                                        {
                                            value = addScript(childTemp.gameObject, typeOne.FullName);
                                        }
                                    }
                                    arr.SetValue(value, i);
                                    if (value == null)
                                    {
                                        Debug.LogError("this value is null");
                                    }
                                }
                                item.SetValue(this, arr);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }
                    }

                    else
                    {
                        continue;
                    }
                }
            }
            var time2 = DateTime.Now;
        }
        bool isUIClass(Type typeItem)
        {
            var p = typeItem.GetProperty("transform");
            if (p == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public T getChildByName<T>(string name)
        {
            var traTemp = getChildByName(name);
            if (traTemp == null)
            {
                return default(T);
            }
            if (isUIClass(typeof(T)) == false)
            {
                return default(T);
            }
            if (typeof(T) == typeof(GameObject) || typeof(T) == typeof(Transform))
            {
                Debug.LogError("该方法不能获取 GameObject  Transform");
            }
            return traTemp.gameObject.GetComponent<T>();
        }
        public Transform getChildByName(string name)
        {
            initDic();
            ArrayList list = null;
            _dic.TryGetValue(name, out list);
            Transform child = null;
            if (list != null)
            {
                child = list[0] as Transform;
            }
            return child;
        }
        public ArrayList getChildsByName(string name)
        {
            initDic();
            ArrayList list = null;
            _dic.TryGetValue(name, out list);
            return list;
        }

        public static UIFormLogic addScript(GameObject obj, string strClassName)
        {
            var type = Type.GetType(strClassName);
            UIFormLogic ret = null;
            if (type != null)
            {
                ret = obj.AddComponent(type) as UIFormLogic;
                if (ret != null)
                {
                    ret.initProperty();
                    ret.Init();
                }
            }
            return ret;
        }

        void SetValues(params object[] sceneData)
        {

        }


        public virtual void OnClick(string str)
        {

        }

        public virtual void OnClick(Button btn)
        {
            OnClick(btn.name);
        }

        static bool isKindOfPPPUIBase(Type type)
        {
            return typeof(UIForm).IsAssignableFrom(type);
        }

        /// <summary>
        /// 用于获取组件什么的,类似Start,只执行一次
        /// </summary>
        void Init()
        {



        }
        Component findChild(string strPath, Type type)
        {
            var child = transform.Find(strPath);
            if (child == null)
            {
                return null;
            }
            if (isUIClass(type) == false)
            {
                return null;
            }
            return child.gameObject.GetComponent(type);
        }


        public T findChild<T>(string strPath)
        {
            var child = transform.Find(strPath);
            if (child == null)
            {
                return default(T);
            }
            if (isUIClass(typeof(T)) == false)
            {
                return default(T);
            }
            return child.gameObject.GetComponent<T>();
        }

        public static T newGameObject<T>(Transform parent, string strName, Vector3 pos, Vector2 size)
        {
            var obj = new GameObject(strName, new Type[1] { typeof(T) });
            if (parent != null)
            {
                obj.transform.SetParent(parent);
            }
            obj.transform.localPosition = pos;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;

            var ret = obj.GetComponent<T>();
            if (size != Vector2.zero)
            {
                var rect = obj.GetComponent<RectTransform>();
                if (rect == null)
                {
                    rect = obj.AddComponent<RectTransform>();
                }
                rect.sizeDelta = size;
            }
            if (typeof(T).Equals(typeof(Text)))
            {
                var txt = ret as Text;
                //txt.font = Game.Instance.txtVersion.font;
                txt.fontSize = 20;
                txt.alignment = TextAnchor.MiddleCenter;
            }
            return ret;
        }

        public static GameObject newGameObject(Transform parent, string strName, Vector3 pos, Vector2 size)
        {
            var ret = new GameObject(strName);
            if (parent != null)
            {
                ret.transform.SetParent(parent);
            }
            ret.transform.localPosition = pos;
            ret.transform.localRotation = Quaternion.identity;
            ret.transform.localScale = Vector3.one;

            if (size != Vector2.zero)
            {
                var rect = ret.GetComponent<RectTransform>();
                if (rect == null)
                {
                    rect = ret.AddComponent<RectTransform>();
                }
                rect.sizeDelta = size;
            }
            return ret;
        }

        public static ScrollRect newScrollView(Transform parent, string strName, Vector3 pos, Vector2 size, Vector2 sizeContent, Vector2 spacingContent, GridLayoutGroup.Axis axis, TextAnchor childAlignment, GridLayoutGroup.Constraint constraint, int constraintCount)
        {
            var scrollRect = newGameObject<ScrollRect>(parent, strName, pos, size);
            var viewport = newGameObject<Mask>(scrollRect.transform, "Viewport", Vector3.zero, size);
            var content = newGameObject<GridLayoutGroup>(viewport.transform, "Content", Vector3.zero, size);
            content.cellSize = sizeContent;
            content.spacing = spacingContent;
            content.startAxis = axis;
            content.childAlignment = childAlignment;
            content.constraint = constraint;
            content.constraintCount = constraintCount;
            scrollRect.content = content.GetComponent<RectTransform>();
            return scrollRect;
        }

        static CanvasScaler getScaler(Transform traRoot)
        {
            CanvasScaler ret = null;
            var parent = traRoot;
            while (parent != null)
            {
                ret = parent.GetComponent<CanvasScaler>();
                if (ret != null)
                {
                    break;
                }
                else
                {
                    parent = parent.parent;
                }
            }
            return ret;
        }
        public static void autoLayout2D(Transform traRoot)
        {
            if (traRoot == null)
            {
                return;
            }
            int ManualWidth = 1280;   //首先记录下你想要的屏幕分辨率的宽
            int ManualHeight = 720;   //记录下你想要的屏幕分辨率的高        //普通安卓的都是 1280*720的分辨率
            var scaleOld = traRoot.localScale;
            var canvasScaler = getScaler(traRoot);
            if (canvasScaler == null)
            {
                //PPP.pppShow();
                return;
            }
            if (canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.Expand)
            {
                //然后得到当前屏幕的高宽比 和 你自定义需求的高宽比。通过判断他们的大小，来不同赋值
                //*其中Convert.ToSingle（）和 Convert.ToFloat() 来取得一个int类型的单精度浮点数（C#中没有 Convert.ToFloat() ）；
                if (Convert.ToSingle(Screen.height) / Screen.width > Convert.ToSingle(ManualHeight) / ManualWidth)
                {
                    //如果屏幕的高宽比大于自定义的高宽比 。则通过公式  ManualWidth * manualHeight = Screen.width * Screen.height；
                    //来求得适应的  manualHeight ，用它待求出 实际高度与理想高度的比率 scale
                    int manualHeight = Mathf.RoundToInt(Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
                    var scaleY = manualHeight * 1.0f / ManualHeight;
                    traRoot.transform.localScale = new Vector3(scaleOld.x, scaleOld.y * scaleY, 1f);
                }
                else if (Convert.ToSingle(Screen.height) / Screen.width < Convert.ToSingle(ManualHeight) / ManualWidth)
                {
                    int manualWidth = Mathf.RoundToInt(Convert.ToSingle(ManualHeight) / Screen.height * Screen.width);
                    var scaleX = manualWidth * 1.0f / ManualWidth;
                    traRoot.transform.localScale = new Vector3(scaleOld.x * scaleX, scaleOld.y, 1f);
                }
            }
            else if (canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight && canvasScaler.matchWidthOrHeight == 0)
            {
                //然后得到当前屏幕的高宽比 和 你自定义需求的高宽比。通过判断他们的大小，来不同赋值
                //*其中Convert.ToSingle（）和 Convert.ToFloat() 来取得一个int类型的单精度浮点数（C#中没有 Convert.ToFloat() ）；
                if (Convert.ToSingle(Screen.height) / Screen.width > Convert.ToSingle(ManualHeight) / ManualWidth)
                {
                    //如果屏幕的高宽比大于自定义的高宽比 。则通过公式  ManualWidth * manualHeight = Screen.width * Screen.height；
                    //来求得适应的  manualHeight ，用它待求出 实际高度与理想高度的比率 scale
                    int manualHeight = Mathf.RoundToInt(Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
                    var scaleY = manualHeight * 1.0f / ManualHeight;
                    traRoot.transform.localScale = new Vector3(scaleOld.x, scaleOld.y * scaleY, 1f);
                }
                else if (Convert.ToSingle(Screen.height) / Screen.width < Convert.ToSingle(ManualHeight) / ManualWidth)
                {
                    int manualWidth = Mathf.RoundToInt(Convert.ToSingle(ManualHeight) / Screen.height * Screen.width);
                    var scaleX = manualWidth * 1.0f / ManualWidth;
                    traRoot.transform.localScale = new Vector3(scaleOld.x, scaleOld.y / scaleX, 1f);
                }
            }
            else
            {
                //PPP.pppShow();
            }
        }

        //public static void addButtonClickEffect(Button button)
        //{
        //    if (button.transition == Selectable.Transition.ColorTint || button.transition == Selectable.Transition.None)
        //    {
        //        GameObject btn = button.gameObject;
        //        EventTriggerListener.Get(btn).onDown = (bt) =>
        //        {
        //            bt.transform.DOKill();
        //            bt.transform.DOScale(((Vector3)PPP.getPropertyOriginal(bt.transform, "localScale")) * 0.9f, 0.1f);
        //        };

        //        EventTriggerListener.Get(btn).onUp = (bt) =>
        //        {
        //            bt.transform.DOKill();
        //            bt.transform.DOScale((Vector3)PPP.getPropertyOriginal(bt.transform, "localScale"), 0.1f);
        //        };
        //    }
        //}

        void checkName(Transform tra, System.Reflection.FieldInfo item)
        {
            if (tra == null)
            {
                return;
            }
            var strName = tra.name;
            var type = item.FieldType;
            if (type == typeof(Button))
            {
                if (strName.StartsWith("btn") == false)
                {
                    Debug.LogWarning(this.name + ":" + strName + " 命名不规范,Button 以 btn 开头");
                }
            }
            else if (type == typeof(Image))
            {
                if (strName.StartsWith("img") == false)
                {
                    Debug.LogWarning(this.name + ":" + strName + " 命名不规范,Image 以 img 开头");
                }
            }
            else if (type == typeof(Text))
            {
                if (strName.StartsWith("txt") == false)
                {
                    Debug.LogWarning(this.name + ":" + strName + " 命名不规范,Text 以 txt 开头");
                }
            }
            else if (type == typeof(Toggle))
            {
                if (strName.StartsWith("tog") == false)
                {
                    Debug.LogWarning(this.name + ":" + strName + " 命名不规范,Toggle 以 tog 开头");
                }
            }
            else if (type == typeof(Transform))
            {
                if (strName.StartsWith("tra") == false)
                {
                    Debug.LogWarning(this.name + ":" + strName + " 命名不规范,Transform 以 tra 开头");
                }
            }
            else if (type == typeof(GameObject))
            {
                if (strName.StartsWith("obj") == false)
                {
                    Debug.LogWarning(this.name + ":" + strName + " 命名不规范,GameObject 以 obj 开头");
                }
            }
            else if (type.BaseType == typeof(Array))
            {
                if (strName.StartsWith("arr") == false)
                {
                    Debug.LogWarning(this.name + ":" + strName + " 命名不规范,数组 以 arr 开头");
                }
            }
        }

        public static void setActive(MonoBehaviour m, bool active)
        {
            if (m != null)
            {
                m.gameObject.SetActive(active);
            }
        }

        public static Type GetType(string TypeName)
        {
            var type = Type.GetType(TypeName);
            if (type != null)
            {
                return type;
            }
            var assemblyName = TypeName.Substring(0, TypeName.LastIndexOf('.'));
            var assembly = Assembly.Load(assemblyName);
            //var assembly = Assembly.LoadWithPartialName( assemblyName );
            if (assembly == null)
            {
                return null;
            }
            type = assembly.GetType(TypeName);
            //		if (type==null) {
            //			if (TypeName=="UnityEngine.UI.Text") {
            //				return typeof(UnityEngine.UI.Text);
            //			}
            //			else if (TypeName=="UnityEngine.UI.Button") {
            //				return typeof(UnityEngine.UI.Button);
            //			}
            //			else if (TypeName=="UnityEngine.UI.Toggle") {
            //				return typeof(UnityEngine.UI.Toggle);
            //			}
            //			else if (TypeName=="UnityEngine.UI.Image") {
            //				return typeof(UnityEngine.UI.Image);
            //			}
            //		}
            return type;
        }

        #endregion
    }
}
