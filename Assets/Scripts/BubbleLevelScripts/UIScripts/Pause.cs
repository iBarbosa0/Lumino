using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    
    [SerializeField]  GameObject pauseMenu;


    public void PauseGame()
    {
        if (pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void HomeScreen()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
