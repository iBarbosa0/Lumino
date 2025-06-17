using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class LevelSelector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void LoadBubbleMinigame()
    {
        SceneManager.LoadScene(6);
    }
    public void LoadMathMinigame()
    {
        SceneManager.LoadScene(7);
    }
    public void LoadAnimalMinigame()
    {
        SceneManager.LoadScene(5);
    }
    public void LoadShopMinigame()
    {
        SceneManager.LoadScene(4);
    }
    public void LoadAnmals()
    {
        SceneManager.LoadScene(2);
    }
    

    

}
