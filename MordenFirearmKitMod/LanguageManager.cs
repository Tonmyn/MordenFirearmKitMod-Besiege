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

        void Update()
        {
            currentLanguageName = LocalisationManager.Instance.currLangName;

            if (!lastLanguageName.Equals(currentLanguageName))
            {
                lastLanguageName = currentLanguageName;
                ChangLanguage(currentLanguageName);
                OnLanguageChanged?.Invoke(currentLanguageName);
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

        string bulletPower { get; }
        string bulletMass { get; }
        string bulletDrag { get; }
        string bulletTrailLength { get; }
        string bulletTrailColor { get; }
        string distance { get; }
        string damper { get; }
        string hold { get; }
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

        public string bulletPower { get; } = "子弹能量";
        public string bulletMass { get; } = "子弹质量";
        public string bulletDrag { get; } = "子弹阻力";
        public string bulletTrailLength { get; } = "子弹拖尾长度";
        public string bulletTrailColor { get; } = "子弹尾迹颜色";
        public string distance { get; } = "枪口距离";
        public string damper { get; } = "阻尼";
        public string hold { get; } = "按住发射";
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

        public string bulletPower { get; } = "Bullet Power";
        public string bulletMass { get; } = "Bullet Mass";
        public string bulletDrag { get; } = "Bullet Drag";
        public string bulletTrailLength { get; } = "Bullet TrailLength";
        public string bulletTrailColor { get; } = "Bullet TrailColor";
        public string distance { get; } = "Spawn Distance";
        public string damper { get; } = "Damper";
        public string hold { get; } = "Hold Fire";
    }

}
