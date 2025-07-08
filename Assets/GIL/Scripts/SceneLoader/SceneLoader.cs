using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string m_sceneName;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadScene(m_sceneName);
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
