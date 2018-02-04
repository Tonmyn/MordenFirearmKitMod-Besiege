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

            .Obj(new List<Obj>() { new Obj("/MordenFirearmKitMod/Direction.obj", "/MordenFirearmKitMod/Direction.png", new VisualOffset(new Vector3(1f, 1f, 1f), new Vector3(1.25f, 0f, 0.3f), new Vector3(45f, 0f, 180f))) })

            .IconOffset(new Icon(1.3f, new Vector3(-0.25f, -0.25f, 0f), new Vector3(15f, -15f, 45f)))

            .Components(new Type[] { typeof(DirectionBlockScript) })

            .Properties(new BlockProperties().SearchKeywords(new string[] { "指向", "direction" }).Burnable(0.1f).CanBeDamaged(0.1f))

            .Mass(0.5f)
#if DEBUG
            .ShowCollider(true)
#endif
            .CompoundCollider(new List<ColliderComposite>() { ColliderComposite.Capsule(0.25f, 2.65f, Direction.X, new Vector3(-0.125f, 0f, 0.3f), Vector3.zero) })

            .NeededResources(new List<NeededResource>() { })

            .AddingPoints(new List<AddingPoint>()
            {
                new BasePoint(false,true)//.Motionable(true,true,true)
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

        float force = 0.1f,i = 0;

        Vector3 offset = new Vector3(0,1,0);

        LineRenderer lr1,lr2,lr3;

        

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


        protected override void OnSimulateStart()
        {
            lr1 = gameObject.AddComponent<LineRenderer>();
            lr1.enabled = false;
            lr1.useWorldSpace = true;
            lr1.SetVertexCount(2);
            lr1.material = new Material(Shader.Find("Particles/Additive"));
            lr1.SetColors(Color.red, Color.yellow);
            lr1.SetWidth(0.5f, 0.5f);

            //lr2 = gameObject.AddComponent<LineRenderer>();
            //lr2.useWorldSpace = true;
            //lr2.SetVertexCount(2);
            //lr2.material = new Material(Shader.Find("Particles/Additive"));
            //lr2.SetColors(Color.blue, Color.magenta);
            //lr2.SetWidth(2f, 2f);

            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //go.transform.parent = transform;
            //go.transform.position = -transform.right + transform.position;
            //Destroy(go.GetComponent<Rigidbody>());
            //lr2 = go.AddComponent<LineRenderer>();
            lr2 = new GameObject().AddComponent<LineRenderer>();
            lr2.enabled = false;
            lr2.useWorldSpace = true;
            lr2.SetVertexCount(2);
            lr2.material = new Material(Shader.Find("Particles/Additive"));
            lr2.SetColors(Color.blue, Color.magenta);
            lr2.SetWidth(2f, 2f);
            lr2.gameObject.AddComponent<DestroyIfEditMode>();

            lr3 = new GameObject().AddComponent<LineRenderer>();
            lr3.enabled = false;
            lr3.useWorldSpace = true;
            lr3.SetVertexCount(2);
            lr3.material = new Material(Shader.Find("Particles/Additive"));
            lr3.SetColors(Color.grey, Color.green);
            lr3.SetWidth(2f, 2f);
            lr3.gameObject.AddComponent<DestroyIfEditMode>();
        }


        protected override void OnSimulateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RHit, Camera.main.farClipPlane))
                {
                    if ((RHit.transform.position == transform.position ? false : !RHit.transform.tag.Contains("Cloaked")))
                    {
                        //this.currentTarget = this.hitt.transform.gameObject;
                        TargetObject = RHit.transform.gameObject;
                        //TargetPoint = RHit.point;
                        //Debug.Log(RHit.transform.gameObject.name);
                    }
                }
            }


            if (TargetObject)
            {
                //Debug.Log(TargetObject.transform.position);
                //transform.rotation = Quaternion.LookRotation(Target.transform.position );
                //Rigidbody.MoveRotation(Quaternion.LookRotation(Target.transform.position));

                lr1.enabled = true;
                lr1.SetPosition(0, transform.position);
                lr1.SetPosition(1, TargetObject.transform.position);

                lr2.enabled = true;
                lr2.SetPosition(0, transform.position);
                lr2.SetPosition(1, -transform.right*5 + transform.position);

                lr3.enabled = true;
                lr3.SetPosition(0, transform.position);
                lr3.SetPosition(1, CrossVector);
            }
            else
            {
                //transform.rotation = Camera.main.transform.rotation;
            }

            //lr2.SetPosition(0, transform.position + offset);
            //lr2.SetPosition(1, transform.right + offset);

        }

        protected override void OnSimulateFixedUpdate()
        {

            if (TargetObject)
            {

                TargetVector = TargetObject.transform.position - transform.position;
                //TargetVector = transform.InverseTransformVector(TargetVector);
                CrossVector = Vector3.Cross(TargetVector, -transform.right) + transform.position;

                Debug.Log(Vector3.Angle(-transform.right, TargetVector) + " " +TargetVector);
                //Rigidbody.MoveRotation(Quaternion.LookRotation(Target.transform.right * Time.deltaTime * force));
                //Debug.Log(Vector2.Angle(new Vector2(TargetVector.x, TargetVector.z), -new Vector2(transform.right.x, transform.right.z))+" - "+ Vector2.Angle(new Vector2(TargetVector.y, TargetVector.x), new Vector2(transform.right.y, transform.right.x)));
                //rigidbody.AddTorque ( Vector3.Scale(TargetVector+transform.right, new Vector3(100, 100, 100) * Time.deltaTime));
                //Debug.Log(Vector2.Angle(new Vector2(TargetVector.x,TargetVector.y),Vector2.right)+"|"+ Vector2.Angle(new Vector2(TargetVector.x, TargetVector.z), Vector2.right));
                //Debug.Log(TargetVector);
                //transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(TargetObject.transform.position,transform.position),Time.deltaTime);
                //transform.rotation = Quaternion.AngleAxis(1 * Time.deltaTime, CrossVector);
                //Rigidbody.AddTorque(-CrossVector * 1000f);
                //Rigidbody.AddRelativeTorque(Vector3.Scale(transform.InverseTransformVector(-CrossVector), new Vector3(1, 1, 0))*1000f);
                //Rigidbody.AddTorque(Vector3.Scale( TargetVector,new Vector3(1,1,0)) * 1000f);
                //transform.RotateAround(transform.position + offset, TargetVector, Vector3.Angle(-transform.right, TargetVector) * Time.deltaTime * 50);

                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-transform.right, -CrossVector), i=i+0.01f);
                //Quaternion .t
            }


            if (Input.GetKeyDown(KeyCode.X))
            {
                //transform.rotation = Quaternion.AngleAxis((i++) * Time.deltaTime, Vector3.right);
                //transform.RotateAround( transform.position,transform.right, 20 * Time.deltaTime);
                //Rigidbody.AddTorque(Vector3.right * 100f);

                //transform.rotation = Quaternion.LookRotation(-transform.right, -CrossVector);
            }

        }

    }

}
