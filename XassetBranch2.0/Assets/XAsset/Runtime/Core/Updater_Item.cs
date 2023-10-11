//
// Updater.cs
//
// Author:
//       fjy <jiyuan.feng@live.com>
//
// Copyright (c) 2020 fjy
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace libx
{
    [RequireComponent(typeof(Downloader))]
    [RequireComponent(typeof(NetworkMonitor))]
    public class Updater_Item : MonoBehaviour, IUpdater, INetworkMonitorListener
    {
        enum Step
        {
            Wait,
            Copy,
            Coping,
            Versions,
            Prepared,
            Download,
            //IsLoaded
        }

        private Step _step;

        [SerializeField] private string baseURL = "http://127.0.0.1:7888/DLC/";
        [SerializeField] private string gameScene = "Game.unity";
        [SerializeField] private bool enableVFS = true;
        [SerializeField] private bool development;

        public IUpdater listener { get; set; }

        private Downloader _downloader;
        private NetworkMonitor _monitor;
        private string _platform;
        private string _savePath;
        private List<VFile> _versions = new List<VFile>();
        /// <summary>
        /// 加载完成后时间
        /// </summary>
        public Action oncompleted;
        /// <summary>
        /// 是否直接加载
        /// </summary>
        public bool IsLoad = true;
        public void OnMessage(string msg)
        {
            if (listener != null)
            {
                listener.OnMessage(msg);
            }
        }

        public void OnProgress(float progress)
        {
            if (listener != null)
            {
                listener.OnProgress(progress);
            }
        }

        public void OnVersion(string ver)
        {
            if (listener != null)
            {
                listener.OnVersion(ver);
            }
        }

        private void Start()
        {
            _downloader = gameObject.GetComponent<Downloader>();
            _downloader.onUpdate = OnUpdate;
            _downloader.onFinished = OnComplete;

            _monitor = gameObject.GetComponent<NetworkMonitor>();
            _monitor.listener = this;

            _savePath = string.Format("{0}/DLC/", Application.streamingAssetsPath);
            _platform = GetPlatformForAssetBundles(Application.platform);

            _step = Step.Wait;

            Assets.updatePath = _savePath;
        }
        /// <summary>
        ///仿照原来写法，依然从聚焦开始
        /// </summary>
        /// <param name="url"></param>
        public void Restart(string url, string schoolname,string scenename = null)
        {
            
            if (GlobalStatus.IsDownloading==true)
            {
                Stop();
                return;
            }

            if (url != baseURL)
            {
                _downloader.Clear();
                GlobalStatus.IsDownloading = true;
                SendMessage("ShowProgess", "加载中");
                _savePath = string.Format("{0}/{1}/", Application.streamingAssetsPath, schoolname);
                Assets.updatePath = _savePath;
                Debug.Log(_savePath);
                gameScene = scenename;
                //跳过VFS设置一次整合
                baseURL = url;
                _step = Step.Copy;
                StartUpdate();
            }
            else
            {
                _downloader.Restart();
                //_step = Step.Copy;
                GlobalStatus.IsDownloading = true;
            }

        }
        public void LoadItem(string scenename)
        {
            _savePath = string.Format("{0}/{1}/", Application.streamingAssetsPath, "20上海城建职业学院元宇宙校园");
            Assets.updatePath = _savePath;
            //baseURL = url;
            //IsLoad = true;
            gameScene = scenename;
            _step = Step.Prepared;
            StartCoroutine(Checking());
        }
        public void Stop()
        {
            _downloader.Stop();
            GlobalStatus.IsDownloading = false;
            //_step = Step.Copy;

        }
        /// <summary>
        /// 直接加载
        /// </summary>
        public void LoadItem(string url, string schoolname, string scenename = null)
        {
            _savePath = string.Format("{0}/{1}/", Application.streamingAssetsPath, schoolname);
            Assets.updatePath = _savePath;
            baseURL = url;
            IsLoad = true;
            gameScene = scenename;
            //_step = Step.Prepared;
            StartCoroutine(Checking());
        }
        /// <summary>
        /// 是否存在这一目录
        /// </summary>
        /// <param name="url"></param>
        /// <param name="schoolname"></param>
        /// <param name="scenename"></param>
        /// <returns></returns>
        public bool CheckItem(string url, string schoolname, string scenename = null)
        {
            _savePath = string.Format("{0}/{1}/", Application.streamingAssetsPath, schoolname);
            if (File.Exists(_savePath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void OnApplicationFocus(bool hasFocus)
        {
            //if (_reachabilityChanged || _step == Step.Wait)
            //{
            //    return;
            //}

            //if (hasFocus)
            //{
            //    MessageBox.CloseAll();
            //    if (_step == Step.Download)
            //    {
            //        _downloader.Restart();
            //    }
            //    else
            //    {
            //        StartUpdate();
            //    }
            //}
            //else
            //{
            //    if (_step == Step.Download)
            //    {
            //        _downloader.Stop();
            //    }
            //}
        }

        private bool _reachabilityChanged;

        public void OnReachablityChanged(NetworkReachability reachability)
        {
            if (_step == Step.Wait)
            {
                return;
            }

            _reachabilityChanged = true;
            if (_step == Step.Download)
            {
                _downloader.Stop();
            }

            if (reachability == NetworkReachability.NotReachable)
            {
                MessageBox.Show("提示！", "找不到网络，请确保手机已经联网", "确定", "退出").onComplete += delegate (MessageBox.EventId id)
                {
                    if (id == MessageBox.EventId.Ok)
                    {
                        if (_step == Step.Download)
                        {
                            _downloader.Restart();
                        }
                        else
                        {
                            StartUpdate();
                        }
                        _reachabilityChanged = false;
                    }
                    else
                    {
                        Quit();
                    }
                };
            }
            else
            {
                if (_step == Step.Download)
                {
                    _downloader.Restart();
                }
                else
                {
                    StartUpdate();
                }
                _reachabilityChanged = false;
                MessageBox.CloseAll();
            }
        }

        private void OnUpdate(long progress, long size, float speed)
        {
            Debug.Log(string.Format("下载中...{0}/{1}, 速度：{2}",
                Downloader.GetDisplaySize(progress),
                Downloader.GetDisplaySize(size),
                Downloader.GetDisplaySpeed(speed)));
            int progress_value =(int)(progress*100/ size) ;
            string text = "下载中..." + progress_value+"%";
            SendMessage("ShowProgess", text, SendMessageOptions.DontRequireReceiver);

            SendMessage("DownProgess", (float)progress*1f/size, SendMessageOptions.DontRequireReceiver);
            SendMessage("OnProgess", text, SendMessageOptions.DontRequireReceiver);

            OnProgress(progress * 1f / size);
        }

        public void Clear()
        {
            MessageBox.Show("提示", "清除数据后所有数据需要重新下载，请确认！", "清除").onComplete += id =>
            {
                if (id != MessageBox.EventId.Ok)
                    return;
                OnClear();
            };
        }

        public void OnClear()
        {
            OnMessage("数据清除完毕");
            OnProgress(0);
            _versions.Clear();
            _downloader.Clear();
            _step = Step.Wait;
            _reachabilityChanged = false;

            Assets.Clear();

            if (listener != null)
            {
                listener.OnClear();
            }

            if (Directory.Exists(_savePath))
            {
                Directory.Delete(_savePath, true);
            }
        }

        public void OnStart()
        {
            if (listener != null)
            {
                listener.OnStart();
            }
        } 

        private IEnumerator _checking;

        public void StartUpdate()
        {
            Debug.Log("StartUpdate.Development:" + development);
#if UNITY_EDITOR
            if (development)
            {
                Assets.runtimeMode = false;
                StartCoroutine(LoadGameScene());
                return;
            }
#endif
            OnStart();

            if (_checking != null)
            {
                StopCoroutine(_checking);
            }

            _checking = Checking();

            StartCoroutine(_checking);
        }

        private void AddDownload(VFile item)
        {
            _downloader.AddDownload(GetDownloadURL(item.name), item.name, _savePath + item.name, item.hash, item.len);
        }

        private void PrepareDownloads()
        {
            if (enableVFS)
            {
                var path = string.Format("{0}{1}", _savePath, Versions.Dataname);
                if (!File.Exists(path))
                {
                    AddDownload(_versions[0]);
                    return;
                }

                Versions.LoadDisk(path);
            }

            for (var i = 1; i < _versions.Count; i++)
            {
                var item = _versions[i];
                if (Versions.IsNew(string.Format("{0}{1}", _savePath, item.name), item.len, item.hash))
                {
                    AddDownload(item);
                }
            }
        }

        private IEnumerator RequestVFS()
        {
            var mb = MessageBox.Show("提示", "是否开启VFS？开启有助于提升IO性能和数据安全。", "开启");
            yield return mb;
            enableVFS = mb.isOk;
        }

        private static string GetPlatformForAssetBundles(RuntimePlatform target)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (target)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "Windows";
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return "iOS"; // OSX
                default:
                    return null;
            }
        }

        private string GetDownloadURL(string filename)
        {
            return string.Format("{0}/{1}/{2}", baseURL, _platform, filename);
        }

        private IEnumerator Checking()
        {
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            Debug.Log(_step);
            if (_step == Step.Wait)
            {
                //yield return RequestVFS();
                _step = Step.Copy;
            }

            if (_step == Step.Copy)
            {
                yield return RequestCopy();
            }

            if (_step == Step.Coping)
            {
                var path = _savePath + Versions.Filename + ".tmp";
                var versions = Versions.LoadVersions(path);
                var basePath = GetStreamingAssetsPath() + "/";
                yield return UpdateCopy(versions, basePath);
                _step = Step.Versions;
            }

            if (_step == Step.Versions)
            {
                yield return RequestVersions();
            }

            if (_step == Step.Prepared)
            {
                Debug.Log("正在检查版本信息...");
                var totalSize = _downloader.size;
                if (totalSize > 0)
                {
                    _downloader.StartDownload();
                    _step = Step.Download;
                    Debug.Log("SetDownLoad");
                    Debug.Log(string.Format("发现内容更新，总计需要下载 {0} 内容", Downloader.GetDisplaySize(totalSize)));
                    //var tips = string.Format("发现内容更新，总计需要下载 {0} 内容", Downloader.GetDisplaySize(totalSize));
                    //var mb = MessageBox.Show("提示", tips, "下载", "退出");
                    //yield return mb;.
                    //if (mb.isOk)
                    //{
                    //    _downloader.StartDownload();
                    //    _step = Step.Download;
                    //}
                    //else
                    //{
                    //    Quit();
                    //} 
                }
                else
                {
                    OnComplete();
                }
            }
            else
            {
                StartCoroutine(LoadGameScene());
            }
            //if(_step == Step.IsLoaded)
            //{
            //    StartCoroutine(LoadGameScene());
            //}
        }

        private IEnumerator RequestVersions()
        {
            OnMessage("正在获取版本信息...");
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                var mb = MessageBox.Show("提示", "请检查网络连接状态", "重试", "退出");
                yield return mb;
                if (mb.isOk)
                {
                    StartUpdate();
                }
                else
                {
                    Quit();
                } 
                yield break;
            }

            var request = UnityWebRequest.Get(GetDownloadURL(Versions.Filename));
            Debug.Log(GetDownloadURL(Versions.Filename));
            request.downloadHandler = new DownloadHandlerFile(_savePath + Versions.Filename);
            yield return request.SendWebRequest();
            var error = request.error;
            request.Dispose();
            if (!string.IsNullOrEmpty(error))
            {
                var mb = MessageBox.Show("提示", string.Format("获取服务器版本失败：{0}", error), "重试", "退出");
                yield return mb;
                if (mb.isOk)
                {
                    StartUpdate();
                }
                else
                {
                    Quit();
                } 
                yield break; 
            } 
            try
            {
                _versions = Versions.LoadVersions(_savePath + Versions.Filename, true);
                if (_versions.Count > 0)
                {
                    PrepareDownloads();
                    _step = Step.Prepared;
                }
                else
                {
                    OnComplete();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                MessageBox.Show("提示", "版本文件加载失败", "重试", "退出").onComplete +=
                    delegate(MessageBox.EventId id)
                    {
                        if (id == MessageBox.EventId.Ok)
                        {
                            StartUpdate();
                        }
                        else
                        {
                            Quit();
                        }
                    };
            }
        }

        private static string GetStreamingAssetsPath()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return Application.streamingAssetsPath;
            }

            if (Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor)
            {
                return "file:///" + Application.streamingAssetsPath;
            }

            return "file://" + Application.streamingAssetsPath;
        }

        private IEnumerator RequestCopy()
        {
            Debug.Log("开始检查版本");
            var v1 = Versions.LoadVersion(_savePath + Versions.Filename);
            var basePath = GetStreamingAssetsPath() + "/";
            var request = UnityWebRequest.Get(basePath + Versions.Filename);
            var path = _savePath + Versions.Filename + ".tmp";
            request.downloadHandler = new DownloadHandlerFile(path);
            yield return request.SendWebRequest();
            if (string.IsNullOrEmpty(request.error))
            {
                var v2 = Versions.LoadVersion(path);
                if (v2 > v1)
                {
                    var mb = MessageBox.Show("提示", "是否将资源解压到本地？", "解压", "跳过");
                    yield return mb;
                    _step = mb.isOk ? Step.Coping : Step.Versions;
                }
                else
                {
                    Versions.LoadVersions(path);
                    _step = Step.Versions;
                }
            }
            else
            {
                _step = Step.Versions;
            } 
            request.Dispose();
        }

        private IEnumerator UpdateCopy(IList<VFile> versions, string basePath)
        {
            var version = versions[0];
            if (version.name.Equals(Versions.Dataname))
            {
                var request = UnityWebRequest.Get(basePath + version.name);
                request.downloadHandler = new DownloadHandlerFile(_savePath + version.name);
                var req = request.SendWebRequest();
                while (!req.isDone)
                {
                    OnMessage("正在复制文件");
                    OnProgress(req.progress);
                    yield return null;
                }

                request.Dispose();
            }
            else
            {
                for (var index = 0; index < versions.Count; index++)
                {
                    var item = versions[index];
                    var request = UnityWebRequest.Get(basePath + item.name);
                    request.downloadHandler = new DownloadHandlerFile(_savePath + item.name);
                    yield return request.SendWebRequest();
                    request.Dispose();
                    OnMessage(string.Format("正在复制文件：{0}/{1}", index, versions.Count));
                    OnProgress(index * 1f / versions.Count);
                }
            }
        }

        private void OnComplete()
        {
            Debug.Log("更新完成");
            if (enableVFS)
            {
                var dataPath = _savePath + Versions.Dataname;
                var downloads = _downloader.downloads;
                if (downloads.Count > 0 && File.Exists(dataPath))
                {
                    OnMessage("更新本地版本信息");
                    var files = new List<VFile>(downloads.Count);
                    foreach (var download in downloads)
                    {
                        files.Add(new VFile
                        {
                            name = download.name,
                            hash = download.hash,
                            len = download.len,
                        });
                    }

                    var file = files[0];
                    if (!file.name.Equals(Versions.Dataname))
                    {
                        Versions.UpdateDisk(dataPath, files);
                    }
                }

                Versions.LoadDisk(dataPath);
            }

            OnProgress(1);
            OnMessage("更新完成");
            var version = Versions.LoadVersion(_savePath + Versions.Filename);
            if (version > 0)
            {
                OnVersion(version.ToString());
            }
            oncompleted?.Invoke();
            GlobalStatus.IsDownloading = false;
            //_step = Step.IsLoaded;
            if (IsLoad)
            {
                StartCoroutine(LoadGameScene());
            }
        }

        private IEnumerator LoadGameScene()
        {
            Debug.Log(gameScene);
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            OnMessage("正在初始化");
            var init = Assets.Initialize();
            yield return init;
            if (string.IsNullOrEmpty(init.error))
            {
                Assets.AddSearchPath("Assets/Resources/Scenes");
                init.Release();
                OnProgress(0);
                OnMessage("加载游戏场景");
                var scene = Assets.LoadSceneAsync(gameScene, false);
                while (!scene.isDone)
                {
                    OnProgress(scene.progress);
                    yield return null;
                }
            }
            else
            {
                init.Release();
                var mb = MessageBox.Show("提示", "初始化异常错误：" + init.error + "请联系技术支持");
                yield return mb;
                Quit();
            }
        }

        private void OnDestroy()
        {
            MessageBox.Dispose();
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
