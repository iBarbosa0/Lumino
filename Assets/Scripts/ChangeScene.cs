using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Permite manipular cenas

public class ChangeScene : MonoBehaviour
{
    // Muda para uma cena com base no ID fornecido
    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID); // Troca para a cena correspondente ao ID
    }
}
