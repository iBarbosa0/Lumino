using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonsScript : MonoBehaviour
{
   public void WorldMapButton()
   {
      SceneManager.LoadScene(1);
   }
   public void ExitGame()
   {
      Application.Quit();
      //EditorApplication.Exit(0);
   }
}
