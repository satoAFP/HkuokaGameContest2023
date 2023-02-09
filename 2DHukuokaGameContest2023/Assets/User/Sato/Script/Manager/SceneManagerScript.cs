using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    [SerializeField, Header("ƒNƒŠƒbƒNŽž‚ÌES")] private AudioClip SE;

    private void Start()
    {
        ManagerAccessor.Instance.sceneManager = this;

        //Screen.SetResolution(1920, 1080, false, 60);
    }

    public void SceneMove(string name)
    {
        //SE–Â‚ç‚·
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        Cursor.visible = true;

        SceneManager.LoadScene(name);
    }

    public void SceneReload()
    {
        //SE–Â‚ç‚·
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
