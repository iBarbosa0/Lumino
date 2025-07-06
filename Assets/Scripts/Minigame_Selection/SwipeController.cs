using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int maxPage; // N�mero m�ximo de p�ginas
    int currentPage; // P�gina atual
    Vector3 targetPos; // Posi��o de destino do container de p�ginas
    [SerializeField] Vector3 pageStep; // Dist�ncia entre p�ginas
    [SerializeField] RectTransform levelPagesRect; // Refer�ncia ao objeto que cont�m as p�ginas

    [SerializeField] float tweenTime; // Dura��o da anima��o de transi��o
    [SerializeField] LeanTweenType tweenType; // Tipo de interpola��o usado na anima��o
    float dragTreshould; // Dist�ncia m�nima para reconhecer um swipe

    [SerializeField] Image[] barImage; // Indicadores visuais (barras) de p�gina
    [SerializeField] Sprite barClosed, barOpen; // Sprites para barra fechada/aberta

    [SerializeField] Button previousBtn, nextBtn; // Bot�es de navega��o

    private void Awake()
    {
        currentPage = 1; // Come�a na primeira p�gina
        targetPos = levelPagesRect.localPosition; // Define a posi��o inicial
        dragTreshould = Screen.width / 15; // Define o limite m�nimo para detectar swipe
        UpdateBar(); // Atualiza as barras visuais
        UpdateArrowButton(); // Atualiza o estado dos bot�es
    }

    // Avan�a para a pr�xima p�gina, se n�o for a �ltima
    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    // Volta para a p�gina anterior, se n�o for a primeira
    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    // Move para a posi��o da p�gina atual com anima��o
    void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
        UpdateBar();
        UpdateArrowButton();
    }

    // Detecta fim de swipe e decide se � para mudar de p�gina
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

    // Atualiza os indicadores de p�gina (barras)
    void UpdateBar()
    {
        foreach (var item in barImage)
        { 
            item.sprite = barClosed;
        }
        barImage[currentPage - 1].sprite = barOpen;
    }

    // Ativa/desativa os bot�es consoante a p�gina atual
    void UpdateArrowButton()
    {
        nextBtn.interactable = true;
        previousBtn.interactable = true;
        if (currentPage == 1) previousBtn.interactable = false;
        else if (currentPage == maxPage) nextBtn.interactable = false;
    }
}
