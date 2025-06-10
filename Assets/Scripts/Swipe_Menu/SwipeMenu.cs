using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject scrollbar; // Referência ao objeto Scrollbar do menu
    float scroll_pos = 0; // Armazena a posição atual do scroll
    float[]pos; // Posições-alvo para cada item no menu

    void Start()
    {
        // Nada definido no Start neste momento
    }

    void Update()
    {
        // Define um array com posições igualmente distribuídas com base no número de filhos (itens do menu)
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f); // Espaço entre cada item no scroll

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i; // Preenche o array com as posições-alvo
        }
        // Se o botão do rato estiver pressionado, atualiza a posição do scroll com base na Scrollbar
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar> ().value;
        }
        else // Se o botão não estiver pressionado, ajusta o scroll suavemente para a posição mais próxima
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance/2) && scroll_pos > pos[i] - (distance/2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp (scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        // Aumenta o tamanho do item central e reduz os outros, criando efeito de destaque
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp (transform.GetChild(i).localScale, new Vector2(1f,1f), 0.1f);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f,0.8f), 0.1f);
                    }
                }

            }
        }
    }   
}
