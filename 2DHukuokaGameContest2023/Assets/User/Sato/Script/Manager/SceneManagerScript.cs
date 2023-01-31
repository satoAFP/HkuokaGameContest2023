using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    [SerializeField, Header("クリック時のES")] private AudioClip SE;

    private void Start()
    {
        ManagerAccessor.Instance.sceneManager = this;
    }

    public void SceneMove(string name)
    {
        //SE鳴らす
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        SceneManager.LoadScene(name);
    }

    public void SceneReload()
    {
        //SE鳴らす
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
