// ========================================================
// 描 述：chartHelper扩展类，用于折线图标记的custom
// 作 者：张成
// 创建时间：2023/12/21 10:44:11
// 版 本：v 1.0
// ========================================================
using System.Collections;
using System.Configuration;
using UnityEngine;
using UnityEngine.UI;


namespace XCharts.Runtime
{
    public static class ChartHelperExtension
	{
        static int index = 0;
        public static void AddLineSeriesIcon(string name, Transform parent, SymbolStyle symbolStyle,Vector3 dataPos,bool isOpenCustom = true)
        {
            if (isOpenCustom)
            {
                if (parent.childCount == 19)
                {
                    for (int i = 0; i < parent.childCount; i++)
                    {
                        Vector3 scale = parent.GetChild(i).localScale;
                        parent.GetChild(i).localScale = scale == Vector3.zero ? Vector3.one : Vector3.one;
                        SymbolIcon symbol = parent.GetChild(i).GetComponent<SymbolIcon>();                        
                        Debug.Log("---symbol---: " + symbol.m_img.sprite);
                        symbol.UpdateIcon(symbolStyle, Color.white, symbolStyle.image);
                    }
                }
                else
                {
                    index += 1;
                    Debug.Log("添加物体: " + index);
                    var sizeDelta = new Vector2(symbolStyle.width, symbolStyle.height);
                    GameObject iconObj = AddObject(name, parent, sizeDelta);
                    iconObj.GetComponent<CanvasRenderer>();
                    Image img = ChartHelper.EnsureComponent<Image>(iconObj);
                    SymbolIcon symbol = ChartHelper.EnsureComponent<SymbolIcon>(iconObj);
                    symbol.InitIcon();
                    symbol.m_img.sprite = symbolStyle.image;
                    symbol.m_img.raycastTarget = false;
                    symbol.m_img.color = Color.white;
                    symbol.SetSize(symbolStyle.width, symbolStyle.height);
                    symbol.transform.localScale = Vector3.one;
                    iconObj.transform.localPosition = dataPos;
                    Debug.Log($"<color=red>iconObj: {iconObj} ___ pos: {dataPos} </color>");
                    symbol.UpdateIcon(symbolStyle, Color.white, symbolStyle.image);
                }               
            }
            else
            {
                if (parent.childCount > 0)
                {
                    for (int i = 0; i < parent.childCount; i++)
                    {
                        parent.GetChild(i).localScale = Vector3.zero;
                    }
                }
            }
        }

        public static GameObject AddObject(string name, Transform parent,Vector2 sizeDelta, int replaceIndex = -1)
        {
            GameObject obj;
            if (parent.Find(name))
            {
                obj = parent.Find(name).gameObject;
                ChartHelper.SetActive(obj, true);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (replaceIndex >= 0 && replaceIndex < parent.childCount)
            {
                obj = parent.GetChild(replaceIndex).gameObject;
                if (!obj.name.Equals(name)) obj.name = name;
                ChartHelper.SetActive(obj, true);
            }
            else
            {
                obj = new GameObject();
                obj.name = name;
                obj.transform.SetParent(parent);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                obj.layer = parent.gameObject.layer;
            }
            RectTransform rect = ChartHelper.EnsureComponent<RectTransform>(obj);
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = sizeDelta;
            rect.anchorMin = Vector2.one * .5f;
            rect.anchorMax = Vector2.one * .5f;
            rect.pivot = Vector2.one * .5f;
            rect.anchoredPosition3D = Vector3.zero;
            return obj;
        }

        public static Image AddIconObject(string objectName, Transform parent, Vector2 sizeDelta, SymbolStyle symbolStyle)
        {
            GameObject iconObj = AddObject(objectName, parent,sizeDelta);       
            iconObj.layer = parent.gameObject.layer;
            
            Image img = ChartHelper.EnsureComponent<Image>(iconObj);
            img.raycastTarget = false;     
            if (symbolStyle.width == 0 || symbolStyle.height == 0)
            {
                img.SetNativeSize();
            }

            img.gameObject.SetActive(symbolStyle.show);
            img.type = symbolStyle.imageType;
            img.sprite = symbolStyle.image;

            RectTransform rect = ChartHelper.EnsureComponent<RectTransform>(iconObj);
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = sizeDelta;
            rect.anchorMin = Vector2.one * .5f;
            rect.anchorMax = Vector2.one * .5f;
            rect.pivot = Vector2.one * .5f;
            return img;
        }
    }	
}