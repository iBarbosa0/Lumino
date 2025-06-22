using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;

    public void GoToScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
