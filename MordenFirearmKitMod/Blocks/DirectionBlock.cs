using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MordenFirearmKitMod
{
    //导弹资料
    //http://www.ceeger.com/forum/read.php?tid=3919
    //http://www.cnblogs.com/jqg-aliang/p/4768101.html
    
        /*总结 2018-5-1
         * 
        通过学习资料得知  导弹大致由三部分组成: 目标落点预测计算,导弹飞行轨迹计算,导弹旋转控制计算
    通过计算目标落点然后计算导弹飞行轨迹得出导弹应该旋转多少度以多大的仰角进行射击,控制导弹姿态,然后不断的重复此过程直到导弹与目标落点距离逐渐减小.
        预测精度,控制精度和轨迹精度都是影响导弹命中目标的关键因素,预测精度过低将直接导致导弹拦截失败,所以预测精度是拦截过程中的第一步,然后就是飞行轨
    迹的预测,轨迹的预测结果将影响到导弹姿态控制如果精度过低或预测错误将导致拦截失败.最后是导弹姿态控制部分,通过获取预测数据导弹将控制自身达到相应的旋
    转角和仰角,如果精度过低或控制错误也将导致拦截失败.
        从面向对象的角度来看的话 应该是两个部分:预测计算,控制计算.因为目标预测和轨迹预测两个算法并不是能独立工作的,需要两个同时合作才能得出"目标落点", "相遇时间" 两个核心数据.
    一个预测对象,一个控制对象.预测对象两面又包括两个部分飞行轨迹和炮弹轨迹,两个空间曲线的交点就是目标落点,途中需要的时间就是相遇时间.

    目标落点预测大致原理为 :
        由于两次取值时间很近将目标视为直线匀速运动,通过计算目标速度与时间得到预测坐标(类似于初中打点计时器通过两点距离和两点消耗的时间算出速度然后估算期望时间后的距离)
                            在游戏中先要估算导弹和目标相遇所用的时间,然后直接取 目标速度x相遇时间+目标当前位置=预测位置 (这个是直线预测,后期可以试试引入曲线预测之类的复杂预测).
                            预估相遇时间大致方法为两种:1.炮弹直线运动,目标也是直线运动.这种方法最简单,通过三角函数计算出相遇时间,具体算法在第一篇文章中有详细展示.
                                                       2.炮弹为抛物线,目标为直线运动.通过联立炮弹抛物线和目标直线方程组得出相遇时间.
                            实际情况中不止这两种情况,不过通过上面两种方法足够我们解决大部分问题.其中用第二种方法居多.
                            实际操作种我准备写一个预测脚本,先写一个直线预测的脚本后期再在里面扩展曲线预测的方法,在导弹获取目标后,在目标物体上添加一个预测脚本有一个公共变量"预测落点",在拦截过程中
                            导弹只需要不断去获取目标物体返回的预测落点就行了,在有多预测模式后将添加一个"模式"变量,这样脚本就会以相应预测模式来返回预测落点.
    飞行轨迹原理:
        导弹运动大致分为两种一种就是直线飞行,另一种就是曲线飞行,直线飞行最简单,导弹受到恒定的推力,阻力和重力,导弹质量也不变因为游戏中没有燃料质量消耗这个变量,所以就是简单的匀加速直线运动(F=ma)
        F = 推力-阻力-重力分力. 如果不考虑重力那更加简单.力的方向显而易见因为是直线运动 力的方向与导弹和目标落点连线重合.那么已知合力方向,通过里的分解就能得知推力应该朝哪个方向了然后得出导弹姿态控制
        数据交给姿态控制脚本来控制即可.算出导弹所受的力后导弹质量为已知就能算出导弹加速度,在实际开发中我准备定义一个目标加速度来简化预测过程的难度,轨迹以目标加速度来进行预测,通过控制脚本来让导弹达到
        目标加速度,把变加速运动简化成匀加速运动,计算难度和需要考虑的因素也将减小,提高预测精度,但是这样做的后果就是对控制脚本提出了更高的要求.
    控制原理:
        控制主要分为两大类,一个是利用变换直接转动导弹这种简单粗暴效果好更精确,但是这种转动不真实容易把导弹上的其他零件直接甩飞.二一个就是在刚体上添加力进行控制,这种控制方式比较复杂,需要考虑转动惯量等
    诸多因素的影响,但是这种控制更真实,虽然也有现成代码但是效果也不是很好,可虑加入PID控制也许会好一些不过对技术要求和计算量带来了增加.
        
        技术设想:
    在目标点和发射点之间建立曲线,曲线起点的切线方向就是导弹初始仰角(在炮塔模式下极为重要)

                
        
    
    */

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

            .Components(new Type[] { typeof(DirectionBlockScript2) })

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
                //rigidbody.AddTorque(-(CrossVector-transform.position)*500);
            }

        }



    }


    //直接旋转
    class DirectionBlockScript1 : BlockScript
    {

        RaycastHit RHit;

        Transform TargetObject;

        float rotationSpeed = 600;
        float moveSpeed = 50;



        MKey Lock, Release;

        public override void SafeAwake()
        {
            base.SafeAwake();
            Lock = AddKey("锁定", "Lock", KeyCode.T);
            Release = AddKey("释放", "Release", KeyCode.R);


            BoxCollider BC = GetComponentsInChildren<BoxCollider>().ToList().Find(match => match.name == "Adding Point");
            BC.center = Vector3.zero;
            BC.size = Vector3.one * 1.1f;
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
                        TargetObject = RHit.transform;
                    }
                }
            }



          

        }

        protected override void OnSimulateFixedUpdate()
        {
            if (TargetObject)
            {
                // 开始追踪敌人  
                Vector3 target = (TargetObject.position - transform.position).normalized;
                float a = Vector3.Angle(transform.forward, target) / rotationSpeed;
                if (a > 0.1f || a < -0.1f)
                {
                    transform.forward = Vector3.Slerp(transform.forward, target, Time.deltaTime / a).normalized;
                    moveSpeed = Mathf.MoveTowards(moveSpeed, 50f, 2 * Time.deltaTime);
                    rotationSpeed = Mathf.MoveTowardsAngle(rotationSpeed, 1200f, 10 * Time.deltaTime);
                }
                   
                else
                {
                    moveSpeed = Mathf.MoveTowards(moveSpeed, 200f, 2 * Time.deltaTime);
                    rotationSpeed = Mathf.MoveTowardsAngle(rotationSpeed, 600f, 10 * Time.deltaTime);
                    transform.forward = Vector3.Slerp(transform.forward, target, 1).normalized;
                }

                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }


    //直接旋转2
    class DirectionBlockScript2 : BlockScript
    {

        RaycastHit RHit;

        Transform TargetObject;

        MKey Lock, Release;

        RadarOfRocket radar;

        public override void SafeAwake()
        {

            Lock = AddKey("锁定", "Lock", KeyCode.T);
            Release = AddKey("释放", "Release", KeyCode.R);


            BoxCollider BC = GetComponentsInChildren<BoxCollider>().ToList().Find(match => match.name == "Adding Point");
            BC.center = Vector3.zero;
            BC.size = Vector3.one * 1.1f;

            
        }

        protected override void OnSimulateStart()
        {
            
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
                        TargetObject = RHit.transform;
                        if (!TargetObject.GetComponent<SpeedTest>())
                        {
                            TargetObject.gameObject.AddComponent<SpeedTest>();
                            gameObject.AddComponent<SpeedTest>();
                            radar = gameObject.AddComponent<RadarOfRocket>();
                            radar.target = TargetObject;
                            gameObject.AddComponent<Missile>();
                        }
                    }
                }
            }

        }

        protected override void OnSimulateFixedUpdate()
        {

            if (!TargetObject)
            {
                return;
            }
            
           
        }
    }
    public class PhycisMath
    {
        public static float GetSpeed(Vector3 lastPos, Vector3 newPs, float time)
        {
            if (time == 0) return 0;
            return Vector3.Distance(lastPos, newPs) / time;
        }
        public static Vector3 GetDir(Vector3 lastPos, Vector3 newPs)
        {
            return (newPs - lastPos).normalized;
        }
        public static float GetDelta(float a, float b, float c)
        {
            return b * b - 4 * a * c;
        }
        public static float GetRad(float dis, float angle)
        {
            return -(2 * dis * Mathf.Cos(angle * Mathf.Deg2Rad));
        }
        public static float GetPom(float a, float b)
        {
            return 1 - Mathf.Pow(a, b);
        }
        public static float GetSqrtOfMath(float a, float b, float d)
        {
            float a1 = (-b + Mathf.Sqrt(d)) / (2 * a);
            float a2 = (-b - Mathf.Sqrt(d)) / (2 * a);

            return a1 > a2 ? a1 : a2;
        }
        public Vector3 GetHitPoint()
        {
            return Vector3.zero;
        }


        public static int iterativeCount = 0;

        //抛物线方程
        //XY落点,GV 重力加速度 炮弹速度
        //返回XY发射角 飞行时间
        public Vector2 formulaProjectile(float X, float Y, float V, float G)
        {
            if (G == 0)
            {
                float THETA = Mathf.Atan(Y / X);
                float T = (Y / Mathf.Sin(THETA)) / V;
                return (new Vector2(THETA, T));
            }
            else
            {
                float DELTA = Mathf.Pow(V, 4) - G * (G * X * X - 2 * Y * V * V);
                if (DELTA < 0)
                {
                    return Vector2.zero;
                }
                float THETA1 = Mathf.Atan((-(V * V) + Mathf.Sqrt(DELTA)) / (G * X));
                float THETA2 = Mathf.Atan((-(V * V) - Mathf.Sqrt(DELTA)) / (G * X));
                if (THETA1 > THETA2)
                    THETA1 = THETA2;
                float T = X / (V * Mathf.Cos(THETA1));
                return new Vector2(THETA1, T);
            }
        }
        //目标运动直线
        public Vector3 formulaTarget(float VT, Vector3 PT, Vector3 DT, float TT)
        {
            Vector3 newPosition = PT + DT * (VT * TT);
            return newPosition;
        }


        public Vector3 calculateNoneLinearTrajectory(float gunVelocity, float AirDrag, Vector3 gunPosition, float TargetVelocity, Vector3 TargetPosition, Vector3 TargetDirection, Vector3 hitPoint, float G, float accuracy, float diff)
        {
            iterativeCount++;
            if (iterativeCount > 512) { iterativeCount = 0; return hitPoint; }
            if (hitPoint == Vector3.zero || gunVelocity < 1)
            {
                return currentTarget.transform.position;
            }
            Vector3 gunDirection = new Vector3(hitPoint.x, gunPosition.y, hitPoint.z) - gunPosition;
            Quaternion gunRotation = Quaternion.FromToRotation(gunDirection, Vector3.forward);
            Vector3 localHitPoint = gunRotation * (hitPoint - gunPosition);
            float currentCalculatedDistance = (hitPoint - gunPosition).magnitude;

            float b2M4ac = gunVelocity * gunVelocity - 4 * AirDrag * currentCalculatedDistance;
            if (b2M4ac < 0) { /*Debug.Log("Nan!!!" + (gunVelocity * gunVelocity - 2 * AirDrag * currentCalculatedDistance));*/ return currentTarget.transform.position; }
            float V = (float)Math.Sqrt(b2M4ac);
            float X = localHitPoint.z;//z为前方
            float Y = localHitPoint.y;
            Vector2 TT = formulaProjectile(X, Y, V, G);
            if (TT == Vector2.zero)
            {
                iterativeCount = 0;
                return currentTarget.transform.position;
            }
            float VT = TargetVelocity;
            Vector3 PT = TargetPosition;
            Vector3 DT = TargetDirection;
            float T = TT.y;
            Vector3 newHitPoint = formulaTarget(VT, PT, DT, T);
            float diff1 = (newHitPoint - hitPoint).magnitude;
            if (diff1 > diff)
            {
                iterativeCount = 0;
                return currentTarget.transform.position;
            }
            if (diff1 < accuracy)
            {
                gunRotation = Quaternion.Inverse(gunRotation);
                Y = Mathf.Tan(TT.x) * X;
                newHitPoint = gunRotation * new Vector3(0, Y, X) + gunPosition;
                iterativeCount = 0;
                return newHitPoint;
            }
            return calculateNoneLinearTrajectory(gunVelocity, AirDrag, gunPosition, TargetVelocity, TargetPosition, TargetDirection, newHitPoint, G, accuracy, diff1);
        }
        public Vector3 calculateNoneLinearTrajectoryWithAccelerationPrediction(float gunVelocity, float AirDrag, Vector3 gunPosition, float TargetVelocity, float targetAcceleration, Vector3 TargetPosition, Vector3 TargetDirection, Vector3 hitPoint, float G, float accuracy, float diff)
        {
            iterativeCount++;
            if (iterativeCount > 512) { iterativeCount = 0; return TargetPosition; }
            if (hitPoint == Vector3.zero || gunVelocity < 1)
            {
                return currentTarget.transform.position;
            }
            Vector3 gunDirection = new Vector3(hitPoint.x, gunPosition.y, hitPoint.z) - gunPosition;
            Quaternion gunRotation = Quaternion.FromToRotation(gunDirection, Vector3.forward);
            Vector3 localHitPoint = gunRotation * (hitPoint - gunPosition);
            float currentCalculatedDistance = (hitPoint - gunPosition).magnitude;

            float b2M4ac = gunVelocity * gunVelocity - 4 * AirDrag * currentCalculatedDistance;
            if (b2M4ac < 0) { /*Debug.Log("Nan!!!" + (gunVelocity * gunVelocity - 2 * AirDrag * currentCalculatedDistance));*/ return currentTarget.transform.position; }
            float V = (float)Math.Sqrt(b2M4ac);
            float X = localHitPoint.z;//z为前方
            float Y = localHitPoint.y;
            Vector2 TT = formulaProjectile(X, Y, V, G);
            if (TT == Vector2.zero)
            {
                iterativeCount = 0;
                return currentTarget.transform.position;
            }
            float VT = TargetVelocity + targetAcceleration * currentCalculatedDistance;
            Vector3 PT = TargetPosition;
            Vector3 DT = TargetDirection;
            float T = TT.y;
            Vector3 newHitPoint = formulaTarget(VT, PT, DT, T);
            float diff1 = (newHitPoint - hitPoint).magnitude;
            if (diff1 > diff)
            {
                iterativeCount = 0;
                return currentTarget.transform.position;
            }
            if (diff1 < accuracy)
            {
                gunRotation = Quaternion.Inverse(gunRotation);
                Y = Mathf.Tan(TT.x) * X;
                newHitPoint = gunRotation * new Vector3(0, Y, X) + gunPosition;
                iterativeCount = 0;
                return newHitPoint;
            }
            return calculateNoneLinearTrajectory(gunVelocity, AirDrag, gunPosition, TargetVelocity, TargetPosition, TargetDirection, newHitPoint, G, accuracy, diff1);
        }
        public Vector3 calculateLinearTrajectory(float gunVelocity, Vector3 gunPosition, float TargetVelocity, Vector3 TargetPosition, Vector3 TargetDirection)
        {

            Vector3 hitPoint = Vector3.zero;

            if (TargetVelocity != 0)
            {
                Vector3 D = gunPosition - TargetPosition;
                float THETA = Vector3.Angle(D, TargetDirection) * Mathf.Deg2Rad;
                float DD = D.magnitude;

                float A = 1 - Mathf.Pow((gunVelocity / TargetVelocity), 2);
                float B = -(2 * DD * Mathf.Cos(THETA));
                float C = DD * DD;
                float DELTA = B * B - 4 * A * C;

                if (DELTA < 0)
                {
                    return Vector3.zero;
                }

                float F1 = (-B + Mathf.Sqrt(B * B - 4 * A * C)) / (2 * A);
                float F2 = (-B - Mathf.Sqrt(B * B - 4 * A * C)) / (2 * A);

                if (F1 < F2)
                    F1 = F2;
                hitPoint = TargetPosition + TargetDirection * F1;
            }
            else
            {
                hitPoint = TargetPosition;
            }
            return hitPoint;
        }
        public Vector3 calculateLinearTrajectoryWithAccelerationPrediction(float gunVelocity, Vector3 gunPosition, float TargetVelocity, float TargetAcceleration, Vector3 TargetPosition, Vector3 TargetDirection, Vector3 PredictedPoint, float Precision)
        {

            Vector3 hitPoint = Vector3.zero;

            iterativeCount++;
            if (iterativeCount > 512) { iterativeCount = 0; return calculateLinearTrajectory(gunVelocity, gunPosition, TargetVelocity, targetPoint, TargetDirection); }

            if (TargetVelocity != 0)
            {
                Vector3 D = gunPosition - TargetPosition;
                float THETA = Vector3.Angle(D, TargetDirection) * Mathf.Deg2Rad;
                float DD = D.magnitude;

                float A = 1 - Mathf.Pow((gunVelocity / TargetVelocity + (TargetAcceleration * (PredictedPoint.magnitude / gunVelocity))), 2);
                float B = -(2 * DD * Mathf.Cos(THETA));
                float C = DD * DD;
                float DELTA = B * B - 4 * A * C;

                if (DELTA < 0)
                {
                    return Vector3.zero;
                }

                float F1 = (-B + Mathf.Sqrt(B * B - 4 * A * C)) / (2 * A);
                float F2 = (-B - Mathf.Sqrt(B * B - 4 * A * C)) / (2 * A);

                if (F1 < F2 && F1 >= 0)
                    F1 = F2;
                hitPoint = TargetPosition + TargetDirection * F1;
            }
            else
            {
                hitPoint = TargetPosition;
            }
            if ((hitPoint - PredictedPoint).sqrMagnitude < Precision * Precision)
            {
                return hitPoint;
            }
            else
            {
                return calculateLinearTrajectoryWithAccelerationPrediction(gunVelocity, gunPosition, TargetVelocity, TargetAcceleration, TargetPosition, TargetDirection, hitPoint, Precision);
            }
        }

        public Vector3 getCorrTorque(Vector3 from, Vector3 to, Rigidbody rb, float SpeedPerSecond)
        {
            try
            {
                Vector3 x = Vector3.Cross(from.normalized, to.normalized);                // axis of rotation
                float theta = Mathf.Asin(x.magnitude);                                    // angle between from & to
                Vector3 w = x.normalized * theta / SpeedPerSecond;                        // scaled angular acceleration
                Vector3 w2 = w - rb.angularVelocity;                                      // need to slow down at a point
                Quaternion q = rb.rotation * rb.inertiaTensorRotation;                    // transform inertia tensor
                return q * Vector3.Scale(rb.inertiaTensor, (Quaternion.Inverse(q) * w2)); // calculate final torque
            }
            catch { return Vector3.zero; }
        }
    }
    public class SpeedTest : MonoBehaviour
    {
        private Vector3 lastPos;
        private float lastTime;  
        private float dtime;

        public Vector3 CurrentVector;
        public float Speed;

        // Update is called once per frame
        void OnEnable()
        {
            lastTime = Time.time;
            lastPos = transform.position;
        }
        void Update()
        {
            dtime = Time.time - lastTime;
            if (dtime > 0)
            {
                lastTime = Time.time;

                Speed = PhycisMath.GetSpeed(lastPos, transform.position, dtime);
                CurrentVector = PhycisMath.GetDir(lastPos, transform.position);
                if (Mathf.Abs(Speed) < 0.001f)
                {
                    CurrentVector = transform.TransformDirection(Vector3.forward);
                }
                lastPos = transform.position;
            }
        }
    }
    public class RadarOfRocket : MonoBehaviour
    {

        public Transform target;//目标
        private SpeedTest rocketSpeed;//
        private SpeedTest targetSpeed;


        private Vector3 targetDir;
        private float angle;
        private float distence;

        private bool isAim = false;
        public bool IsAim { get { return isAim; } set { isAim = value; } }

        private Vector3 aimPos;
        public Vector3 AimPos { get { return aimPos; } set { aimPos = value; } }



        private void Start()
        {
            checkTarget();
        }

        void checkTarget()
        {
            if (!(rocketSpeed = GetComponent<SpeedTest>()))
            {
                gameObject.AddComponent<SpeedTest>();
                rocketSpeed = GetComponent<SpeedTest>();
            }
            if (target && !(targetSpeed = target.GetComponent<SpeedTest>()))
            {
                target.gameObject.AddComponent<SpeedTest>();
                targetSpeed = target.GetComponent<SpeedTest>();
            }
        }

        void Update()
        {
            if (target) TestAim();
        }
        public void TestAim()
        {

            if (Mathf.Abs(targetSpeed.Speed) < 0.01f)
            { //物体的速度过小，则默认物体是静止的。

                isAim = true;
                aimPos = target.position;
            }
            else
            {
                targetDir = transform.position - target.position;
                angle = Vector3.Angle(targetDir, targetSpeed.CurrentVector);

                distence = targetDir.magnitude;

                float a = PhycisMath.GetPom((rocketSpeed.Speed / targetSpeed.Speed), 2);
                float b = PhycisMath.GetRad(distence, angle);
                float c = distence * distence;
                float d = PhycisMath.GetDelta(a, b, c);
                isAim = d >= 0 && !float.IsNaN(d) && !float.IsInfinity(d);

                if (isAim)
                {
                    float r = PhycisMath.GetSqrtOfMath(a, b, d);
                    if (r < 0) isAim = false;//如果得出的是负值，则代表交点有误
                    aimPos = target.transform.position + targetSpeed.CurrentVector * r*0.975f;
                }

            }




        }
    }
    public class Missile : MonoBehaviour
    {
        private RadarOfRocket radar;
        public float Speed = 50;
        public float RoteSpeed = 50;
        public float Noise = 0;
        void OnEnable()
        {
            radar = GetComponent<RadarOfRocket>();
        }
        //void Update()
        //{
        //    Fly();
        //    if (radar.IsAim)
        //    {
        //        FlyToTarget(radar.AimPos - transform.position);
        //    }
        //}

        private void FixedUpdate()
        {
            if (radar.IsAim)
            {
                FlyToTarget(radar.AimPos - transform.position);
            }
        }
        private void FlyToTarget(Vector3 point)
        {
            if (point != Vector3.zero)
            {
                Quaternion missileRotation = Quaternion.LookRotation(point, Vector3.up);

                transform.rotation = Quaternion.Slerp(transform.rotation, missileRotation, Time.deltaTime * RoteSpeed);
            }

        }
        private void Fly()
        {
            Speed = Mathf.Max( Mathf.MoveTowards(Speed, radar.target.GetComponent<SpeedTest>().Speed * 1.1f, 10 * Time.deltaTime),50f);
            Move(transform.forward.normalized + transform.right * Mathf.PingPong(Time.time, 0.5f) * Noise, Speed * Time.deltaTime);
        }
        public void Move(Vector3 dir, float speed)
        {
            transform.Translate(dir * speed, Space.World);
        }
        //void  (Collider other)
        //{
        //    print("hit");
        //}

        void OnCollisionEnter(Collision collision)
        {
            print("hit");
        }



    }

}
