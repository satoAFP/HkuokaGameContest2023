using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyFly : BaseStatusClass
{
    [SerializeField, Header("ボスのとき")]
    private bool BossMode;

    [SerializeField, Header("敵の攻撃時の飛んでいく方向")]
    private Vector3 MoveDirection;


    [SerializeField, Header("出るエフェクトの数"), Range(0, 10)]
    public int EffectNum;

    [SerializeField, Header("エフェクトが連続で出るときのフレーム"), Range(0, 20)]
    public int EffectInterval;



    [SerializeField, Header("主人公感知のレイの速度"), Range(0.01f, 1)]
    private float RaySpeed;

    [SerializeField, Header("レイの距離"), Range(1, 10)]
    private float raydistance;


    [SerializeField, Header("主人公に近づいて止まる距離"), Range(0, 5), Space(50)]
    private float StopDistance;

    [SerializeField, Header("死んでから消えるまでのフレーム"), Range(0, 100)]
    public int DethFrame;


    [SerializeField, Header("等速移動")]
    private bool TrackingFrag;
    [SerializeField, Header("等速移動")]
    private bool ConstantFrag;


    [SerializeField, Header("攻撃されたときのエフェクト")]
    private GameObject RecEffct;

    [SerializeField, Header("死ぬときのエフェクト")]
    private GameObject DethEffct;

    [SerializeField, Header("死ぬときのアニメーション")]
    private GameObject DethAni;

    [SerializeField, Header("死ぬときのSE")]
    private AudioClip DethSound;

    [SerializeField, Header("ボスの弱点が死ぬときのSE")]
    private AudioClip BossDethSound;



    private Vector2 RayRotato;                  //レイの回転位置決定変数
    private float rotato = 0;                   //回転量
    private GameObject SearchGameObject;        //レイに触れたオブジェクト
    private bool AttckMode = false;             //主人公を見つけた時の攻撃モード
    private int DethFrameCount = 0;             //死ぬまでのカウント
    private bool OnDamageEffect = false;        //ダメージ受けた時のエフェクト
    private int EffectIntervalCount = 0;        //エフェクトのインターバルのカウント
    private int EffectCount = 0;                //エフェクトの回数カウント
    private int NowHP = 0;                      //現在のHP
    private AudioSource audioSource;

    private bool first1 = true;


    [System.NonSerialized]
    public int stopCount = 0;                   //移動停止までのカウント


    private void Start()
    {
        NowHP = HP;
        audioSource = ManagerAccessor.Instance.soundManager.GetComponent<AudioSource>();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        //死亡処理
        Deth();

        //主人公サーチ
        if (TrackingFrag)
        {
            RayPlayerCheck();
        }

        //エフェクト呼び出し
        DamageEffect();

        //行動処理
        if(ConstantFrag)
        {
            transform.position += MoveDirection;
        }

        //HPが減った時エフェクト表示
        if (NowHP > HP)
        {
            OnDamageEffect = true;
            NowHP = HP;


            //SE再生
            if (!BossMode)
                audioSource.PlayOneShot(DethSound);
            else
                audioSource.PlayOneShot(BossDethSound);
        }

    }




    

    /// <summary>
    /// ダメージを受けた時のエフェクト処理
    /// </summary>
    private void DamageEffect()
    {
        //エフェクト再生開始
        if(OnDamageEffect)
        {
            //エフェクトの数
            if (EffectCount < EffectNum) 
            {
                //次エフェクトを出すまでのフレーム
                if (EffectIntervalCount % EffectInterval == 0) 
                {
                    //生成しランダムな方向を向く
                    GameObject clone = Instantiate(RecEffct, transform.position, Quaternion.identity);
                    clone.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 180));
                    EffectCount++;
                }
            }
            else 
            {
                //エフェクト終了
                EffectCount = 0;
                OnDamageEffect = false;
            }
            EffectIntervalCount++;
        }
    }




    /// <summary>
    /// 死亡処理
    /// </summary>
    private void Deth()
    {
        if (HP <= 0)
        {
            //エフェクトを表示して消す
            //OnDamageEffect = true;
            DethFrameCount++;
            Destroy(gameObject.GetComponent<CapsuleCollider2D>());



            if (DethFrame == DethFrameCount)
            {
                //死亡時のエフェクト
                Instantiate(DethEffct, transform.position, Quaternion.identity);

                //死亡時のアニメーション
                Instantiate(DethAni, transform.position, Quaternion.identity);

                //削除
                if (!BossMode)
                    Destroy(transform.parent.gameObject);
                else
                    Destroy(gameObject);
            }
        }
    }


    /// <summary>
    /// レイによるプレイヤー感知処理
    /// </summary>
    private void RayPlayerCheck()
    {
        //オブジェクトから右側にRayを伸ばす
        Ray2D ray = new Ray2D(transform.position, transform.right);
        RaycastHit2D hit;

        //Corgi、Shibaレイヤーとだけ衝突する
        int layerMask = LayerMask.GetMask(new string[] { "Player" });

        //処理の最初に初期化
        if (first1)
        {
            hit = Physics2D.Raycast(ray.origin, new Vector2(0, 0) * raydistance, raydistance, layerMask);
            first1 = false;
        }

        //レイの回転処理
        if (!AttckMode)
        {
            rotato += RaySpeed;
            RayRotato = new Vector2(Mathf.Cos(rotato), Mathf.Sin(rotato));

            //レイを飛ばす
            hit = Physics2D.Raycast(ray.origin + RayRotato, RayRotato * raydistance, raydistance, layerMask);
            Debug.DrawRay(ray.origin + RayRotato, RayRotato * raydistance, Color.green);
        }
        else
        {
            //レイを飛ばす
            hit = Physics2D.Raycast(ray.origin, new Vector2(SearchGameObject.transform.localPosition.x, SearchGameObject.transform.localPosition.y) - ray.origin, raydistance, layerMask);
            Debug.DrawRay(ray.origin, new Vector2(SearchGameObject.transform.localPosition.x, SearchGameObject.transform.localPosition.y) - ray.origin, Color.green);
        }

        //レイで主人公を見つけた時
        if (hit.collider)
        {
            SearchGameObject = hit.collider.gameObject;

            if (SearchGameObject.tag == "Player")
            {
                //攻撃モードに移行
                AttckMode = true;
            }
        }
    }



    //private void RightMove()
    //{
    //    //最高速度になるとそれ以上加速しない
    //    if (rigidbody2d.velocity.x < LimitSpeed)
    //    {
    //        rigidbody2d.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
    //    }
    //    transform.localScale = new Vector3(1f, 1f, 1f);
    //}


    //private void LeftMove()
    //{
    //    //止まるフレームまで動く
    //    //最高速度になるとそれ以上加速しない
    //    if (rigidbody2d.velocity.x > -LimitSpeed)
    //    {
    //        rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
    //    }
    //    transform.localScale = new Vector3(-1f, 1f, 1f);
    //}


}
