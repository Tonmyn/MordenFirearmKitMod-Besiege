using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{


    //贴图数据
    public Texture gunFlash;

    public Mesh gunMeshCone;

    public Mesh gunMeshCube;

    public Texture gunFlame;

    public Texture gunSmoke;

    public Texture gunHeat;

    public Vector3 GunPoint = new Vector3(0, 0, 3.5f);

    public float Size = 1;

    public float Time = 1;

    ParticleSystem[] gunParticles = new ParticleSystem[5];




    private void Awake()
    {
        gunParticles[0] = new GameObject().AddComponent<ParticleSystem>();
        gunParticles[1] = new GameObject().AddComponent<ParticleSystem>();
        gunParticles[2] = new GameObject().AddComponent<ParticleSystem>();
        gunParticles[3] = new GameObject().AddComponent<ParticleSystem>();
        gunParticles[4] = new GameObject().AddComponent<ParticleSystem>();
        gunParticles[0].gameObject.transform.SetParent(gameObject.transform);
        gunParticles[1].gameObject.transform.SetParent(gameObject.transform);
        gunParticles[2].gameObject.transform.SetParent(gameObject.transform);
        gunParticles[3].gameObject.transform.SetParent(gameObject.transform);
        gunParticles[4].gameObject.transform.SetParent(gameObject.transform);

        gunParticles[2].gameObject.AddComponent<AnimationUV>();
        gunParticles[3].gameObject.AddComponent<AnimationUV>().SetCut(8, 8);
        gunParticles[4].gameObject.AddComponent<AnimationUV>().SetPropertise(8, 8, 40, 0.5f);
    }


    // Use this for initialization
    void Start()
    {

        gunParticles[0].gameObject.transform.position = gameObject.transform.position;
        gunParticles[1].gameObject.transform.position = gameObject.transform.position;
        gunParticles[2].gameObject.transform.position = gameObject.transform.position;
        gunParticles[3].gameObject.transform.position = gameObject.transform.position;//gameObject.transform.TransformPoint(gameObject.transform.localPosition - GunPoint);
        gunParticles[4].gameObject.transform.position = gameObject.transform.position;

        #region 粒子[0]为枪口光效
        //gunParticles[0].playOnAwake = false;
        gunParticles[0].Stop();
        gunParticles[0].loop = false;
        gunParticles[0].startSize = 0.5f * Size;
        gunParticles[0].startSpeed = 0;
        gunParticles[0].maxParticles = 100;
        gunParticles[0].startLifetime = 0.01f * Time;
        gunParticles[0].startColor = new Color32(250, 100, 0, 255);
        gunParticles[0].scalingMode = ParticleSystemScalingMode.Shape;


        ParticleSystem.EmissionModule em = gunParticles[0].emission;
        em.rate = new ParticleSystem.MinMaxCurve (0);
        em.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 15, 15) });
        em.enabled = true;

        ParticleSystem.ShapeModule sm = gunParticles[0].shape;
        sm.meshShapeType = ParticleSystemMeshShapeType.Edge;
        sm.shapeType = ParticleSystemShapeType.Mesh;
        sm.mesh = gunMeshCone;
        sm.enabled = true;

        ParticleSystem.ColorOverLifetimeModule colm = gunParticles[0].colorOverLifetime;
        colm.color = new Gradient()
        {
            alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0, 0), new GradientAlphaKey(255, 0.2f), new GradientAlphaKey(255, 0.8f), new GradientAlphaKey(0, 1) },

            //colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white,0),new GradientColorKey(Color.white,1) }
        };
        colm.enabled = false;

        ParticleSystem.SizeBySpeedModule sbsm = gunParticles[0].sizeBySpeed;
        sbsm.range = new Vector2(0, 1);
        sbsm.size = new ParticleSystem.MinMaxCurve(1, 0);
        sbsm.enabled = false;

        ParticleSystemRenderer psr = gunParticles[0].GetComponent<ParticleSystemRenderer>();
        psr.renderMode = ParticleSystemRenderMode.Billboard;
        psr.normalDirection = 1;
        psr.material = new Material(Shader.Find("Particles/Additive"));
        psr.material.mainTexture = gunFlash;
        //psr.material.color = new Color(70, 35, 20, 255);        
        psr.sortMode = ParticleSystemSortMode.Distance;
        psr.sortingFudge = 0;
        psr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        psr.receiveShadows = true;
        psr.minParticleSize = 0;
        psr.maxParticleSize = 0.5f;

        #endregion

        #region 粒子[1]为枪口光效2
        gunParticles[1].Stop();
        gunParticles[1].loop = false;
        gunParticles[1].startSize = 0.35f * Size;
        gunParticles[1].startSpeed = 0;
        gunParticles[1].maxParticles = 100;
        gunParticles[1].startLifetime = 0.1f * Time;
        gunParticles[1].startColor = new Color32(250, 100, 0, 255);
        gunParticles[1].scalingMode = ParticleSystemScalingMode.Shape;


        em = gunParticles[1].emission;
        em.rate = new ParticleSystem.MinMaxCurve(0);
        em.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 15, 15) });
        em.enabled = true;

        sm = gunParticles[1].shape;
        sm.meshShapeType = ParticleSystemMeshShapeType.Vertex;
        sm.shapeType = ParticleSystemShapeType.Mesh;
        sm.mesh = gunMeshCube;
        sm.randomDirection = false;
        sm.enabled = true;

        ParticleSystem.SizeOverLifetimeModule solm = gunParticles[1].sizeOverLifetime;
        sbsm.range = new Vector2(0, 1);
        sbsm.size = new ParticleSystem.MinMaxCurve(1, new AnimationCurve()
        {
            keys = new Keyframe[]
                {
                    new Keyframe (0,0.5f)
                    ,new Keyframe(0.3f,1)
                    ,new Keyframe(1,0.75f)
                }
        });
        sbsm.enabled = false;

        psr = gunParticles[1].GetComponent<ParticleSystemRenderer>();
        psr.renderMode = ParticleSystemRenderMode.Stretch;
        psr.lengthScale = 1.6f;
        psr.normalDirection = 1;
        psr.material = new Material(Shader.Find("Particles/Additive"));
        psr.material.mainTexture = gunFlash;
        //psr.material.color = new Color(70, 35, 20, 255);        
        psr.sortMode = ParticleSystemSortMode.Distance;
        psr.sortingFudge = 0;
        psr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        psr.receiveShadows = true;
        psr.minParticleSize = 0;
        psr.maxParticleSize = 0.5f;

        #endregion

        #region 粒子[2]为枪口火焰

        gunParticles[2].Stop();
        gunParticles[2].loop = false;
        gunParticles[2].startSize = 0.5f * Size;
        gunParticles[2].startSpeed = -0.1f;
        gunParticles[2].maxParticles = 100;
        gunParticles[2].startLifetime = 0.05f * Time;
        gunParticles[2].startColor = new Color32(250, 100, 0, 255);
        gunParticles[2].scalingMode = ParticleSystemScalingMode.Shape;

        em = gunParticles[2].emission;
        em.rate = new ParticleSystem.MinMaxCurve (0);
        em.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 1) });
        em.enabled = true;

        sm = gunParticles[2].shape;
        sm.shapeType = ParticleSystemShapeType.Cone;
        sm.radius = 0.01f;
        sm.angle = 5f;
        sm.length = 5;
        //sm.randomDirection = false;
        sm.enabled = true;

        psr = gunParticles[2].GetComponent<ParticleSystemRenderer>();
        psr.renderMode = ParticleSystemRenderMode.Stretch;
        psr.lengthScale = 2.5f;
        psr.normalDirection = 1;
        psr.material = new Material(Shader.Find("Particles/Additive"));
        psr.material.mainTexture = gunFlame;

        #endregion



        #region 粒子[3]为枪口热气
        gunParticles[3].Stop();
        gunParticles[3].loop = false;
        gunParticles[3].startSize = 0.5f * Size;
        gunParticles[3].startSpeed = 0.1f;
        gunParticles[3].maxParticles = 100;
        gunParticles[3].startLifetime = 1f * Time;
        gunParticles[3].startColor = new Color32(250, 100, 0, 255);
        gunParticles[3].scalingMode = ParticleSystemScalingMode.Shape;

        em = gunParticles[3].emission;
        em.rate = new ParticleSystem.MinMaxCurve { constant = 0, mode = ParticleSystemCurveMode.Constant };
        em.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 3) });
        em.enabled = true;

        sm = gunParticles[3].shape;
        sm.shapeType = ParticleSystemShapeType.Sphere;
        sm.radius = 0.43f;
        sm.randomDirection = false;
        sm.enabled = true;

        colm = gunParticles[3].colorOverLifetime;
        colm.color = new ParticleSystem.MinMaxGradient()
        {
            gradient = new Gradient()
            {
                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(0, 0), new GradientAlphaKey(255, 0.1f), new GradientAlphaKey(50, 0.35f), new GradientAlphaKey(0, 1) },
                //colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, 0), new GradientColorKey(Color.white, 1) }
            }
        };
        colm.enabled = false;

        psr = gunParticles[3].GetComponent<ParticleSystemRenderer>();
        psr.renderMode = ParticleSystemRenderMode.Stretch;
        psr.lengthScale = 2.5f;
        psr.normalDirection = 1;
        //psr.material = new VacuumBlock().particle[0].GetComponent<ParticleSystemRenderer>().material.shader = new Shader();
        //psr.material.shader = new Shader();
        psr.material.mainTexture = gunHeat;
        #endregion

        #region 粒子[4]为枪口烟雾
        gunParticles[4].Stop();
        gunParticles[4].loop = false;
        gunParticles[4].startSize = 2f * Size;
        gunParticles[4].startSpeed = 0.1f;
        gunParticles[4].maxParticles = 100;
        gunParticles[4].startLifetime = 1f + gunParticles[4].GetComponent<AnimationUV>().DelayTime;
        gunParticles[4].startColor = new Color(0.7f,0.7f,0.7f,0.8f);
        gunParticles[4].scalingMode = ParticleSystemScalingMode.Shape;

        em = gunParticles[4].emission;
        em.rate = new ParticleSystem.MinMaxCurve (0);
        em.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0, 1) });
        em.enabled = true;

        sm = gunParticles[4].shape;
        sm.shapeType = ParticleSystemShapeType.Sphere;
        sm.radius = 0.5f;
        //sm.randomDirection = false;
        sm.enabled = true;

        solm = gunParticles[4].sizeOverLifetime;
        solm.size = new ParticleSystem.MinMaxCurve(1, new AnimationCurve()
        {
            keys = new Keyframe[]
                {
                    new Keyframe (0,0.5f)
                    ,new Keyframe(0.3f,1)
                    ,new Keyframe(1,2)
                }

        }
        );
        solm.enabled = true;

        psr = gunParticles[4].GetComponent<ParticleSystemRenderer>();
        psr.renderMode = ParticleSystemRenderMode.Billboard;
        psr.lengthScale = 2.5f;
        psr.normalDirection = 1;
        psr.material = new Material(Shader.Find("Particles/Alpha Blended"));
        psr.material.mainTexture = gunSmoke;


        #endregion

    }


    // Update is called once per frame
    void Update()
    {

        //if (gunParticles[1].isPlaying)
        //{

        //    deltX += SpeedX * Time.deltaTime * Direction;
        //    deltY += SpeedY * Time.deltaTime * Direction;
        //    gunParticles[1].GetComponent<ParticleSystemRenderer>().material.mainTextureOffset = new Vector2(deltX, deltY);
        //}
    }

    public void Play()
    {
        gunParticles[0].Play();
        //gunParticles[1].Play();
        gunParticles[2].Play();
        //gunParticles[3].Play();
        gunParticles[4].Play();
    }
}

public class AnimationUV : MonoBehaviour
{
    public ParticleSystem ps;

    public int Xcut = 4;
    public int Ycut = 4;
    public float DelayTime = 0f;
    public int AniSpeed = 1;
    public bool Loop = false;
    public bool SuiJi = false;

    public string TexName = "_MainTex";

    protected Material m_Mat;
    protected float m_DangQianTime;
    private float TikingX = 0f;
    private float TikingY = 0f;
    private int numberCut;
    private int dangQianCut = 0;
    private float speed;
    private Vector2[] offsets;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        m_Mat = GetComponent<ParticleSystemRenderer>().material;
        if (AniSpeed <= 0)
        {
            AniSpeed = 1;
        }
        speed = AniSpeed / 60f;

        numberCut = Xcut * Ycut;

        MainTexTiLing();

        offsets = new Vector2[numberCut];
        for (int i = 0; i < offsets.Length; i++)
        {
            float offsetX = (i) % (float)Xcut / Xcut;
            int oy = ((i) % numberCut) / Xcut;
            float offsetY = (1.0f - TikingY) - oy * TikingY;
            Vector2 offset = new Vector2(offsetX, offsetY);
            offsets[i] = offset;
        }
    }
    void Update()
    {
        if (ps.isPlaying)
        {
            MainTexOffset();
        }
        else
        {
            dangQianCut = 0;
        }

    }

    void MainTexTiLing()
    {
        if (Xcut <= 0)
        {
            Xcut = 1;
        }
        if (Ycut <= 0)
        {
            Ycut = 1;
        }
        TikingX = 1f / Xcut;
        TikingY = 1f / Ycut;
        Vector2 TiKing = new Vector2(TikingX, TikingY);
        m_Mat.SetTextureScale(TexName, TiKing);
        Vector2 Offset = new Vector2(0f, 1f - TikingY);
        m_Mat.SetTextureOffset(TexName, Offset);
    }

    void MainTexOffset()
    {
        speed -= Time.deltaTime * Time.timeScale;

        m_DangQianTime += Time.deltaTime * Time.timeScale;

        if (m_DangQianTime >= DelayTime)
        {
            if (speed <= 0 && !SuiJi)
            {
                //if (Loop)
                //{
                //    dangQianCut += 1;
                //}
                //else
                //{
                //    dangQianCut += 1;
                //    if (dangQianCut > numberCut)
                //    {
                //        //dangQianCut = numberCut;
                //        dangQianCut = 0;
                //    }
                //}
                dangQianCut += 1;
                if (!Loop)
                {
                    if (dangQianCut > numberCut)
                    {
                        dangQianCut = 0;
                        m_DangQianTime = 0;
                        return;
                    }
                }

                float offsetX = (dangQianCut - 1) % (float)Xcut / Xcut;
                int oy = ((dangQianCut - 1) % numberCut) / Xcut;
                float offsetY = (1.0f - TikingY) - oy * TikingY;
                Vector2 offset = new Vector2(offsetX, offsetY);
                m_Mat.SetTextureOffset(TexName, offset);
                //speed = AniSpeed / 60;
                speed = AniSpeed / (60);
                
            }
            if (speed <= 0 && SuiJi)
            {
                var i = Random.Range(0, numberCut);
                m_Mat.SetTextureOffset(TexName, offsets[i]);
                speed = AniSpeed / 60f;
            }
        }

    }

    public void SetCut(int x,int y)
    {
        Xcut = x;
        Ycut = y;
    }

    public void SetSpeed(int speed)
    {
        AniSpeed = speed;
    }

    public void SetDelay(float time)
    {
        DelayTime = time;
    }

    public void SetPropertise(int x,int y,int speed,float time)
    {
        Xcut = x;
        Ycut = y;
        AniSpeed = speed;
        DelayTime = time;
    }
}



