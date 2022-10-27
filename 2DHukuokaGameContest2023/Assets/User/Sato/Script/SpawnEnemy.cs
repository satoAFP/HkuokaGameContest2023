using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField, Header("�o���p�x"),Range(0,500)]
    private int FrequencyAppearance;

    [SerializeField, Header("�ő�o����")]
    private int spawnMax;

    [SerializeField, Header("�X�|�[�����鉺���C��")]
    public float spawnUnderLine;

    [SerializeField, Header("�X�|�[������ド�C��")]
    public float spawnOverLine;

    [SerializeField, Header("�X�|�[������X���W")]
    public float spawnXpos;

    [SerializeField,Header("�o���������ړ��G�̎��")]
    public GameObject[] spawnLeftEnemy;

    [SerializeField, Header("�o�������E�ړ��G�̎��")]
    public GameObject[] spawnRightEnemy;


    private int FrequencyCount = 0;     //�o���p�x�J�E���g�p
    private int spawnCount = 0;         //�X�|�[�����J�E���g�p

    // Update is called once per frame
    void FixedUpdate()
    {

        if (spawnMax == spawnCount)
        {
            FrequencyCount++;
            if (FrequencyAppearance == FrequencyCount)
            {
                //���o��
                if (Random.Range(0, 2) == 0)
                {
                    GameObject clone = Instantiate(spawnLeftEnemy[Random.Range(0, spawnLeftEnemy.Length)], new Vector3(spawnXpos, Random.Range(spawnUnderLine, spawnOverLine), 0), Quaternion.identity);
                }
                //�E�o��
                else
                {
                    GameObject clone = Instantiate(spawnRightEnemy[Random.Range(0, spawnLeftEnemy.Length)], new Vector3(-spawnXpos, Random.Range(spawnUnderLine, spawnOverLine), 0), Quaternion.identity);
                }
                //�X�|�[���J�E���g
                spawnCount++;

                //�J�E���g���Z�b�g
                FrequencyCount = 0;
            }
        }


    }
}
