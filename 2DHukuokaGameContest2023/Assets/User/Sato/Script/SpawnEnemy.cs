using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField, Header("出現頻度"),Range(0,500)]
    private int FrequencyAppearance;

    [SerializeField, Header("最大出現数")]
    private int spawnMax;

    [SerializeField, Header("スポーンする下ライン")]
    public float spawnUnderLine;

    [SerializeField, Header("スポーンする上ライン")]
    public float spawnOverLine;

    [SerializeField, Header("スポーンするX座標")]
    public float spawnXpos;

    [SerializeField,Header("出したい左移動敵の種類")]
    public GameObject[] spawnLeftEnemy;

    [SerializeField, Header("出したい右移動敵の種類")]
    public GameObject[] spawnRightEnemy;


    private int FrequencyCount = 0;     //出現頻度カウント用
    private int spawnCount = 0;         //スポーン数カウント用

    // Update is called once per frame
    void FixedUpdate()
    {

        if (spawnMax == spawnCount)
        {
            FrequencyCount++;
            if (FrequencyAppearance == FrequencyCount)
            {
                //左出現
                if (Random.Range(0, 2) == 0)
                {
                    GameObject clone = Instantiate(spawnLeftEnemy[Random.Range(0, spawnLeftEnemy.Length)], new Vector3(spawnXpos, Random.Range(spawnUnderLine, spawnOverLine), 0), Quaternion.identity);
                }
                //右出現
                else
                {
                    GameObject clone = Instantiate(spawnRightEnemy[Random.Range(0, spawnLeftEnemy.Length)], new Vector3(-spawnXpos, Random.Range(spawnUnderLine, spawnOverLine), 0), Quaternion.identity);
                }
                //スポーンカウント
                spawnCount++;

                //カウントリセット
                FrequencyCount = 0;
            }
        }


    }
}
