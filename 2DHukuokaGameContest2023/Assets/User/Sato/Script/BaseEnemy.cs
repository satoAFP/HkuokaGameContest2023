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


    private Rigidbody2D rigidbody;              //リジットボディ2D取得
    private int MoveCount = 0;                  //左右移動切り替えのタイミング取得用
    private Vector2 RayRotato;                  //レイの回転位置決定変数
    private float rotato = 0;                   //回転量
    private GameObject SearchGameObject;        //レイに触れたオブジェクト
    private bool AttckMode = false;             //主人公を見つけた時の攻撃モード

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //重力設定-----------------------------------------------------------------------------------------------------------------
        Physics2D.gravity = new Vector3(0, -Gravity, 0);




        //主人公サーチ-------------------------------------------------------------------------------------------------------------
        //オブジェクトから右側にRayを伸ばす
        Ray2D ray = new Ray2D(transform.position, transform.right);
        SearchGameObject = null;
        RaycastHit2D hit;

        //Corgi、Shibaレイヤーとだけ衝突する
        int layerMask = LayerMask.GetMask(new string[] { "Player" });

        //レイの回転の初期化
        rotato += RaySpeed;
        RayRotato = new Vector2(Mathf.Cos(rotato), Mathf.Sin(rotato));
        //レイを飛ばす
        hit = Physics2D.Raycast(ray.origin + RayRotato, RayRotato * raydistance, raydistance, layerMask);
        Debug.DrawRay(ray.origin + RayRotato, RayRotato * raydistance, Color.green);

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
                if (rigidbody.velocity.x > -LimitSpeed)
                {
                    rigidbody.AddForce(transform.right * (-MoveSpeed), ForceMode2D.Force);
                }
            }
            if (MoveCount > (MoveFrame / 2) && MoveCount <= MoveFrame)
            {
                //最高速度になるとそれ以上加速しない
                if (rigidbody.velocity.x < LimitSpeed)
                {
                    rigidbody.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
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
            //ここ書いてね
        }




        
        

        

    }
}
