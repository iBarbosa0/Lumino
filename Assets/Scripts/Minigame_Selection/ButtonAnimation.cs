using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    Button btn; // Refer�ncia ao componente Button
    Vector3 upScale = new Vector3(1.2f, 1.2f, 1); // Escala aumentada temporariamente

    private void Awake()
    {
        btn = gameObject.GetComponent<Button>(); // Obt�m o bot�o associado a este GameObject
        btn.onClick.AddListener(Anim); // Associa a fun��o de anima��o ao evento de clique
    }

    void Anim()
    {
        // Aumenta ligeiramente o bot�o (efeito de clique)
        LeanTween.scale(gameObject, upScale, 0.1f);
        // Retorna � escala normal ap�s um pequeno delay
        LeanTween.scale(gameObject, Vector3.one, 0.1f).setDelay(0.1f);
    }
}
