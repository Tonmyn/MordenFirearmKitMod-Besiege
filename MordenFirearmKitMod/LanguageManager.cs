using Localisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ModernFirearmKitMod
{
    public class LanguageManager : SingleInstance<LanguageManager>
    {
        public override string Name { get; } = "Language Manager";

        public Action<string> OnLanguageChanged;

        private string currentLanguageName;
        private string lastLanguageName = "English";

        public ILanguage CurrentLanguage { get; private set; } = new English();
        Dictionary<string, ILanguage> Dic_Language = new Dictionary<string, ILanguage>
    {
        { "简体中文",new Chinese()},
        { "English",new English()},
    };

        void Awake()
        {
            OnLanguageChanged += ChangLanguage;
        }

        void Update()
        {
            currentLanguageName = LocalisationManager.Instance.currLangName;

            if (!lastLanguageName.Equals(currentLanguageName))
            {
                lastLanguageName = currentLanguageName;

                OnLanguageChanged.Invoke(currentLanguageName);
            }
        }

        void ChangLanguage(string value)
        {
            try
            {
                CurrentLanguage = Dic_Language[value];
            }
            catch
            {
                CurrentLanguage = Dic_Language["English"];
            }
        }
    }

    public interface ILanguage
    {
        //Rocket
        string launch { get; }
        string thrustForce { get; }
        string thrustTime { get; }
        string thrustDelay { get; }
        string drag { get; }
        //Pod
        string bulletNumber { get; }
        string rate { get; }
        //MachineGun
      string fire { get; }
        string strength { get; }
        string knockBack { get; }
    }

    public class Chinese : ILanguage
    {
        //Rocket
        public string launch { get; } = "发射";
        public string thrustForce { get; } = "推力";
        public string thrustTime { get; } = "推力时间" + Environment.NewLine + "(10s)";
        public string thrustDelay { get; } = "延迟发射" + Environment.NewLine + "(0.1s)";
        public string drag { get; } = "阻力";
        //Pod
        public string bulletNumber { get; } = "弹药数量";
        public string rate { get; } = "射速";
        //MachineGun
        public string fire { get; } = "开火";
        public string strength { get; } = "炮力";
        public string knockBack { get; } = "后坐力";
    }

    public class English : ILanguage
    {
        //Rocket
        public string launch { get; } = "Launch";
        public string thrustForce { get; } = "Thrust Force";
        public string thrustTime { get; } = "Thrust Time" + Environment.NewLine + "(10s)";
        public string thrustDelay { get; } = "Thrust Delay" + Environment.NewLine + "(0.1s)" ;
        public string drag { get; } = "Drag";
        //Pod
        public string bulletNumber { get; } = "Bullet Number";
        public string rate { get; } = "Rate";
        //MachineGun
        public string fire { get; } = "Fire";
        public string strength { get; } = "Force";
        public string knockBack { get; } = "Knock Back";
    }



}
