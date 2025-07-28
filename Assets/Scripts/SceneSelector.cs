using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
   [SerializeField] private int Scene;

   public void SelectScene()
   {
      SceneManager.LoadScene(Scene);
   }
}
