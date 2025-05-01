using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage; // Número máximo de páginas
    int currentPage; // Página atual
    Vector3 targetPos; // Posição de destino do container de páginas
    [SerializeField] Vector3 pageStep; // Distância entre páginas
    [SerializeField] RectTransform levelPagesRect; // Referência ao objeto que contém as páginas

    [SerializeField] float tweenTime; // Duração da animação de transição
    [SerializeField] LeanTweenType tweenType; // Tipo de interpolação usado na animação
    float dragTreshould; // Distância mínima para reconhecer um swipe

    [SerializeField] Image[] barImage; // Indicadores visuais (barras) de página
    [SerializeField] Sprite barClosed, barOpen; // Sprites para barra fechada/aberta

    [SerializeField] Button previousBtn, nextBtn; // Botões de navegação

    private void Awake()
    {
        currentPage = 1; // Começa na primeira página
        targetPos = levelPagesRect.localPosition; // Define a posição inicial
        dragTreshould = Screen.width / 15; // Define o limite mínimo para detectar swipe
        UpdateBar(); // Atualiza as barras visuais
        UpdateArrowButton(); // Atualiza o estado dos botões
    }

    // Avança para a próxima página, se não for a última
    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    // Volta para a página anterior, se não for a primeira
    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    // Move para a posição da página atual com animação
    void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
        UpdateBar();
        UpdateArrowButton();
    }

    // Detecta fim de swipe e decide se é para mudar de página
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragTreshould)
        {
            if(eventData.position.x>eventData.pressPosition.x) Previous();
            else Next();
        }
        else
        {
            MovePage();
        }
    }

    // Atualiza os indicadores de página (barras)
    void UpdateBar()
    {
        foreach (var item in barImage)
        { 
            item.sprite = barClosed;
        }
        barImage[currentPage - 1].sprite = barOpen;
    }

    // Ativa/desativa os botões consoante a página atual
    void UpdateArrowButton()
    {
        nextBtn.interactable = true;
        previousBtn.interactable = true;
        if (currentPage == 1) previousBtn.interactable = false;
        else if (currentPage == maxPage) nextBtn.interactable = false;
    }
}
