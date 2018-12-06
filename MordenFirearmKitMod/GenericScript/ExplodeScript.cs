using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using System.Collections;

namespace ModernFirearmKitMod
{
    public class ExplodeScript :MonoBehaviour
    {

        public bool isExplodey { get; private set; }

        public float Power { get; set; }

        public float Radius { get; set; }

        public Vector3 Position { get; set; }

        public explosionType ExplosionType { get; set; }

        public Rigidbody rigidbody;

        public Action OnExplode;
        public Action OnExploded;

        //爆炸类型
        public enum explosionType
        {
            炸弹 = 0,
            手雷 = 1,
            烟花 = 2
        }

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            OnExplode += () => { };
            OnExploded += () => { };
        }

        public void Explodey()
        {
            StartCoroutine(Explodey(Power,Radius,ExplosionType));
        }

        //爆炸事件
        IEnumerator Explodey(float power,float radius,explosionType explosiontype)
        {

            if (isExplodey)
            {
                yield break;
            }

            yield return new WaitForFixedUpdate();

            isExplodey = true;

            OnExplode();

            //float power = Power;

            ////爆炸范围
            //float radius = Radius;

            //explosionType explosiontype = ExplosionType;

            //爆炸位置
            //Vector3 position_hit = transform.TransformDirection(new Vector3(-1f, rigidbody.centerOfMass.y, rigidbody.centerOfMass.z)) + transform.position;

            //爆炸类型 炸弹
            //if (explosiontype == explosionType.炸弹)
            //{
            //    GameObject explo = (GameObject)Instantiate(PrefabMaster.BlockPrefabs[23].gameObject, Position, transform.rotation);
            //    explo.transform.localScale = Vector3.one * 0.01f;
            //    ExplodeOnCollideBlock ac = explo.GetComponent<ExplodeOnCollideBlock>();
            //    ac.radius = 2 + radius;
            //    ac.power = 3000f * radius;
            //    ac.torquePower = 5000f * radius;
            //    ac.upPower = 0;
            //    ac.Explodey();
            //    explo.AddComponent<TimedSelfDestruct>();
            //}
            //else if (explosiontype == explosionType.手雷)
            //{
            //    GameObject explo = (GameObject)GameObject.Instantiate(PrefabMaster.BlockPrefabs[54].gameObject, Position, transform.rotation);
            //    explo.transform.localScale = Vector3.one * 0.01f;
            //    ControllableBomb ac = explo.GetComponent<ControllableBomb>();
            //    ac.OnExplode(power * 3000f, 0, 0, Position, radius + 2, 0);
            //    explo.AddComponent<TimedSelfDestruct>();
            //}
            //else if (explosiontype == explosionType.烟花)
            //{
            //    GameObject explo = (GameObject)GameObject.Instantiate(PrefabMaster.BlockPrefabs[59].gameObject, Position, transform.rotation);
            //    explo.transform.localScale = Vector3.one * 0.01f;
            //    TimedRocket ac = explo.GetComponent<TimedRocket>();
            //    ac.slipRenderer.material.color = Color.white;
            //    ac.OnExplode(power * 3000f, 0, 0, Position, radius + 2, 0);
            //    explo.AddComponent<TimedSelfDestruct>();
            //}


            OnExploded();

            //foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            //{
            //    if (r.name == "Vis")
            //    {
            //        r.enabled = false;


            //    }
            //}

            //transform.localScale = Vector3.zero;
            //Rigidbody.isKinematic = true;
            //rigidbody.detectCollisions = false;
            //Destroy(gameObject.GetComponentInChildren<FireController>());
            //psp_fire.lifetime = 0;
            //ps_fire.Stop();
            //ps_smoke.Stop();


            //gameObject.AddComponent<TimedSelfDestruct>().lifeTime = psp_smoke.lifetime * 120;

        }
    }
}
