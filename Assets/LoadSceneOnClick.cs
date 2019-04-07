using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public void LoadByIndex(int _sceneIndex) {
        SceneManager.LoadScene(_sceneIndex);
    }
}
