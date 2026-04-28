// ========================================================
// 描 述：只适用于折线图的symbol创建icon
// 作 者：张成
// 创建时间：2023/12/21 11:12:04
// 版 本：v 1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace XCharts.Runtime
{
    public class ChartSymbolIcon
	{
        private Image m_Image;

        public Image image
        {
            get { return m_Image; }
            set { m_Image = value; }
        }

        public GameObject gameObject
        {
            get
            {
                if (m_Image != null) return m_Image.gameObject;
                return null;
            }
        }

        public void SetActive(bool flag)
        {
            if (m_Image != null) 
                ChartHelper.SetActive(m_Image.gameObject, flag);
        }

        public void SetLocalPosition(Vector3 position)
        {
            if (m_Image != null) 
                m_Image.transform.localPosition = position;
        }

        public void SetRectPosition(Vector3 position)
        {
            if (m_Image != null) 
                m_Image.GetComponent<RectTransform>().anchoredPosition3D = position;
        }

        public void SetSizeDelta(Vector2 sizeDelta)
        {
            if (m_Image != null) 
                m_Image.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        }
    }
}