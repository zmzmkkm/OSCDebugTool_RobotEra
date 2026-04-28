// ========================================================
// 描 述：只适用于折线图的symbol下custom类型
// 作 者：张成
// 创建时间：2023/12/21 09:57:38
// 版 本：v 1.0
// ========================================================
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
	public class SymbolIcon : MonoBehaviour
	{  
        private float m_Width = 0;
        private float m_Height = 0;
        public Image m_img;
        //private RectTransform m_IconRect;
        public RectTransform m_ObjectRect;
        private bool m_Active = true;

        public bool isIconActive { get; private set; }
                  

        public void InitIcon()
        {
            m_img = transform.GetComponent<Image>();
            m_ObjectRect = gameObject.GetComponent<RectTransform>();
        }    

        public float GetWidth()
        {
            return m_Width;
        }

        public float GetHeight()
        {
            return m_Height;
        }

        public void SetSize(float width, float height)
        {
            this.m_Width = width;
            this.m_Height = height;
            m_ObjectRect.sizeDelta = new Vector2(width, height);
        }

        public void SetIconSprite(Sprite sprite)
        {
            if (m_img.sprite != null) 
                m_img.sprite = sprite;
        }

        public void SetIconSize(float width, float height)
        {
            if (m_ObjectRect != null)
                m_ObjectRect.sizeDelta = new Vector3(width, height);
        }

        public void UpdateIcon(SymbolStyle symbolStyle, Color color,Sprite sp)
        {
            if (symbolStyle == null)
                return;
            m_ObjectRect.sizeDelta = new Vector2(symbolStyle.width, symbolStyle.height);
            StartCoroutine(OnDeplay(sp));
        }

        private IEnumerator OnDeplay(Sprite sp)
        {
            while (CanvasUpdateRegistry.IsRebuildingLayout())
            {
                yield return null;
            }
            m_img.sprite = sp;
            
        }

        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        public void SetRectPosition(Vector3 position)
        {
            m_ObjectRect.anchoredPosition3D = position;
        }

        public Vector3 GetPosition()
        {
            return transform.localPosition;
        }

        public bool IsActiveByScale()
        {
            return m_Active;
        }

        public void SetActive(bool flag)
        {
            m_Active = flag;
            ChartHelper.SetActive(gameObject, flag);
        }

        public void SetIconActive(bool flag)
        {
            isIconActive = flag;
            Debug.Log("---symbolIcon.m_img----: " + m_img.sprite);
            if (m_img.sprite) 
                ChartHelper.SetActive(gameObject, flag);
        } 
    }
	
}