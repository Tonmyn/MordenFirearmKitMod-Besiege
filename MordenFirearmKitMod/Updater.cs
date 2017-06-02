using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
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
        //当前Mod版本号
        private Version CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;

        //最新Mod版本号
        public Version LatestVersion { get; private set; }

        //最新Mod发布名称
        public string LatestReleaseName { get; private set; }

        //最新Mod发布介绍
        public string LatestReleaseBody { get; private set; }

        //最新Mod发布地址
        public string url { get; set; }

        //更新Mod可用
        public bool UpdaterEnable = false;

        private string value;

        private Rect windowDialog = new Rect(300, 300, 320, 150);

        private int WindowID = Util.GetWindowID();

        private IEnumerator Start()
        {
            var www = new WWW(url);
            yield return www;

            if (!www.isDone || !string.IsNullOrEmpty(www.error))
            {
                //if (verbose) Debug.Log("=> Unable to connect.");
                Destroy(this);
                yield break;
            }

            value = www.text;

            var release = SimpleJSON.JSON.Parse(value);

            LatestVersion = new Version(release["tag_name"].Value.Trim('v'));
            LatestReleaseName = release["name"].Value;
            LatestReleaseBody = release["body"].Value.Replace(@"\r\n", "\n");

            //比较最新版本和当前版本
            if (LatestVersion > CurrentVersion)
            {
#if DEBUG
                Debug.Log(Assembly.GetExecutingAssembly().GetName().Name + "有新版可以更新");
#endif
                ////取更新内容标题
                //LatestReleaseName = GetData("release-header");

                ////取更新内容
                //LatestReleaseBody = GetData("markdown-body");

                //更新可用为真
                UpdaterEnable = true;
            }
            else
            {
#if DEBUG
                Debug.Log(Assembly.GetExecutingAssembly().GetName().Name + "无需更新");
#endif
            }

        }

        public void Url(string str)
        {
            url = str;
        }

        public void Url(string owner, string path)
        {
            url = "https://git.oschina.net/api/v5/repos/" + owner + "/" + path + "/releases/latest";
        }

        private void OnGUI()
        {
            if (!UpdaterEnable) return;

            GUI.skin = ModGUI.Skin;
            GUI.backgroundColor = new Color(0.7f, 0.7f, 0.7f, 0.7f);

            windowDialog = GUI.Window(WindowID, windowDialog, doWindow, Assembly.GetExecutingAssembly().GetName().Name + " 更新提示");

        }

        private void doWindow(int windowId)
        {
            //画新版资料
            //GUILayout.Label("有新版本可以更新", new GUIStyle(Elements.Labels.Default) { alignment = TextAnchor.MiddleCenter,fontSize = 12 });    

            GUILayout.Label($"<b>v{LatestVersion}: {LatestReleaseName}</b>", new GUIStyle(Elements.Labels.Default) { alignment = TextAnchor.MiddleCenter, fontSize = 15 });

            GUILayout.Label(LatestReleaseBody, new GUIStyle(Elements.Labels.Default) { fontSize = 12, margin = new RectOffset(8, 8, 16, 16) });

            //画更新按钮
            if (GUILayout.Button("去更新页面下载新版", Elements.Buttons.ComponentField))
            {
                Application.OpenURL(url);
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
