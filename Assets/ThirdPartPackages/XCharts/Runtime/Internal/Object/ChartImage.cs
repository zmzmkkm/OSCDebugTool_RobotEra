// ========================================================
// 描 述：
// 作 者：SW
// 创建时间：2024/04/18 11:14:38
// 版 本：v 1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace XCharts.Runtime
{
    public class ChartImage : MonoBehaviour
    {
        public RectTransform _imagRect;
        public Image _image;

        public Image Image
        {
            get { return _image; }
            set { SetIcon(value); }
        }

        public void SetIcon(Image image)
        {
            _image = image;
            if (image != null)
            {
                _imagRect = _image.GetComponent<RectTransform>();
            }
        }

        public void SetSize(Vector2 size)
        {
            _imagRect.sizeDelta = size;
        }

        public void UpdateImage(Sprite sprite = null, Color color = default(Color))
        {
            if (_image == null)
                return;

            
            _image.sprite = sprite;
            _image.color = color;
            
        }
    }
}