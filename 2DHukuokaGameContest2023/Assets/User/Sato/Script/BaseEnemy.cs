using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseStatusClass
{
    [SerializeField, Header("移動速度"), Range(0, 100), Space(50)]
    private float MoveSpeed;

    [SerializeField, Header("最高速度"), Range(0, 20)]
    private float LimitSpeed;

    [SerializeField, Header("移動時間"), Range(0, 1000), Space(50)]
    private float MoveFrame;


    [SerializeField, Header("ジャンプ力"), Range(0, 50)]
    private float JumpPower;

    [SerializeField, Header("ジャンプする間隔"), Range(0, 1000), Space(50)]
    private int JumpInterval;


    [SerializeField, Header("重力"), Range(0, 100), Space(50)]
    private float Gravity;


    [SerializeField, Header("主人公感知のレイの速度"), Range(0.01f, 1)]
    private float RaySpeed;

    [SerializeField, Header("レイの距離"), Range(1, 10)]
    private float raydistance;


    [SerializeField, Header("主人公に近づいて止まる距離"), Range(0, 5), Space(50)]
    private float StopDistance;

    [SerializeField, Header("自身からどの位置にパンチ判定を設定するか")]
    private Vector3 AttackPunchPos;

    [SerializeField, Header("自身からどの位置にキック判定を設定するか")]
    private Vector3 AttackKickPos;

    [SerializeField, Header("攻撃頻度"), Range(0, 500)]
    private int AttackFrequency;

    [SerializeField, Header("攻撃モーションのフレーム"), Range(0, 50), Space(50)]
    private int AttackMotionFrame;

    
    [SerializeField, Header("主人公と衝突した時のノックバック")]
    private Vector2 KnockbackPow;

    [SerializeField, Header("けり上げ時のノックバック"), Space(50)]
    private Vector2 KnockbackKickPow;

    
    [SerializeField, Header("アイテムドロップ率"), Range(0, 100), Space(50)]
    private int dropRate;

    
    [SerializeField, Header("左右移動"), Space(50)]
    private bool RLMoveFrag;
    [SerializeField, Header("右移動")]
    private bool RMoveFrag;
    [SerializeField, Header("左移動")]
    private bool LMoveFrag;
    [SerializeField, Header("追尾移動")]
    private bool TrackingFrag;
    [SerializeField, Header("ジャンプ移動")]
    private bool JumpFrag;
    [SerializeField, Header("パンチ")]
    private bool PunchFrag;
    [SerializeField, Header("キック")]
    private bool KickFrag;


    
    [SerializeField, Header("パンチ判定オブジェクト"), Space(50)]
    private GameObject Attack1Collision;

    [SerializeField, Header("キック判定オブジェクト")]
    private GameObject Attack2Collision;

    [SerializeField, Header("待機時の画像")]
    private Sprite StandImage;

    [SerializeField, Header("攻撃時の画像")]
    private Sprite AttackImage;




    private Rigidbody2D rigidbody2d;            //リジットボディ2D取得
    private int MoveCount = 0;                  //左右移動切り替えのタイミング取得用
    private int JumpIntCount = 0;               //ジャンプする間隔設定
    private bool MoveStop = false;              //動きを止めたいとき使用
    private Vector2 RayRotato;                  //レイの回転位置決定変数
    private float rotato = 0;                   //回転量
    private GameObject SearchGameObject;        //レイに触れたオブジェクト
    private bool AttckMode = false;             //主人公を見つけた時の攻撃モード
    private DropItemList dropItemList;          //ドロップアイテム管理用
    private int AttckDirection = 0;             //1:右に攻撃　2:左に攻撃
    private int AttackFreCount = 0;             //攻撃頻度計算時フレームカウント用
    private int AttackMotCount = 0;             //攻撃モーション計算時フレームカウント用
    private bool AttackMotCheck = false;        //攻撃モーション中trueになる
    private bool Attck1 = false;                //攻撃1
    private bool Attck2 = false;                //攻撃2

    private bool first1 = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        dropItemList = GameObject.Find("DropItemList").GetComponent<DropItemList>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //重力設定
        Physics2D.gravity = new Vector3(0, -Gravity, 0);

        //死亡処理
        Deth();

        //主人公サーチ
        if (TrackingFrag)
        {
            RayPlayerCheck();
        }

        //ジャンプ処理
        if (JumpFrag)
        {
            Jump();
        }


        //行動処理>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (!MoveStop)
        {
            //攻撃モードに入っていないときの行動---------------------------------------------------
            if (!AttckMode)
            {
                //左右移動
                if (RLMoveFrag)
                {
                    MoveCount++;
                    if (MoveCount <= (MoveFrame / 2))
                    {
                        LeftMove(); Debug.Log("ccc");
                    }
                    if (MoveCount > (MoveFrame / 2) && MoveCount <= MoveFrame)
                    {
                        RightMove();
                        Debug.Log("aaa");
                    }
                    if (MoveCount > MoveFrame)
                    {
                        //カウントリセット
                        MoveCount = 0; Debug.Log("bbb");
                    }
                }

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

                    //攻撃モードでない時は攻撃までのフレームカウントをリセット
                    AttackFreCount = 0;
            }
            //------------------------------------------------------------------


            //攻撃モードの行動--------------------------------------------------
            else
            {
                //攻撃までのフレームカウント
                AttackFreCount++;

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

                
                if (PunchFrag || KickFrag)
                {
                    //攻撃処理
                    Vector3 pos = transform.position;//攻撃位置の座標更新用
                                                     //攻撃までタイミング
                    if (AttackFrequency == AttackFreCount)
                    {
                        if (PunchFrag)
                            Attck1 = true;
                        if (KickFrag)
                            Attck2 = true;
                        AttackMotCheck = true;
                        AttackFreCount = 0;
                        gameObject.GetComponent<SpriteRenderer>().sprite = AttackImage;
                    }

                    //攻撃するときの向き
                    if (AttckDirection == 1)
                    {
                        if (PunchFrag)
                            pos += AttackPunchPos;//右
                        if (KickFrag)
                        {
                            pos.x += AttackKickPos.x;
                            pos.y += AttackKickPos.y;
                        }
                    }
                    if (AttckDirection == 2)
                    {
                        if (PunchFrag)
                            pos -= AttackPunchPos;//左
                        if (KickFrag)
                        {
                            pos.x -= AttackKickPos.x;
                            pos.y += AttackKickPos.y;
                        }
                    }

                    //攻撃モーション
                    if (AttackMotCheck)
                    {
                        AttackMotCount++;
                        //モーション終了条件
                        if (AttackMotionFrame == AttackMotCount)
                        {
                            AttackMotCount = 0;
                            AttackMotCheck = false;
                            gameObject.GetComponent<SpriteRenderer>().sprite = StandImage;
                        }
                    }

                    //パンチ攻撃
                    if (Attck1)
                    {
                        GameObject obj = Instantiate(Attack1Collision, pos, Quaternion.identity);
                        obj.transform.parent = gameObject.transform;
                        obj.GetComponent<AttckCollision>().Damage = ATK;
                        Attck1 = false;
                    }
                    //キック攻撃
                    if (Attck2)
                    {
                        GameObject obj = Instantiate(Attack2Collision, pos, Quaternion.identity);
                        obj.transform.parent = gameObject.transform;
                        obj.GetComponent<AttckCollision>().Damage = ATK;
                        Attck2 = false;
                    }
                }
                //-------------------------------------------------------------------------
            }
        }
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        //地面に着地で移動停止終了
        if (collision.gameObject.tag == "Ground")
        {
            MoveStop = false;
        }


        //主人公と衝突時のノックバック
        if (collision.gameObject.tag=="Player")
        {
            if (AttckDirection == 1)
            {
                rigidbody2d.AddForce(new Vector2(-KnockbackPow.x, KnockbackPow.y), ForceMode2D.Force);
            }
            if (AttckDirection == 2)
            {
                rigidbody2d.AddForce(KnockbackPow, ForceMode2D.Force);
            }
            MoveStop = true;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //主人公の攻撃に当たった時
        if (collision.tag == "PlayerAttack") 
        {
            if(PunchFrag)
            {
                //プレイヤーの攻撃力とパンチ判定受け取る
            }
            if(KickFrag)
            {
                //プレイヤーの攻撃力とキック判定受け取る

                //蹴り上げられた時の処理
                if (AttckDirection == 1)
                {
                    rigidbody2d.AddForce(new Vector2(-KnockbackKickPow.x, KnockbackKickPow.y), ForceMode2D.Force);
                }
                if (AttckDirection == 2)
                {
                    rigidbody2d.AddForce(KnockbackKickPow, ForceMode2D.Force);
                }
                MoveStop = true;
            }

        }

    }



    /// <summary>
    /// 死亡処理
    /// </summary>
    private void Deth()
    {
        if (HP <= 0)
        {
            //アイテムドロップ処理
            if (dropRate >= Random.Range(0, 100))
            {
                Instantiate(dropItemList.DropItem[Random.Range(0, dropItemList.DropItem.Length)], transform.localPosition, Quaternion.identity);
            }

            //削除
            Destroy(gameObject);
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


    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump()
    {
        //ジャンプまでのフレームカウント
        JumpIntCount++;
        //ジャンプ実行
        if (JumpIntCount == JumpInterval)
        {
            rigidbody2d.AddForce(transform.up * JumpPower);
            rigidbody2d.velocity = transform.up * JumpPower;
            JumpIntCount = 0;
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
        //最高速度になるとそれ以上加速しない
        if (rigidbody2d.velocity.x > -LimitSpeed)
        {
            rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
        }
        transform.localScale = new Vector3(-1f, 1f, 1f);
    }
}
