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

        public bool isExplodey { get; set; } = false;

        public float Power { get; set; }

        public float Radius { get; set; }

        public Vector3 Position { get; set; }

        public explosionType ExplosionType { get; set; }

        public Rigidbody rigidbody;

        public Action OnExplode;
        public Action OnExploded;
        public Action OnExplodeFinal;

        //爆炸类型
        public enum explosionType
        {
            炸弹 = 0,
            手雷 = 1,
            烟花 = 2
        }

        private GameObject fireEffect;

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();          
        }

        public void Explodey()
        {
            StartCoroutine(Explodey(Position, Power,Radius,ExplosionType));
        }

        //爆炸事件
        public IEnumerator Explodey(Vector3 position, float power,float radius,explosionType explosiontype)
        {

            if (isExplodey)
            {
                yield break;
            }
            else
            {
                fireEffect = (GameObject)Instantiate(AssetManager.Instance.Explosion.explosionEffect, position, Quaternion.identity);
                fireEffect.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.left);
                fireEffect.AddComponent<TimedSelfDestruct>().lifeTime = 30f;
                fireEffect.transform.localScale *= 5f;
                fireEffect.GetComponent<TimedSelfDestruct>().OnDestruct += OnExplodeFinal;
            }

            yield return new WaitForFixedUpdate();

            isExplodey = true;

            OnExplode?.Invoke();

            //定义爆炸位置为炸弹位置
            Vector3 explosionPos = position;
            //这个方法用来反回球型半径之内（包括半径）的所有碰撞体collider[]
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

            //遍历返回的碰撞体，如果是刚体，则给刚体添加力
            foreach (Collider hit in colliders)
            {
                if (hit.attachedRigidbody != null)
                {
                    float force = UnityEngine.Random.Range(30000f, 50000f) * power * (Vector3.Distance(hit.transform.position, explosionPos) / (radius + 0.25f));
                    hit.attachedRigidbody.AddExplosionForce(force, explosionPos, radius);
                    hit.attachedRigidbody.AddTorque(force * Vector3.Cross((hit.transform.position - explosionPos), Vector3.up));
                }
            }

            OnExploded?.Invoke();
        }
    }
}
