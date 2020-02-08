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
        public float Power { get; private set; }
        public float Radius { get; private set; }
        public Vector3 Position { get; private set; } = default;
        public explosionType ExplosionType { get;private set; }

        //public Rigidbody rigidbody;

        public event Action OnExplode;
        public event Action<Collider[]> OnExploded;
        public event Action OnExplodeFinal;

        #region Network
        /// <summary>ExplosionType,position</summary>
        public static MessageType ExplodyMessage = ModNetworking.CreateMessageType(DataType.Integer,DataType.Vector3);
        #endregion
        
        public enum explosionType
        {
            Large = 0,
            Small = 1,
            Firework = 2,
            Big=3,
            Smoke=4,
        }

        private GameObject fireEffect;

        //void Awake()
        //{
        //    //rigidbody = GetComponent<Rigidbody>();
        //    fireEffect = (GameObject)Instantiate(AssetManager.Instance.Explosion.bigExplosionEffect);
        //    fireEffect.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.left);
        //    fireEffect.transform.localScale *= 5f;
        //    fireEffect.SetActive(false); 
        //}

        public ExplodeScript Setup(explosionType explosionType,float power,float radius)
        {
            ExplosionType = explosionType;
            Power = power;
            Radius = radius;
            return this;
        }
        private GameObject getExplodeEffectObject(explosionType explosionType)
        {
            GameObject go = null;
            if (explosionType == explosionType.Large)
            {
                go = (GameObject)Instantiate(AssetManager.Instance.Explosion.largeExplosionEffect);
                //go.SetActive(false);
            }
            else if (explosionType == explosionType.Small)
            {
                go = (GameObject)Instantiate(AssetManager.Instance.Explosion.smallExplosionEffect);
                //go.transform.localScale = Vector3.Scale(go.transform.localScale, MordenFirearmKitBlockMod.Configuration.GetValue<Vector3>("SmallExplosion"));
                //go.SetActive(false);
            }
            else if (explosionType == explosionType.Firework)
            {
                go = (GameObject)Instantiate(AssetManager.Instance.Explosion.fireworkeExplosionEffect);
                //go.SetActive(false);
            }
            else if (explosionType == explosionType.Big)
            {
                go = (GameObject)Instantiate(AssetManager.Instance.Explosion.bigExplosionEffect);
                //go.transform.FindChild("Debris").localRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up);
                //go.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.left);
                //go.SetActive(false);
            }
            else if (explosionType == explosionType.Smoke)
            {
                go = (GameObject)Instantiate(AssetManager.Instance.Explosion.smokeExplosionEffect);
                //go.SetActive(false);
            }
            return go;
        }
        public void Explodey()
        {
            StartCoroutine(explodey(ExplosionType, Position, Power, Radius));
        }
        public void Explodey(Vector3 position)
        {
            Position = position;
            Explodey();
        }
        public void Explodey(explosionType explosiontype, Vector3 position, float power, float radius)
        {
            StartCoroutine(explodey(explosiontype, position, power, radius));
        }
        private IEnumerator explodey(explosionType explosiontype,Vector3 position, float power, float radius)
        {
            if (StatMaster.isClient) yield break;

            if (isExplodey)
            {
                yield break;
            }
            else
            {
                fireEffect = getExplodeEffectObject(explosiontype);
                if (ExplosionType == explosionType.Big) { fireEffect.transform.FindChild("Debris").localRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up); }
                fireEffect.transform.position = position;

                if (!StatMaster.isClient)
                {
                    var message = ExplodyMessage.CreateMessage((int)explosiontype,position);
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
        public void Explody_Network(explosionType explosionType, Vector3 position)
        {
            fireEffect = getExplodeEffectObject(explosionType);
            if (ExplosionType == explosionType.Big) { fireEffect.transform.FindChild("Debris").localRotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up); }
            fireEffect.transform.position = position;

            StartCoroutine(ParticleEffectEvent());

            IEnumerator ParticleEffectEvent()
            {
                yield return new WaitForFixedUpdate();
                isExplodey = true;

                fireEffect.SetActive(true);
                fireEffect.AddComponent<TimedSelfDestruct>().Begin(30f);
                yield return new WaitForSeconds(3f);
                fireEffect.SetActive(false);
            }        
        }
        public static void Explody_Network(Message message)
        {
            if (StatMaster.isClient)
            {
                var explosionType = (explosionType)((int)message.GetData(0));
                var position = (Vector3)message.GetData(1);

                var go = new GameObject("Exploder");
                go.AddComponent<ExplodeScript>().Explody_Network(explosionType, position);
                go.AddComponent<TimedSelfDestruct>().Begin(30f);
            }
        }
    }
}
