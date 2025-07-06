using UnityEngine;

public class PanelActivator : MonoBehaviour
{
    public GameObject[] panels; // Lista com todos os panels poss�veis

    void Start()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        if (!string.IsNullOrEmpty(SceneData.PanelToActivate))
        {
            GameObject targetPanel = System.Array.Find(panels, p => p.name == SceneData.PanelToActivate);
            if (targetPanel != null)
            {
                targetPanel.SetActive(true);
            }

            SceneData.PanelToActivate = null; // Limpa para n�o repetir
        }
        else if (panels.Length > 0)
        {
            panels[0].SetActive(true); // Por padr�o, ativa o primeiro
        }
    }  
}
