using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    private void Start()
    {
        ManagerAccessor.Instance.sceneManager = this;
    }

    public void SceneMove(string name)
    {
        Cursor.visible = true;
        SceneManager.LoadScene(name);
    }

    public void SceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
