using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    [SerializeField, Header("�N���b�N����ES")] private AudioClip SE;

    private void Start()
    {
        ManagerAccessor.Instance.sceneManager = this;

        //Screen.SetResolution(1920, 1080, false, 60);
    }

    public void SceneMove(string name)
    {
        //SE�炷
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        Cursor.visible = true;

        SceneManager.LoadScene(name);
    }

    public void SceneReload()
    {
        //SE�炷
        gameObject.GetComponent<AudioSource>().PlayOneShot(SE);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
