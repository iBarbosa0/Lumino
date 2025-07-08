using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
   
    [SerializeField]  GameObject MainMenuUI;


    public void MainMenu()
    {
        if (MainMenuUI.activeSelf == true)
        {
            MainMenuUI.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            MainMenuUI.SetActive(false);
            Time.timeScale = 0;
        }
    }
}
