using System.Collections;
using System.Collections.Generic;
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
    public List<Image> PreviewAnimals;           // Imagens para mostrar a sequência
    public List<Button> AnimalButtons;           // Botões clicáveis com imagens dos animais

    public List<AudioClip> AnimalSounds;         // <- Sons dos animais (na mesma ordem dos sprites)

    public Text LevelText;                       // Texto do nível atual
    public GameObject EndScene;                  // Painel de fim de jogo
    public Text HighscoreText;                   // Texto da pontuação máxima
    public GameObject StartPanel;                // Painel inicial antes de começar o jogo

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
    }

    public void StartMinigame()
    {
        StartPanel.SetActive(false);  // Esconde o menu inicial
        StartCoroutine(StartGame());  // Começa o jogo
    }

    IEnumerator StartGame()
    {
        // Limpa os previews
        foreach (var preview in PreviewAnimals)
        {
            preview.sprite = null;
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

        // Limpa previews
        for (int i = 0; i < PreviewAnimals.Count; i++)
        {
            PreviewAnimals[i].sprite = null;
        }

        // Mostra sequência de animais (boca aberta -> fechada)
        for (int i = 0; i < Sequence.Count; i++)
        {
            int id = Sequence[i];

            PreviewAnimals[i].sprite = AnimalSprites[id].openMouth;
            PlayAnimalSound(id); // <- Toca o som
            yield return new WaitForSeconds(revealSpeed);

            PreviewAnimals[i].sprite = AnimalSprites[id].closedMouth;
            yield return new WaitForSeconds(0.5f);
        }

        // Após mostrar a sequência, ativa os botões com boca fechada
        for (int i = 0; i < AnimalButtons.Count; i++)
        {
            AnimalButtons[i].GetComponent<Image>().sprite = AnimalSprites[i].closedMouth;
        }

        SetAnimalButtonsInteractable(true);
    }

    public void AnimalButton(int ID)
    {
        if (ID == Sequence[ShowColor])
        {
            // Resposta correta: mostra boca aberta
            AnimalButtons[ID].GetComponent<Image>().sprite = AnimalSprites[ID].openMouth;
            PlayAnimalSound(ID); // <- Toca o som

            ShowColor++;
            StillMissing--;

            if (StillMissing == 0)
            {
                // Passou o nível
                SetAnimalButtonsInteractable(false);
                StartCoroutine(StartGame());
            }
        }
        else
        {
            // Errou: fim de jogo
            EndScene.SetActive(true);

            Highscore = PlayerPrefs.GetInt("Highscore", 0);
            if (ColorNumber > Highscore)
            {
                Highscore = ColorNumber;
                PlayerPrefs.SetInt("Highscore", Highscore); // Guarda novo recorde
            }

            HighscoreText.text = "Highscore: " + Highscore;
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

