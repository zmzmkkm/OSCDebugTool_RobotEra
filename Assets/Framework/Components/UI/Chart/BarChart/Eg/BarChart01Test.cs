// ========================================================
// 描 述：柱形图01示例
// 作 者：SW
// 创建时间：2024/01/26 10:01:11
// 版 本：v 1.0
// ========================================================

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Prospect
{
    public class BarChart01Test : MonoBehaviour
    {
        private BarChartVerticalCtrl _barChartVerticalCtrl;
       

        void Start()
        {
            _barChartVerticalCtrl = this.GetComponent<BarChartVerticalCtrl>();
            _barChartVerticalCtrl.setBarActionCategory = SetBarAction;

            _barChartVerticalCtrl.Init();
            _barChartVerticalCtrl.SetAttribute();
            _barChartVerticalCtrl.dataCategory = new()
            {
                new BarCharCategoryData
                {
                    key = "07-01", values = new()
                    {
                        new BarCharValue { value = 518 },
                    }
                },
                new BarCharCategoryData
                {
                    key = "07-02", values = new()
                    {
                        new BarCharValue { value = 613 }
                    }
                },
                new BarCharCategoryData
                {
                    key = "07-03", values = new()
                    {
                        new BarCharValue { value = 345 }
                    }
                },
                new BarCharCategoryData
                {
                    key = "07-04", values = new()
                    {
                        new BarCharValue { value = 425 }
                    }
                },
                new BarCharCategoryData
                {
                    key = "07-05", values = new()
                    {
                        new BarCharValue { value = 647 }
                    }
                }
            };


            _barChartVerticalCtrl.Refresh();
        }

        
        public List<TMP_Text> aaa;
        public Sprite sprit;
        private void SetBarAction(string arg1, int arg2, int arg3, Image arg4, Image arg5, Image arg6, TMP_Text arg7)
        {
            // if (arg2 == 0 && arg3 == 0)
            // {
            //     aaa[0].text = arg7.text;
            //     aaa[0].transform.position = new Vector3(arg7.transform.position.x, aaa[0].transform.position.y, 0);
            // }
            //
            // if (arg2 == 1 && arg3 == 0)
            // {
            //     aaa[1].text = arg7.text;
            //     aaa[1].color = Color.red;
            //     arg7.color = Color.cyan;
            //     aaa[1].transform.position = new Vector3(arg7.transform.position.x, aaa[1].transform.position.y, 0);
            //     arg4.sprite = sprit;
            // }
            //
            // if (arg2 == 2 && arg3 == 0)
            // {
            //     aaa[2].text = arg7.text;
            //     aaa[2].transform.position = new Vector3(arg7.transform.position.x, aaa[2].transform.position.y, 0);
            // }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _barChartVerticalCtrl.dataCategory = new List<BarCharCategoryData>
                {
                    new BarCharCategoryData
                    {
                        key = "07-01", values = new List<BarCharValue>
                        {
                            new BarCharValue { value = 479 },
                        }
                    },
                    new BarCharCategoryData
                    {
                        key = "07-02", values = new List<BarCharValue>
                        {
                            new BarCharValue { value = 313 }
                        }
                    },
                    new BarCharCategoryData
                    {
                        key = "07-03", values = new List<BarCharValue>
                        {
                            new BarCharValue { value = 945 }
                        }
                    },
                    new BarCharCategoryData
                    {
                        key = "07-11", values = new List<BarCharValue>
                        {
                            new BarCharValue { value = 625 }
                        }
                    }
                };
                _barChartVerticalCtrl.Refresh();
            }
        }
    }
}