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

        public event Action OnExplode;
        public event Action<Collider[]> OnExploded;
        public event Action OnExplodeFinal;

        #region Network
        /// <summary>position</summary>
        public static MessageType ExplodyMessage = ModNetworking.CreateMessageType(DataType.Vector3);
        #endregion

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
            fireEffect = (GameObject)Instantiate(AssetManager.Instance.Explosion.explosionEffect);
            fireEffect.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.left);
            fireEffect.transform.localScale *= 5f;
            fireEffect.SetActive(false);
        }

        public void Explodey()
        {
            StartCoroutine(Explodey(Position, Power, Radius, ExplosionType));
        }
        public void Explody_Network(Vector3 position)
        {
            fireEffect.transform.FindChild("Debris").localRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up);
            fireEffect.transform.position = position;

            StartCoroutine(ParticleEffectEvent());

            IEnumerator ParticleEffectEvent()
            {
                fireEffect.SetActive(true);
                yield return new WaitForSeconds(3f);
                fireEffect.SetActive(false);
            }        
        }
        public IEnumerator Explodey(Vector3 position, float power,float radius,explosionType explosiontype)
        {
            if (StatMaster.isClient) yield break;

            if (isExplodey)
            {
                yield break;
            }
            else
            {
                fireEffect.transform.FindChild("Debris").localRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up);
                fireEffect.transform.position = position;

                if (!StatMaster.isClient)
                {
                    var message = ExplodyMessage.CreateMessage(position);
                    ModNetworking.SendToAll(message);
                }
            }

            yield return new WaitForFixedUpdate();

            isExplodey = true;
            fireEffect.SetActive(true);
            OnExplode?.Invoke();

            //定义爆炸位置为炸弹位置
            Vector3 explosionPos = position;
            //这个方法用来返回球型半径之内（包括半径）的所有碰撞体collider[]
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

            //遍历返回的碰撞体，如果是刚体，则给刚体添加力
            foreach (Collider hit in colliders)
            {
                if (hit.attachedRigidbody != null)
                {
                    float force = UnityEngine.Random.Range(30000f, 50000f) * power * (Vector3.Distance(hit.transform.position, explosionPos) / (radius + 0.25f));
                    hit.attachedRigidbody.AddExplosionForce(force, explosionPos, radius);
                    hit.attachedRigidbody.AddTorque(force * Vector3.Cross((hit.transform.position - explosionPos), Vector3.up));

                    reduceBlockHealth(hit.attachedRigidbody.gameObject);
                }
            }

            OnExploded?.Invoke(colliders);
            yield return new WaitForSeconds(3f);
            fireEffect.SetActive(false);
            OnExplodeFinal?.Invoke();
            //-------------------------------------------------------------
            void reduceBlockHealth(GameObject gameObject)
            {
                var bhb = gameObject.GetComponent<BlockHealthBar>();
                if (bhb != null)
                {
                    bhb.DamageBlock(1);
                }
            }
            //---------------------------------------------------------------
        }

        public static void Explody_Network(Message message)
        {
            if (StatMaster.isClient)
            {
                var position = (Vector3)message.GetData(0);

               var go = new GameObject("Exploder");
                go.AddComponent<DestroyIfEditMode>();
                go.AddComponent<TimedSelfDestruct>().Begin(30f);
                go.AddComponent<ExplodeScript>().Explody_Network(position);
            }
        }
    }
}
