using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MordenFirearmKitMod
{
    partial class MordenFirearmKitBlockMod
    {
        /// <summary>
        /// 指向模块
        /// </summary>
        public Block direction = new Block()
            .ID((int)BlockList.指向模块)

            .BlockName(BlockList.指向模块.ToString())

            .Obj(new List<Obj>() { new Obj("/MordenFirearmKitMod/Direction.obj", "/MordenFirearmKitMod/Direction.png", new VisualOffset(Vector3.one, Vector3.zero, Vector3.zero)) })

            .IconOffset(new Icon(1.3f, new Vector3(-0.25f, -0.25f, 0f), new Vector3(15f, -15f, 45f)))

            .Components(new Type[] { typeof(DirectionBlockScript) })

            .Properties(new BlockProperties().SearchKeywords(new string[] { "指向", "direction" }).Burnable(0.1f).CanBeDamaged(0.1f))

            .Mass(0.5f)
#if DEBUG
            .ShowCollider(true)
#endif
            .CompoundCollider(new List<ColliderComposite>() { /*ColliderComposite.Capsule(0.25f, 2.65f, Direction.Z, new Vector3(-0.125f, 0f, 0.3f), Vector3.zero)*/
                ColliderComposite.Box(Vector3.one,new Vector3(0,0,0.5f),Vector3.zero) })

            .NeededResources(new List<NeededResource>() { })

            .AddingPoints(new List<AddingPoint>()
            {
                new BasePoint(false,true)//.Motionable(true,true,true)
                ,new AddingPoint(new Vector3(0,0,0.5f),new Vector3(0,0,90),true)
            }
            );

    }

    class DirectionBlockScript : BlockScript
    {

        RaycastHit RHit;

        GameObject TargetObject;

        Vector3 TargetVector;

        Vector3 CrossVector;

        Vector3 LastVector = Vector3.zero;

        Vector3 offset = new Vector3(0,1,0);

        //偏差 间隔
        float Error = 0,Interval = 1,i=0;

        float Hardness = 1;
        //PID
        float P = 1, I = 1, D = 1;

        PID pid = new PID();

        MKey Lock, Release;

        MSlider Pslider, Islider, Dslider;

        //LineRenderer lr1,lr2,lr3;


        class PID
        {
            //比例,积分,微分
            float P,I,D;
            //偏差,上次偏差,上上次偏差
            float Error, LastError,BeforeLastError;
            //时间间隔
            //float T = Time.deltaTime;
            //计算结果
            float Result;

            public PID()
            {
                Result = Error = LastError = BeforeLastError = 0;

                P = I = D = 1;
            }

            public PID(float p,float i,float d)
            {
                Result = Error = LastError = BeforeLastError = 0;

                P = p;
                I = i;
                D = d;

            }

            public float result(float error)
            {
                Error = error;
                Result = P * Error + I * (Error - LastError) + D * (Error - 2 * LastError + BeforeLastError);

                BeforeLastError = LastError;
                LastError = Error;
                
                return Result;
            }

            public float result(float error, float p, float i, float d)
            {
                Error = error;
                P = p;
                I = i;
                D = d;
                Result = P * Error + I * (Error - LastError) + D * (Error - 2 * LastError + BeforeLastError);

                BeforeLastError = LastError;
                LastError = Error;

                return Result;
            }

            public void SetPID(float p, float i, float d)
            {
                P = p;
                I = i;
                D = d;
            }

        }
        

        protected virtual System.Collections.IEnumerator UpdateMapper()
        {
            if (BlockMapper.CurrentInstance == null)
                yield break;
            while (Input.GetMouseButton(0))
                yield return null;
            BlockMapper.CurrentInstance.Copy();
            BlockMapper.CurrentInstance.Paste();
            yield break;
        }

        public override void OnSave(XDataHolder stream)
        {
            base.OnSave(stream);
            SaveMapperValues(stream);
        }

        public override void OnLoad(XDataHolder stream)
        {
            base.OnLoad(stream);
            LoadMapperValues(stream);
        }

        public override void SafeAwake()
        {
            base.SafeAwake();
            Lock = AddKey("锁定", "Lock", KeyCode.T);
            Release = AddKey("释放", "Release", KeyCode.R);

            AddSlider("比例", "p", 1, 0.1f, 2f).ValueChanged += (float value) => { P = value; };
            AddSlider("积分", "i", 1, 0.1f, 3f).ValueChanged += (float value) => { I = value; };
            AddSlider("微分", "d", 1, 0.1f, 3f).ValueChanged += (float value) => { D = value; };
            AddSlider("间隔", "Interval", 1, 1, 10).ValueChanged += (float value) => { Interval = value; };
            AddSlider("硬度", "Hardness", 1, 0, 5).ValueChanged += (float value) => { Hardness = value; };

            BoxCollider BC = GetComponentsInChildren<BoxCollider>().ToList().Find(match => match.name == "Adding Point");
            BC.center = Vector3.zero;
            BC.size = Vector3.one * 1.1f;
        }

        protected override void OnSimulateStart()
        {

            GetComponent<ConfigurableJoint>().projectionMode = JointProjectionMode.PositionAndRotation;
            GetComponent<ConfigurableJoint>().projectionAngle = Mathf.Clamp(Hardness, 0, 5);


        }

        protected override void OnSimulateUpdate()
        {
            //获取目标
            if (Lock.IsDown)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RHit, Camera.main.farClipPlane))
                {
                    if ((RHit.transform.position == transform.position ? false : !RHit.transform.tag.Contains("Cloaked")))
                    {
                        TargetObject = RHit.transform.gameObject;
                    }
                }
            }


        }

        protected override void OnSimulateFixedUpdate()
        {

            if (TargetObject)
            {
                if (Release.IsPressed)
                {
                    TargetObject = null;
                    return;
                }

                while (i++ < Interval) ;
                i = 0;
                TargetVector = TargetObject.transform.position - transform.position;
                CrossVector = Vector3.Cross(TargetVector, transform.forward) + transform.position;
                Error = Vector3.Angle(transform.forward, TargetVector);

                Error = pid.result(Error, P, I, D);

                rigidbody.angularVelocity = rigidbody.transform.InverseTransformVector(Vector3.Scale(rigidbody.angularVelocity, new Vector3(0, 1, 1)));
                rigidbody.MoveRotation(rigidbody.rotation * Quaternion.AngleAxis(-Error * Time.deltaTime * 20f, transform.InverseTransformDirection(CrossVector - transform.position)));
                //rigidbody.AddTorque((CrossVector-transform.position)*50);
            }

        }



    }

}
