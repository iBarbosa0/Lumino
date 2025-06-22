using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public int scenenumber;

    public void GoToScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
