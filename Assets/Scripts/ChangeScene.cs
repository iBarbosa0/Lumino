using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Permite manipular cenas

public class ChangeScene : MonoBehaviour
{
    public string panelToActivate; // Definir isto no inspector (nome do panel a ativar)

    // Muda para uma cena com base no ID fornecido
    public void MoveToScene(int sceneID)
    {
        SceneData.PanelToActivate = panelToActivate; // Guarda o nome do panel
        SceneManager.LoadScene(sceneID); // Troca para a cena correspondente ao ID
    }
}
