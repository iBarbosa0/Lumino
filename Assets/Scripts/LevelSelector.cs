using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class LevelSelector : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private int correspondentLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(correspondentLevel);
        SceneManager.LoadScene(correspondentLevel);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    

}
