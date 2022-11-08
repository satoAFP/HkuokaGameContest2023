using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyFly : BaseStatusClass
{
    [SerializeField, Header("最高速度"), Range(0, 1)]
    private float MaxSpeed;

    [SerializeField, Header("加速減速"), Range(0, 0.1f)]
    private float Deceleration;

    [SerializeField, Header("一直線の時の移動速度"), Range(0, 100)]
    private float MoveSpeed;

    [SerializeField, Header("最高速度"), Range(0, 20)]
    private float LimitSpeed;

    [SerializeField, Header("停止までのフレーム"), Range(0, 500)]
    public int StopCount;



    [SerializeField, Header("主人公感知のレイの速度"), Range(0.01f, 1), Space(50)]
    private float RaySpeed;

    [SerializeField, Header("レイの距離"), Range(1, 10)]
    private float raydistance;


    [SerializeField, Header("主人公に近づいて止まる距離"), Range(0, 5), Space(50)]
    private float StopDistance;


    [SerializeField, Header("主人公と衝突した時のノックバック"), Space(50)]
    private Vector2 KnockbackPow;


    [SerializeField, Header("死んでから消えるまでのフレーム"), Range(0, 100), Space(50)]
    public int DethFrame;

    [SerializeField, Header("アイテムドロップ率"), Range(0, 100)]
    private int dropRate;


    [SerializeField, Header("左右移動"), Space(50)]
    private bool RLMoveFrag;
    [SerializeField, Header("右移動")]
    private bool RMoveFrag;
    [SerializeField, Header("左移動")]
    private bool LMoveFrag;
    [SerializeField, Header("追尾移動")]
    private bool TrackingFrag;



    [SerializeField, Header("待機時の画像"), Space(50)]
    private Sprite StandImage;

    [SerializeField, Header("攻撃時の画像")]
    private Sprite AttackImage;

    [SerializeField, Header("攻撃されたときのエフェクト")]
    private GameObject RecEffct;

    [SerializeField, Header("死ぬときのエフェクト")]
    private GameObject DethEffct;




    private Rigidbody2D rigidbody2d;            //リジットボディ2D取得
    private float movespeed;                    //移動速度計算用
    private int MoveCount = 0;                  //左右移動切り替えのタイミング取得用
    private bool MoveStop = false;              //動きを止めたいとき使用
    private Vector2 RayRotato;                  //レイの回転位置決定変数
    private float rotato = 0;                   //回転量
    private GameObject SearchGameObject;        //レイに触れたオブジェクト
    private bool AttckMode = false;             //主人公を見つけた時の攻撃モード
    private DropItemList dropItemList;          //ドロップアイテム管理用
    private int AttckDirection = 0;             //1:右に攻撃　2:左に攻撃
    private int DethFrameCount = 0;

    private bool first1 = true;



    [System.NonSerialized]
    public bool deth = false;                   //主人公受け渡し用死亡判定
    [System.NonSerialized]
    public int stopCount = 0;                   //移動停止までのカウント



    void Start()
    {
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        dropItemList = GameObject.Find("DropItemList").GetComponent<DropItemList>();
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

        //行動処理>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (!MoveStop)
        {
            //攻撃モードに入っていないときの行動---------------------------------------------------
            if (!AttckMode)
            {
                if (StopCount > stopCount)
                {
                    //右移動
                    if (RMoveFrag)
                    {
                        RightMove();
                    }
                    //左移動
                    if (LMoveFrag)
                    {
                        LeftMove();
                    }
                }
                else
                    rigidbody2d.velocity = Vector3.zero;


                stopCount++;
            }
            //------------------------------------------------------------------


            //攻撃モードの行動--------------------------------------------------
            else
            {
                //右居る時
                if (SearchGameObject.transform.localPosition.x < transform.localPosition.x)
                {
                    if ((SearchGameObject.transform.localPosition.x - transform.localPosition.x) <= -StopDistance)
                    {
                        LeftMove();
                    }
                    //左に攻撃
                    AttckDirection = 2;
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                //左居る時
                if (SearchGameObject.transform.localPosition.x > transform.localPosition.x)
                {
                    if ((SearchGameObject.transform.localPosition.x - transform.localPosition.x) >= StopDistance)
                    {
                        RightMove();
                    }
                    //右に攻撃
                    AttckDirection = 1;
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        //主人公と衝突時のノックバック
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(RecEffct, transform.position, Quaternion.identity);

            HP -= collision.gameObject.GetComponent<Player_Ver2>().ATK;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //主人公の攻撃に当たった時
        if (collision.tag == "PlayerAttack")
        {
            HP -= collision.gameObject.transform.root.gameObject.GetComponent<Player_Ver2>().ATK;


        }

    }



    /// <summary>
    /// 死亡処理
    /// </summary>
    private void Deth()
    {
        if (HP <= 0)
        {
            DethFrameCount++;
            gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;


            //死亡判定受け渡し用
            deth = true;

            if (DethFrame == DethFrameCount)
            {
                //アイテムドロップ処理
                if (dropRate >= Random.Range(0, 100))
                {
                    Instantiate(dropItemList.DropItem[Random.Range(0, dropItemList.DropItem.Length)], transform.localPosition, Quaternion.identity);
                }


                //死亡時のエフェクト
                Instantiate(DethEffct, transform.position, Quaternion.identity);

                //削除
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


    private void RightMove()
    {
        //最高速度になるとそれ以上加速しない
        if (rigidbody2d.velocity.x < LimitSpeed)
        {
            rigidbody2d.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
    }


    private void LeftMove()
    {
        //止まるフレームまで動く
        //最高速度になるとそれ以上加速しない
        if (rigidbody2d.velocity.x > -LimitSpeed)
        {
            rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
        }
        transform.localScale = new Vector3(-1f, 1f, 1f);
    }



    /// <summary>
    /// 移動時の加速減速の折り返し地点計算用
    /// </summary>
    /// <param name="near"></param>
    /// <returns></returns>
    private bool Near(float near)
    {
        if (((MaxSpeed / 2) - Deceleration) < near && ((MaxSpeed / 2) + Deceleration) > near)
            return true;
        else
            return false;
    }
}
