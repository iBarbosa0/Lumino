using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AnimalSprite
{
    public Sprite closedMouth; // Sprite com a boca fechada
    public Sprite openMouth;   // Sprite com a boca aberta
}

public class XylophoneMinigame : MonoBehaviour
{
    public List<AnimalSprite> AnimalSprites;     // Lista de sprites dos animais (aberto/fechado)
    public List<Button> AnimalButtons;           // Bot�es clic�veis com imagens dos animais

    public List<AudioClip> AnimalSounds;         // <- Sons dos animais (na mesma ordem dos sprites)

    public Text LevelText;                       // Texto do n�vel atual
    public GameObject EndScene;                  // Painel de fim de jogo
    public Text HighscoreText;                   // Texto da pontua��o m�xima
    public GameObject StartPanel;                // Painel inicial antes de come�ar o jogo
    public Image FeedbackImage;                  // A imagem mostra o "certo" ou "errado"

    public Sprite CorrectSprite;                 // Sprite de feedback certo;
    public Sprite WrongSprite;                   // Sprite de feedback errado;
    public float FeedbackDuration = 1f;          // Tempo que o feedback aparece (em segundos)
    public float MouthOpenDuration = 0.5f;       // Dura��o da boca aberta ap�s o clique

    public AudioClip CorrectSound;              // Som para resposta certa
    public AudioClip WrongSound;                // Som para resposta errada

    private AudioSource audioSource;             // <- Fonte de �udio

    private List<int> Sequence = new List<int>(); // Lista com a sequ�ncia gerada
    private int ColorNumber = 0;                  // N�vel atual (quantidade de animais na sequ�ncia)
    private int ShowColor = 0;                    // �ndice da sequ�ncia que o jogador est� a tentar acertar
    private int StillMissing = 0;                 // Quantos animais faltam clicar corretamente
    private int Highscore = 0;                    // Maior n�vel atingido

    void Start()
    {
        StartPanel.SetActive(true);   // Mostra o painel inicial
        EndScene.SetActive(false);    // Esconde o painel de fim de jogo

        audioSource = GetComponent<AudioSource>(); // <- Pega o AudioSource

        // Define a opacidade e a boca fechada nos PreviewAnimals
        foreach (var button in AnimalButtons)
        {
            var img = button.GetComponent<Image>();
            var color = img.color;
            color.a = 0.2f; // Opacidade baixa
            img.color = color;
            img.sprite = AnimalSprites[0].closedMouth; // Inicia com boca fechada
        }
    }

    public void StartMinigame()
    {
        StartPanel.SetActive(false);  // Esconde o menu inicial
        StartCoroutine(StartGame());  // Come�a o jogo
    }

    IEnumerator StartGame()
    {
        // Reseta os Preview Animals para a opacidade baixa e boca fechada
        for (int i = 0; i < AnimalButtons.Count; i++)
        {
            var img = AnimalButtons[i].GetComponent<Image>();
            img.sprite = AnimalSprites[i].closedMouth;

            var color = img.color;
            color.a = 0.2f;
            img.color = color;
        }

        SetAnimalButtonsInteractable(false); // Desativa os bot�es
        yield return new WaitForSeconds(0.5f);
        Generator(); // Gera nova sequ�ncia
    }

    void Generator()
    {
        ColorNumber++; // Aumenta o n�vel
        LevelText.text = "Level: " + ColorNumber;

        // Aumenta o tamanho da sequ�ncia a cada dois n�veis (m�x 4)
        int sequenceLength = Mathf.Min(1 + (ColorNumber - 1) / 2, 4);

        Sequence.Clear();
        for (int i = 0; i < sequenceLength; i++)
        {
            Sequence.Add(Random.Range(0, AnimalSprites.Count)); // Adiciona um �ndice aleat�rio � sequ�ncia
        }

        ShowColor = 0;
        StillMissing = sequenceLength;

        StartCoroutine(RevealSequence()); // Mostra a sequ�ncia ao jogador
    }

    IEnumerator RevealSequence()
    {
        // Velocidade de exibi��o diminui com o n�vel (m�nimo 0.3s)
        float revealSpeed = Mathf.Clamp(1.5f - (ColorNumber * 0.05f), 0.3f, 1.5f);

        // Deixa todos os animais com opacidade reduzida e boca fechada
        for (int i = 0; i < AnimalButtons.Count; i++)
        {
            var img = AnimalButtons[i].GetComponent<Image>();
            img.sprite = AnimalSprites[i].closedMouth;

            var color = img.color;
            color.a = 0.2f;
            img.color = color;
        }

        yield return new WaitForSeconds(0.5f);

        // Mostra sequ�ncia de animais (boca aberta -> fechada)
        for (int i = 0; i < Sequence.Count; i++)
        {
            int id = Sequence[i];

            // Aumentar a opacidade e abrir a boca para o animal atual da sequ�ncia
            var img = AnimalButtons[id].GetComponent<Image>();
            img.sprite = AnimalSprites[id].openMouth;
            var color = img.color;
            color.a = 0.8f;
            img.color = color;
            
            PlayAnimalSound(id); // <- Toca o som
            yield return new WaitForSeconds(revealSpeed);

            // Depois de exibir, volta ao estado inicial
            img.sprite = AnimalSprites[id].closedMouth;
            color.a = 0.2f;
            img.color = color;

            yield return new WaitForSeconds(0.5f); // Pausa antes de passar para o pr�ximo
        }

        // Ap�s mostrar a sequ�ncia, ativa os bot�es com boca fechada
        for (int i = 0; i < AnimalButtons.Count; i++)
        {
            var img = AnimalButtons[i].GetComponent<Image>();
            img.sprite = AnimalSprites[i].closedMouth;

            var color = img.color;
            color.a = 1f;
            img.color = color;
        }

        SetAnimalButtonsInteractable(true);
    }

    IEnumerator ShowFeedback(Sprite spriteToShow, bool nextLevel)
    {
        FeedbackImage.sprite = spriteToShow;
        FeedbackImage.gameObject.SetActive(true);

        // Toca o som correto
        if (spriteToShow == CorrectSprite && CorrectSound != null)
        {
            audioSource.PlayOneShot(CorrectSound);
        }
        else if (spriteToShow == WrongSprite && WrongSound != null)
        {
            audioSource.PlayOneShot(WrongSound);
        }

        yield return new WaitForSeconds(FeedbackDuration);
        FeedbackImage.gameObject.SetActive(false);

        if (nextLevel)
        {
            StartCoroutine(StartGame()); // Pr�ximo n�vel
        }
        else
        {
            EndScene.SetActive(true); // Game over
        }
    }

    public void AnimalButton(int ID)
    {
        StartCoroutine(HandleAnimalClick(ID));
    }

    IEnumerator HandleAnimalClick(int ID)
    {
        // Desativa o bot�o temporariamente
        AnimalButtons[ID].interactable = false;

        // Mostra a boca aberta e toca o som
        var img = AnimalButtons[ID].GetComponent<Image>();
        img.sprite = AnimalSprites[ID].openMouth;

        // Garante opacidade total
        var color = img.color;
        color.a = 1f;
        img.color = color;

        PlayAnimalSound(ID);

        yield return new WaitForSeconds(MouthOpenDuration);

        // Fecha a boca e mant�m a opacidade total
        img.sprite = AnimalSprites[ID].closedMouth;
        color = img.color;
        color.a = 1f;
        img.color = color;

        // S� reativa se o jogador ainda estiver a jogar
        if (ID == Sequence[ShowColor])
        {
            ShowColor++;
            StillMissing--;

            if (StillMissing == 0)
            {
                SetAnimalButtonsInteractable(false);
                StartCoroutine(ShowFeedback(CorrectSprite, true));
            }
            else
            {
                AnimalButtons[ID].interactable = true;
            }
        }
        else
        {
            SetAnimalButtonsInteractable(false);
            Highscore = PlayerPrefs.GetInt("Highscore", 0);
            if (ColorNumber > Highscore)
            {
                Highscore = ColorNumber;
                PlayerPrefs.SetInt("Highscore", Highscore);
            }

            HighscoreText.text = "Highscore: " + Highscore;
            StartCoroutine(ShowFeedback(WrongSprite, false));
        }
    }

    public void TryAgain()
    {
        // Reinicia o jogo
        Sequence.Clear();
        ColorNumber = 0;
        EndScene.SetActive(false);
        StartCoroutine(StartGame());
    }

    void SetAnimalButtonsInteractable(bool active)
    {
        // Ativa ou desativa todos os bot�es de animal
        foreach (Button b in AnimalButtons)
        {
            b.interactable = active;
        }
    }

    void PlayAnimalSound(int id)
    {
        if (id >= 0 && id < AnimalSounds.Count && audioSource != null && AnimalSounds[id] != null)
        {
            audioSource.PlayOneShot(AnimalSounds[id]);
        }
    }
}

