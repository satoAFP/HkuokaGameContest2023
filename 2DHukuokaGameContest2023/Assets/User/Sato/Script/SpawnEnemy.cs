using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField, Header("èoåªïpìx"),Range(0,500)]
    private int FrequencyAppearance;

    [SerializeField,Header("èoÇµÇΩÇ¢ìGÇÃéÌóﬁ")]
    public GameObject[] spawnEnemy;


    private int count = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        count++;
        if(FrequencyAppearance==count)
        {
            GameObject clone = Instantiate(spawnEnemy[Random.Range(0, spawnEnemy.Length)], new Vector3(20, Random.Range(0, 10), 0), Quaternion.identity);
            count = 0;
        }



    }
}
