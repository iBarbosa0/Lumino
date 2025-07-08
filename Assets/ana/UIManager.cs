using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject feedButton;
    public GameObject playButton;
    public GameObject backButton;

    public GameObject foodPrefab;
    public GameObject toyPrefab;

    public Transform spawnPoint;
    public AnimalManager animalManager;

    private GameObject spawnedObject;

    public void OnFeedButtonClick()
    {
        feedButton.SetActive(false);
        playButton.SetActive(false);
        backButton.SetActive(true);

        spawnedObject = Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity);
        spawnedObject.GetComponent<ObjectThrow>().Init("Eat", this);
    }

    public void OnPlayButtonClick()
    {
        feedButton.SetActive(false);
        playButton.SetActive(false);
        backButton.SetActive(true);

        spawnedObject = Instantiate(toyPrefab, spawnPoint.position, Quaternion.identity);
        spawnedObject.GetComponent<ObjectThrow>().Init("Play", this);
    }

    public void OnBackButtonClick()
    {
        if (feedButton.activeSelf && playButton.activeSelf)
        {
            // Normal UI state — go to main menu
            SceneManager.LoadScene("Scene Pickanimals"); // Replace with your main menu scene name
        }
        else
        {
            // Throwable UI state — destroy spawned object if exists
            if (spawnedObject != null)
            {
                Destroy(spawnedObject);
                spawnedObject = null;
            }

            // Show all buttons again
            feedButton.SetActive(true);
            playButton.SetActive(true);
            backButton.SetActive(true);
        }
    }

    public void HandleAfterThrow(string animationName)
    {
        StartCoroutine(WaitForAnimation(animationName));
    }

    private IEnumerator WaitForAnimation(string animationName)
    {
        if (animationName == "Eat")
            animalManager.PlayEat();
        else if (animationName == "Play")
            animalManager.PlayPlay();

        float animLength = animalManager.GetAnimationLength(animationName);
        yield return new WaitForSeconds(animLength);

        // Show feed, play, and back buttons after animation ends
        feedButton.SetActive(true);
        playButton.SetActive(true);
        backButton.SetActive(true);
    }
}
