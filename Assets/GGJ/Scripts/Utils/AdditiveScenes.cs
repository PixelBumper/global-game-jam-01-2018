using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveScenes : MonoBehaviour
{
    [SerializeField]
    private string[] _scenesNamesToAdd;

	void Start ()
    {
        for (int i = 0; i < _scenesNamesToAdd.Length; i++)
        {
            var sceneName = _scenesNamesToAdd[i];
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}
