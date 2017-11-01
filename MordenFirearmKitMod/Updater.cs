using System;
using System.Reflection;
using UnityEngine;
using System.Collections;
using spaar.ModLoader;
using spaar.ModLoader.UI;

namespace MordenFirearmKitMod
{ 
    
    //Mod更新检查组件
    public class Updater : MonoBehaviour
    {
        
        //Mod名字
        public static string ModName = Assembly.GetExecutingAssembly().GetName().Name;

        //Mod作者
        public static string Author = "XultimateX";

        //最新Mod版本号
        public Version LatestVersion { get; private set; }

        //最新Mod发布名称
        public string LatestReleaseName { get; private set; }

        //最新Mod发布介绍
        public string LatestReleaseBody { get; private set; }

        //Josn格式的版本地址
        public static string JosnUrl { get; set; }

        //最新Mod发布地址
        public static string Url { get; set; }

        //更新Mod可用
        public bool UpdaterEnable { get; private set; } = false;

        //当前Mod版本号
        private static  Version CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;

        //提示窗口大小
        private Rect windowDialog = new Rect(300, 300, 320, 150);

        //提示窗口ID
        private int windowID = Util.GetWindowID();

        //组件构造函数
        public Updater()
        {
            UrlToJosn(Author, ModName);
        }

        public Updater(string address)
        {
            SetUrl(address);
        }

        public Updater(string owner, string path)
        {
            SetUrl(owner, path);
        }

        //组件更新检查函数
        public IEnumerator Start()
        {
            Debug.Log(ModName + " 开始检查更新...");

            var www = new WWW(JosnUrl);
            yield return www;

            if (!www.isDone || !string.IsNullOrEmpty(www.error))
            {
                Debug.Log(ModName + " 更新信息好像出问题了... ");
                Destroy(this);
                yield break;
            }

            string value = www.text;

            var release = SimpleJSON.JSON.Parse(value);

            LatestVersion = new Version(release["tag_name"].Value.Trim('v'));
            LatestReleaseName = release["name"].Value;
            LatestReleaseBody = release["body"].Value.Replace(@"\r\n", "\n");

            //比较最新版本和当前版本
            if (LatestVersion > CurrentVersion)
            {
#if DEBUG
                Debug.Log(ModName + " 有新版可以更新");
#endif
                //更新可用为真
                UpdaterEnable = true;
            }
            else
            {
#if DEBUG
                Debug.Log(ModName + " 无需更新");
#endif
            }

        }

        //设置更新地址
        public void SetUrl(string str)
        {
            Url = str;
        }

        /// <summary>
        /// 设置更新地址并格式化
        /// </summary>
        /// <param name="owner">作者</param>
        /// <param name="path">git仓库名</param>
        public void SetUrl(string owner, string path)
        {
            //url = "https://git.oschina.net/" + owner + "/" + path + "/releases";
            UrlToJosn(owner, path);
        }

        private void UrlToJosn(string owner,string path)
        {
            JosnUrl = "https://git.oschina.net/api/v5/repos/" + owner + "/" + path + "/releases/latest";
            Url = "https://git.oschina.net/" + owner + "/" + path + "/releases";
        }

        //画提示更新窗口
        private void OnGUI()
        {
            if (!UpdaterEnable) return;

            GUI.skin = ModGUI.Skin;
            GUI.backgroundColor = new Color(0.7f, 0.7f, 0.7f, 0.7f);

            windowDialog = GUI.Window(windowID, windowDialog, doWindow, ModName + " 更新提示");

        }

        //画窗口组件
        private void doWindow(int windowId)
        {
            //画新版资料
            //GUILayout.Label("有新版本可以更新", new GUIStyle(Elements.Labels.Default) { alignment = TextAnchor.MiddleCenter,fontSize = 12 });    

            GUILayout.Label($"<b>v{LatestVersion}: {LatestReleaseName}</b>", new GUIStyle(Elements.Labels.Default) { alignment = TextAnchor.MiddleCenter, fontSize = 15 });

            GUILayout.Label(LatestReleaseBody, new GUIStyle(Elements.Labels.Default) { fontSize = 12, margin = new RectOffset(8, 8, 16, 16) });

            //画更新按钮
            if (GUILayout.Button("去更新页面下载新版", Elements.Buttons.ComponentField))
            {
                Application.OpenURL(Url);
            }

            //画关闭按钮
            if (GUI.Button(new Rect(windowDialog.width - 38, 8, 30, 30), "×", Elements.Buttons.Disabled))
            {
                Destroy(this);
                Debug.Log("拒绝更新无可救药...");
            }

            //使窗口能够拖拽
            GUI.DragWindow();

            windowDialog.height = 134f + LatestReleaseBody.Split('\n').Length * 16f;
        }
    }
}
