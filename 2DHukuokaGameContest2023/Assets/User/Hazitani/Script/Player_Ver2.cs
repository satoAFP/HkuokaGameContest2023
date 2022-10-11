using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ver2 : MonoBehaviour
{
	[SerializeField, Header("ジャンプ力"), Range(0, 100)]
	private float JumpPower;

	[SerializeField, Header("移動速度"), Range(0, 200)]
	private float MoveSpeed;

	[SerializeField, Header("最高速度"), Range(0, 20)]
	private float LimitSpeed;

	[SerializeField, Header("重力"), Range(0, 100)]
	private float Gravity;

	public enum Direction
	{
		LEFT = -1,
		STOP,
		RIGHT,
	}

	private Rigidbody2D rb2D;
	private int jump_count = 0;
	private bool ground_hit = false;
	private int now_move = 0;//左:-1・停止:0・右:1

	private Ray2D ray_left, ray_right;			//飛ばすレイ
	private float distance = 2.0f;				//レイを飛ばす距離
	private RaycastHit2D hit_left,hit_right;	//レイが何かに当たった時の情報
	private Vector3 rayPosition1, rayPosition2;	//レイを発射する位置
	private GameObject SearchGameObject = null; //レイに触れたオブジェクト取得用
	
	//レイの接地判定
	private void RayGround(Ray2D ray, RaycastHit2D hit, Vector3 vec)
    {
		//レイを下に飛ばす
		ray = new Ray2D(vec, -transform.up);

		//Groundとだけ衝突する
		int layerMask = LayerMask.GetMask(new string[] { "Ground" });
		hit = Physics2D.Raycast(ray.origin, ray.direction, distance, layerMask);

		//レイを赤色で表示させる
		Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);

		if (hit.collider)
		{
			SearchGameObject = hit.collider.gameObject;

			if (SearchGameObject.tag == "Ground" && ground_hit == true && jump_count > 0)
			{
				Debug.Log("着地してる！");
				jump_count = 0;
			}
		}
	}

	void Start()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	void Update()
    {
		//レイを発射する位置の調整
		rayPosition1 = this.transform.position + new Vector3(-0.5f, -0.5f, 0.0f);
		rayPosition2 = this.transform.position + new Vector3(0.5f, -0.5f, 0.0f);

		//レイの接地判定
		RayGround(ray_left, hit_left, rayPosition1);
		RayGround(ray_right, hit_right, rayPosition2);

        //ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space) && jump_count < 1)
		{
			Debug.Log("ジャンプ入力された");
			rb2D.velocity = new Vector2(rb2D.velocity.x, JumpPower);

			//カウント増加
			jump_count++;
		}

		//弱攻撃
		if (Input.GetMouseButtonDown(0))
		{
			switch(now_move)
            {
				case (int)Direction.LEFT:
					Debug.Log("左弱キック！");
					break;
				case (int)Direction.STOP:
					Debug.Log("弱パンチ！");
					break;
				case (int)Direction.RIGHT:
					Debug.Log("右弱キック！");
					break;
			}
		}

		//強攻撃
		if (Input.GetMouseButtonDown(1))
		{
			switch (now_move)
			{
				case (int)Direction.LEFT:
					Debug.Log("左強キック！");
					break;
				case (int)Direction.STOP:
					Debug.Log("強パンチ！");
					break;
				case (int)Direction.RIGHT:
					Debug.Log("右強キック！");
					break;
			}
		}
	}

    void FixedUpdate()
	{
		//重力設定
		Physics2D.gravity = new Vector3(0, -Gravity, 0);

		//移動処理
		if (Input.GetKey(KeyCode.A))
		{
			now_move = (int)Direction.LEFT;
			//最高速度になるとそれ以上加速しない
			if (rb2D.velocity.x > -LimitSpeed)
			{
				rb2D.AddForce(-transform.right * (MoveSpeed), ForceMode2D.Force);
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			now_move = (int)Direction.RIGHT;
			//最高速度になるとそれ以上加速しない
			if (rb2D.velocity.x < LimitSpeed)
			{
				rb2D.AddForce(transform.right * (MoveSpeed), ForceMode2D.Force);
			}
		}
		if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
			now_move = (int)Direction.STOP;
		}
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
			ground_hit = true;
		}
    }

    private void OnCollisionExit2D(Collision2D other)
    {
		if (other.gameObject.CompareTag("Ground"))
		{
			ground_hit = false;
		}
	}
}