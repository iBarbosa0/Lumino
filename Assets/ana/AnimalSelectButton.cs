using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimalSelectButton : MonoBehaviour
{
    public AnimalData myAnimal;

    public void OnAnimalButtonClick()
    {
        AnimalHolder.SelectedAnimal = myAnimal;
        SceneManager.LoadScene("AnimalScene"); 
    }
}
