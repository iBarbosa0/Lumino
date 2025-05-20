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
    public List<Button> AnimalButtons;           // Botões clicáveis com imagens dos animais

    public List<AudioClip> AnimalSounds;         // <- Sons dos animais (na mesma ordem dos sprites)

    public Text LevelText;                       // Texto do nível atual
    public GameObject EndScene;                  // Painel de fim de jogo
    public Text HighscoreText;                   // Texto da pontuação máxima
    public GameObject StartPanel;                // Painel inicial antes de começar o jogo
    public Image FeedbackImage;                  // A imagem mostra o "certo" ou "errado"

    public Sprite CorrectSprite;                 // Sprite de feedback certo;
    public Sprite WrongSprite;                   // Sprite de feedback errado;
    public float FeedbackDuration = 1f;          // Tempo que o feedback aparece (em segundos)
    public float MouthOpenDuration = 0.5f;       // Duração da boca aberta após o clique

    public AudioClip CorrectSound;              // Som para resposta certa
    public AudioClip WrongSound;                // Som para resposta errada

    private AudioSource audioSource;             // <- Fonte de áudio

    private List<int> Sequence = new List<int>(); // Lista com a sequência gerada
    private int ColorNumber = 0;                  // Nível atual (quantidade de animais na sequência)
    private int ShowColor = 0;                    // Índice da sequência que o jogador está a tentar acertar
    private int StillMissing = 0;                 // Quantos animais faltam clicar corretamente
    private int Highscore = 0;                    // Maior nível atingido

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
        StartCoroutine(StartGame());  // Começa o jogo
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

        SetAnimalButtonsInteractable(false); // Desativa os botões
        yield return new WaitForSeconds(0.5f);
        Generator(); // Gera nova sequência
    }

    void Generator()
    {
        ColorNumber++; // Aumenta o nível
        LevelText.text = "Level: " + ColorNumber;

        // Aumenta o tamanho da sequência a cada dois níveis (máx 4)
        int sequenceLength = Mathf.Min(1 + (ColorNumber - 1) / 2, 4);

        Sequence.Clear();
        for (int i = 0; i < sequenceLength; i++)
        {
            Sequence.Add(Random.Range(0, AnimalSprites.Count)); // Adiciona um índice aleatório à sequência
        }

        ShowColor = 0;
        StillMissing = sequenceLength;

        StartCoroutine(RevealSequence()); // Mostra a sequência ao jogador
    }

    IEnumerator RevealSequence()
    {
        // Velocidade de exibição diminui com o nível (mínimo 0.3s)
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

        // Mostra sequência de animais (boca aberta -> fechada)
        for (int i = 0; i < Sequence.Count; i++)
        {
            int id = Sequence[i];

            // Aumentar a opacidade e abrir a boca para o animal atual da sequência
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

            yield return new WaitForSeconds(0.5f); // Pausa antes de passar para o próximo
        }

        // Após mostrar a sequência, ativa os botões com boca fechada
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
            StartCoroutine(StartGame()); // Próximo nível
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
        // Desativa o botão temporariamente
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

        // Fecha a boca e mantém a opacidade total
        img.sprite = AnimalSprites[ID].closedMouth;
        color = img.color;
        color.a = 1f;
        img.color = color;

        // Só reativa se o jogador ainda estiver a jogar
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
        // Ativa ou desativa todos os botões de animal
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

