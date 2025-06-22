using UnityEngine;
using UnityEngine.Rendering;

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
}
