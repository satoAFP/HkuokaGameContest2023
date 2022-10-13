using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseStatusClass
{
    [SerializeField, Header("移動速度"), Range(0, 100)]
    private float MoveSpeed;

    [SerializeField, Header("最高速度"), Range(0, 20)]
    private float LimitSpeed;

    [SerializeField, Header("移動時間"), Range(0, 1000)]
    private float MoveFrame;

    [SerializeField, Header("重力"), Range(0, 100)]
    private float Gravity;

    [SerializeField, Header("主人公感知のレイの速度"), Range(0.01f, 1)]
    private float RaySpeed;

    [SerializeField, Header("レイの距離"), Range(1, 10)]
    private float raydistance;

    [SerializeField, Header("主人公に近づいて止まる距離"), Range(0, 5)]
    private float StopDistance;

    [SerializeField, Header("自身からどの位置に攻撃判定を設定するか")]
    private Vector3 AttackPos;

    [SerializeField, Header("攻撃頻度"), Range(0, 500)]
    private int AttackFrequency;

    [SerializeField, Header("攻撃モーションのフレーム"), Range(0, 50)]
    private int AttackMotionFrame;

    [SerializeField, Header("主人公と衝突した時のノックバック")]
    private Vector2 KnockbackPow;

    [SerializeField, Header("けり上げ時のノックバック")]
    private Vector2 KnockbackKickPow;


    [SerializeField, Header("当たり判定オブジェクト")]
    private GameObject AttackCollision;

    [SerializeField, Header("待機時の画像")]
    private Sprite StandImage;

    [SerializeField, Header("攻撃時の画像")]
    private Sprite AttackImage;




    private Rigidbody2D rigidbody2d;            //リジットボディ2D取得
    private int MoveCount = 0;                  //左右移動切り替えのタイミング取得用
    private bool MoveStop = false;              //動きを止めたいとき使用
    private Vector2 RayRotato;                  //レイの回転位置決定変数
    private float rotato = 0;                   //回転量
    private GameObject SearchGameObject;        //レイに触れたオブジェクト
    private bool AttckMode = false;             //主人公を見つけた時の攻撃モード
    private int AttckDirection = 0;             //1:右に攻撃　2:左に攻撃
    private int AttackFreCount = 0;             //攻撃頻度計算時フレームカウント用
    private int AttackMotCount = 0;             //攻撃モーション計算時フレームカウント用
    private bool AttackMotCheck = false;        //攻撃モーション中trueになる
    private bool Attck1 = false;                //攻撃1

    private bool first1 = true;
    private bool first2 = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //重力設定>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        Physics2D.gravity = new Vector3(0, -Gravity, 0);

        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


        //死亡処理>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (HP <= 0) 
        {
            //アイテムドロップ処理

            //削除
            Destroy(gameObject);
        }

        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


        //主人公サーチ>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
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
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<




        //行動処理>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (!MoveStop)
        {
            //攻撃モードに入っていないときの行動---------------------------------------------------
            if (!AttckMode)
            {
                MoveCount++;
                if (MoveCount <= (MoveFrame / 2))
                {
                    //最高速度になるとそれ以上加速しない
                    if (rigidbody2d.velocity.x > -LimitSpeed)
                    {
                        rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
                    }
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                if (MoveCount > (MoveFrame / 2) && MoveCount <= MoveFrame)
                {
                    //最高速度になるとそれ以上加速しない
                    if (rigidbody2d.velocity.x < LimitSpeed)
                    {
                        rigidbody2d.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
                    }
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                if (MoveCount > MoveFrame)
                {
                    //カウントリセット
                    MoveCount = 0;
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
                        //最高速度になるとそれ以上加速しない
                        if (rigidbody2d.velocity.x > -LimitSpeed)
                        {
                            //移動処理
                            rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
                        }
                    }
                    //左に攻撃
                    AttckDirection = 2;
                    //左向く
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                //左居る時
                if (SearchGameObject.transform.localPosition.x > transform.localPosition.x)
                {
                    if ((SearchGameObject.transform.localPosition.x - transform.localPosition.x) >= StopDistance)
                    {
                        //最高速度になるとそれ以上加速しない
                        if (rigidbody2d.velocity.x < LimitSpeed)
                        {
                            rigidbody2d.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
                        }
                    }
                    //右に攻撃
                    AttckDirection = 1;
                    //右向く
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }


                //攻撃処理
                Vector3 pos = transform.position;//攻撃位置の座標更新用
                                                 //攻撃までタイミング
                if (AttackFrequency == AttackFreCount)
                {
                    Attck1 = true;
                    AttackMotCheck = true;
                    AttackFreCount = 0;
                    gameObject.GetComponent<SpriteRenderer>().sprite = AttackImage;
                }

                //攻撃するときの向き
                if (AttckDirection == 1)
                {
                    pos += AttackPos;//右
                }
                if (AttckDirection == 2)
                {
                    pos -= AttackPos;//左
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

                //攻撃
                if (Attck1)
                {
                    GameObject obj = Instantiate(AttackCollision, pos, Quaternion.identity);
                    obj.transform.parent = gameObject.transform;
                    obj.GetComponent<AttckCollision>().Damage = ATK;
                    Attck1 = false;
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
            //HP減らす処理

        }

        //蹴り上げられた時の処理
        if (collision.tag == "Finish") 
        {
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
