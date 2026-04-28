using System.Collections;
using UnityEngine;
using System.Text;
using UnityEngine.Assertions;

namespace UniRx.Examples
{
    public class Sample14_ObservableWebRequest : MonoBehaviour
    {
        private void Start()
        {
            //ObservableUnityWebRequest.GetAsObservable("http://www.baidu.com/").Subscribe(responseBody => Debug.Log(responseBody));

            //StartCoroutine(GetAsObservable());
            //StartCoroutine(GetTexture2DAsObservable());
            //StartCoroutine(GetAudioClipAsObservable());
            //StartCoroutine(GetAssetBundleAsObservable());
            //StartCoroutine(HttpGetAsObservable());
            //StartCoroutine(HttpPostAsObservable());
            //StartCoroutine(HttpPutAsObservable());
            //StartCoroutine(HttpDeleteAsObservable());
            //StartCoroutine(HttpHeadAsObservable());
            StartCoroutine(Progress());
        }

        public IEnumerator GetAsObservable()
        {
            var yieldInstruction = ObservableUnityWebRequest
                .GetAsObservable($"file://{Application.dataPath}/Z--NoUse/UniRx/Tests/Runtime/Fixtures/Text.txt")
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            Debug.Log("输出结果: " + yieldInstruction.Result);
            //Assert.AreEqual("Text", yieldInstruction.Result);
        }

        public IEnumerator GetTexture2DAsObservable()
        {
            var yieldInstruction = ObservableUnityWebRequest
                .GetTexture2DAsObservable($"file://{Application.dataPath}/Z--NoUse/UniRx/Tests/Runtime/Fixtures/Texture2D.png")
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            Debug.LogFormat("输出宽：{0}  高：{1}", yieldInstruction.Result.width, yieldInstruction.Result.height);
            //Assert.AreEqual(423, yieldInstruction.Result.width);
            //Assert.AreEqual(500, yieldInstruction.Result.height);
        }

        public IEnumerator GetAudioClipAsObservable()
        {
            var yieldInstruction = ObservableUnityWebRequest
                .GetAudioClipAsObservable($"file://{Application.dataPath}/Z--NoUse/UniRx/Tests/Runtime/Fixtures/AudioClip.ogg", AudioType.OGGVORBIS)
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            Debug.LogFormat("输出音频长度: {0}", yieldInstruction.Result.length);
            // Assert.AreEqual(8, yieldInstruction.Result.length);
        }

        public IEnumerator GetAssetBundleAsObservable()
        {
            var yieldInstruction = ObservableUnityWebRequest
                .GetAssetBundleAsObservable($"file://{Application.dataPath}/Z--NoUse/UniRx/Tests/Runtime/Fixtures/AssetBundle.assetbundle", 0U, 0U)
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            var assetBundle = yieldInstruction.Result;
            var assetBundleRequest = assetBundle.LoadAssetAsync<TextAsset>(assetBundle.GetAllAssetNames()[0]);
            yield return assetBundleRequest;

            Debug.LogFormat("输出TextAsset类型: " + assetBundleRequest.asset.GetType());
            Debug.LogFormat("输出Sample\n:  " + (assetBundleRequest.asset as TextAsset)?.text);
        }

        public IEnumerator HttpGetAsObservable()
        {
            var yieldInstruction = ObservableUnityWebRequest
                .GetAsObservable("https://www.fastmock.site/mock/dc917923365b35db2d3a30c0bb408387/_test/firg/test2")
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            Debug.LogFormat("输出接口：{0}", yieldInstruction.Result);
        }

        public IEnumerator HttpPostAsObservable()
        {
            yield return PrepareForPost();

            var yieldInstruction = ObservableUnityWebRequest
                .PostAsObservable("https://www.fastmock.site/mock/dc917923365b35db2d3a30c0bb408387/_test/firg/test1", "username=zhangsan")
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            Debug.LogFormat("输出返回结果: {0}", yieldInstruction.Result);
        }

        public IEnumerator HttpPutAsObservable()
        {
            var yieldInstruction = ObservableUnityWebRequest
                .PutAsObservable("https://www.fastmock.site/mock/dc917923365b35db2d3a30c0bb408387/_test/firg/test3",
                    Encoding.UTF8.GetBytes("username=zhangsan"))
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            Debug.LogFormat("输出返回结果: {0}", yieldInstruction.Result);
        }

        public IEnumerator HttpDeleteAsObservable()
        {
            yield return PrepareForDelete();

            var yieldInstruction = ObservableUnityWebRequest
                .DeleteAsObservable("https://www.fastmock.site/mock/dc917923365b35db2d3a30c0bb408387/_test/firg/test4")
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            Debug.LogFormat("输出返回结果: {0}", yieldInstruction.Result);
        }

        public IEnumerator HttpHeadAsObservable()
        {
            var yieldInstruction = ObservableUnityWebRequest
                .HeadAsObservable("https://www.fastmock.site/mock/dc917923365b35db2d3a30c0bb408387/_test/firg/test5")
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            var responseHeaders = yieldInstruction.Result;
            Debug.LogFormat("输出返回结果: {0}", responseHeaders["Content-Type"]);
            //Assert.AreEqual("application/json; charset=utf-8", responseHeaders["Content-Type"]);
            //Assert.AreEqual("Express", responseHeaders["X-Powered-By"]);
        }

        public IEnumerator Progress()
        {
            var progress = new ScheduledNotifier<float>();
            var hasReported = false;
            var latestProgress = 0.0f;
            var reportCount = 0;
            progress
                .Subscribe(
                    x =>
                    {
                        hasReported = true;
                        latestProgress = x;
                        reportCount++;
                    }
                );
            var yieldInstruction = ObservableUnityWebRequest
                .GetAsObservable("https://www.fastmock.site/mock/dc917923365b35db2d3a30c0bb408387/_test/firg/test-0", null, progress)
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            Debug.LogFormat("输出hasReported: {0}", hasReported);
            Debug.LogFormat("输出latestProgress: {0}", latestProgress);
            Debug.LogFormat("输出reportCount: {0}", reportCount);
            //Assert.True(hasReported);
            //Assert.AreEqual(1.0f, latestProgress);
            //Assert.GreaterOrEqual(reportCount, 1);
        }

        /// <summary>
        /// Remove record if exists for idempotency
        /// </summary>
        /// <returns></returns>
        private static IEnumerator PrepareForPost()
        {
            var yieldInstruction = ObservableUnityWebRequest
                .HeadAsObservable("https://www.fastmock.site/mock/dc917923365b35db2d3a30c0bb408387/_test/firg/test1")
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            if (!yieldInstruction.HasError)
            {
                yield return ObservableUnityWebRequest
                    .DeleteAsObservable("https://www.fastmock.site/mock/dc917923365b35db2d3a30c0bb408387/_test/firg/test1")
                    .ToYieldInstruction(false);
            }
        }

        /// <summary>
        /// Put record if not exists for idempotency
        /// </summary>
        /// <returns></returns>
        private static IEnumerator PrepareForDelete()
        {
            var yieldInstruction = ObservableUnityWebRequest
                .HeadAsObservable("https://www.fastmock.site/mock/dc917923365b35db2d3a30c0bb408387/_test/firg/test4")
                .ToYieldInstruction(false);
            yield return yieldInstruction;
            if (yieldInstruction.HasError && yieldInstruction.Error is UnityWebRequestErrorException.NotFound)
            {
                yield return ObservableUnityWebRequest
                    .PostAsObservable("https://www.fastmock.site/mock/dc917923365b35db2d3a30c0bb408387/_test/firg/test4",
                        Encoding.UTF8.GetBytes("username=zhangsan"))
                    .ToYieldInstruction(false);
            }
        }
    }
}