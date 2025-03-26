using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLevelSelect : MonoBehaviour
{
    //Function to go back to Level Select scene from one of the levels
    public void returnToLevelSelect()
    {
        SceneManager.LoadScene(1);
    }
}
