using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    Button btn; // Referência ao componente Button
    Vector3 upScale = new Vector3(1.2f, 1.2f, 1); // Escala aumentada temporariamente

    private void Awake()
    {
        btn = gameObject.GetComponent<Button>(); // Obtém o botão associado a este GameObject
        btn.onClick.AddListener(Anim); // Associa a função de animação ao evento de clique
    }

    void Anim()
    {
        // Aumenta ligeiramente o botão (efeito de clique)
        LeanTween.scale(gameObject, upScale, 0.1f);
        // Retorna à escala normal após um pequeno delay
        LeanTween.scale(gameObject, Vector3.one, 0.1f).setDelay(0.1f);
    }
}
