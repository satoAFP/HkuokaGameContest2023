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


    private Rigidbody2D rigidbody2d;              //リジットボディ2D取得
    private int MoveCount = 0;                  //左右移動切り替えのタイミング取得用
    private Vector2 RayRotato;                  //レイの回転位置決定変数
    private float rotato = 0;                   //回転量
    private GameObject SearchGameObject;        //レイに触れたオブジェクト
    private bool AttckMode = false;             //主人公を見つけた時の攻撃モード



    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //重力設定-----------------------------------------------------------------------------------------------------------------
        Physics2D.gravity = new Vector3(0, -Gravity, 0);




        //主人公サーチ-------------------------------------------------------------------------------------------------------------
        //オブジェクトから右側にRayを伸ばす
        Ray2D ray = new Ray2D(transform.position, transform.right);
        //SearchGameObject = null;
        RaycastHit2D hit;

        //Corgi、Shibaレイヤーとだけ衝突する
        int layerMask = LayerMask.GetMask(new string[] { "Player" });

        if (first)
        {
            hit = Physics2D.Raycast(ray.origin, new Vector2(0, 0) * raydistance, raydistance, layerMask);
            first = false;
        }

        //レイの回転の初期化
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
            var angles = new Vector3(GetAim(SearchGameObject.transform.position, transform.position), 0, 0);
            var direction = Quaternion.Euler(angles) * Vector3.forward;



            RayRotato = new Vector2(direction.x, direction.y);
            //レイを飛ばす
            hit = Physics2D.Raycast(ray.origin, new Vector2(SearchGameObject.transform.localPosition.x, SearchGameObject.transform.localPosition.y)- ray.origin, raydistance, layerMask);
            Debug.DrawRay(ray.origin , new Vector2(SearchGameObject.transform.localPosition.x, SearchGameObject.transform.localPosition.y) - ray.origin, Color.green);
        }

        if (hit.collider)
        {
            SearchGameObject = hit.collider.gameObject;

            if (SearchGameObject.tag == "Player")
            {
                AttckMode = true;
            }
        }



        //移動処理----------------------------------------------------------------------------------------------------------------
        //攻撃モードに入っていないときの行動
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
            }
            if (MoveCount > (MoveFrame / 2) && MoveCount <= MoveFrame)
            {
                //最高速度になるとそれ以上加速しない
                if (rigidbody2d.velocity.x < LimitSpeed)
                {
                    rigidbody2d.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
                }
            }
            if (MoveCount > MoveFrame)
            {
                //カウントリセット
                MoveCount = 0;
            }
        }
        //攻撃モードの行動
        else
        {
            //右居る時
            if (SearchGameObject.transform.localPosition.x < transform.localPosition.x)
            {
                if ((SearchGameObject.transform.localPosition.x - transform.localPosition.x) <= -StopDistance)
                {
                    //最高速度になるとそれ以上加速しない
                    if (rigidbody2d.velocity.x > -LimitSpeed)
                    {
                        rigidbody2d.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
                    }
                }
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
            }
        }




        
        

        

    }


    /// <summary>
    /// 二点間の角度を求める関数
    /// </summary>
    /// <param name="p1">原点となるオブジェクト座標</param>
    /// <param name="p2">角度を求めたいオブジェクト座標</param>
    /// <returns>二点間の角度</returns>
    public float GetAim(Vector3 p1, Vector3 p2)
    {
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        float rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }
}
