/*
 *		Description:  uGUI 界面组辅助器。 
 *		
 *		用法:
 *	    Game/Builtin/UI 上的组件UIComponent下 , UI Group Helper 设置成这个类即可
 *
 *		CreatedBy: guoShuai
 *
 *		DataTime: 2020.07.09
 *
 */
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace guoShuai
{
    /// <summary>
    /// uGUI 界面组辅助器。
    /// </summary>
    public class UGuiGroupHelper : UIGroupHelperBase
    {
        public const int DepthFactor = 10000;

        private int m_Depth = 0;
        private Canvas m_CachedCanvas = null;

        /// <summary>
        /// 设置界面组深度。
        /// </summary>
        /// <param name="depth">界面组深度。</param>
        public override void SetDepth(int depth)
        {
            m_Depth = depth;
            m_CachedCanvas.overrideSorting = true;
            m_CachedCanvas.sortingOrder = DepthFactor * depth;
        }

        private void Awake()
        {
            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            gameObject.GetOrAddComponent<GraphicRaycaster>();
        }

        private void Start()
        {
            m_CachedCanvas.overrideSorting = true;
            m_CachedCanvas.sortingOrder = DepthFactor * m_Depth;

            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
        }
    }
}
