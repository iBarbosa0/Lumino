using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonsScript : MonoBehaviour
{
   public void WorldMapButton()
   {
      SceneManager.LoadScene(8);
   }
   public void ExitGame()
   {
      Application.Quit();
      //EditorApplication.Exit(0);
   }
}
