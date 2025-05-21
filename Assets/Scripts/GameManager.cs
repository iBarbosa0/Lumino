using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance { get; protected set; } = null; 
    
    private void Awake()
    {
        if(GameManagerInstance == null)
        {
            GameManagerInstance = this;
            Debug.Log("instance");
        }
        else if(GameManagerInstance != this)
        {
            Debug.Log("destory");
            Destroy(gameObject);
        }
        if(SceneManager.GetActiveScene().buildIndex != 1)
        {   
            
            DontDestroyOnLoad(GameManagerInstance);
        }
    }
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
