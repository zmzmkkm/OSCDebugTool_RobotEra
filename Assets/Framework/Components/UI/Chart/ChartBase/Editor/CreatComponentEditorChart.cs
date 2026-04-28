using UnityEditor;
using UnityEngine;

namespace Prospect.Components.UI.Chart.BarChart.Editor
{
    public class CreatComponentEditorChart : MonoBehaviour
    {
        private const string path_head = "Assets/Framework/Components/";
        private const string path = "UI/Chart/";

        [MenuItem("GameObject/SW_UI/BarChart/BarChartVertical", false, 10)]
        private static void LoadPrefab1()
        {
            var parent = Selection.activeGameObject.transform;
            if (!parent)
            {
                var canvasTransform = GameObject.Find("Canvas").transform;
                if (canvasTransform)
                {
                    parent = canvasTransform;
                }
            }

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path_head + path + "BarChart/BarChartVertical.prefab");
            var go = Instantiate(prefab, parent);
            go.name = "BarChartVertical";
            go.transform.localPosition = Vector3.zero;
        }
        
        [MenuItem("GameObject/SW_UI/BarChart/BarChartHorizontal", false, 10)]
        private static void LoadPrefab2()
        {
            var parent = Selection.activeGameObject.transform;
            if (!parent)
            {
                var canvasTransform = GameObject.Find("Canvas").transform;
                if (canvasTransform)
                {
                    parent = canvasTransform;
                }
            }

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path_head + path + "BarChart/BarChartHorizontal.prefab");
            var go = Instantiate(prefab, parent);
            go.name = "BarChartHorizontal";
            go.transform.localPosition = Vector3.zero;
        }
        
        [MenuItem("GameObject/SW_UI/BarChart/VerticalEgs", false, 10)]
        private static void LoadPrefab3()
        {
            var parent = Selection.activeGameObject.transform;
            if (!parent)
            {
                var canvasTransform = GameObject.Find("Canvas").transform;
                if (canvasTransform)
                {
                    parent = canvasTransform;
                }
            }

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path_head + path + "BarChart/Eg/BarVerticalEgs.prefab");
            var go = Instantiate(prefab, parent);
            go.name = "BarVerticalEgs";
            go.transform.localPosition = Vector3.zero;
        }
        
        [MenuItem("GameObject/SW_UI/BarChart/HorizontalEgs", false, 10)]
        private static void LoadPrefab4()
        {
            var parent = Selection.activeGameObject.transform;
            if (!parent)
            {
                var canvasTransform = GameObject.Find("Canvas").transform;
                if (canvasTransform)
                {
                    parent = canvasTransform;
                }
            }

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path_head + path + "BarChart/Eg/BarHorizontalEgs.prefab");
            var go = Instantiate(prefab, parent);
            go.name = "BarHorizontalEgs";
            go.transform.localPosition = Vector3.zero;
        }
        
        
        [MenuItem("GameObject/SW_UI/LineChart", false, 10)]
        private static void LoadPrefab5()
        {
            var parent = Selection.activeGameObject.transform;
            if (!parent)
            {
                var canvasTransform = GameObject.Find("Canvas").transform;
                if (canvasTransform)
                {
                    parent = canvasTransform;
                }
            }

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path_head + path + "LineChart/LineChart.prefab");
            var go = Instantiate(prefab, parent);
            go.name = "LineChart";
            go.transform.localPosition = Vector3.zero;
        }
        
       
    }
}