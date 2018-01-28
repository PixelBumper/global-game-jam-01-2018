using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcherOnSpace : MonoBehaviour
{

    [SerializeField] private string _nextSceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(_nextSceneName);
        }
    }

}
