using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEffect : MonoBehaviour
{
    [SerializeField, Header("エフェクト")] private GameObject Effect;

    private bool first = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //出現処理
        if(Input.GetMouseButton(0))
        {
            if (first)
                OnEffect();
            first = false;
        }
        else
        {
            first = true;
        }
    }

    public void OnEffect()
    {
        Vector2 mousePos = Input.mousePosition;
        // スクリーン座標のZ値を5に変更  
        Vector3 screenPos = new Vector3(mousePos.x, mousePos.y, 5f);
        // ワールド座標に変換  
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        // ワールド座標を3Dオブジェクトの座標に適用  
        transform.localPosition = worldPos;

        //エフェクト生成
        GameObject clone = Instantiate(Effect, Vector3.zero, Quaternion.identity);
        clone.transform.localPosition = worldPos;
    }
}
