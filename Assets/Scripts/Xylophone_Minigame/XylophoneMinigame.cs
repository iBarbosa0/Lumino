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
    public List<Image> PreviewAnimals;           // Imagens para mostrar a sequ�ncia
    public List<Button> AnimalButtons;           // Bot�es clic�veis com imagens dos animais

    public List<AudioClip> AnimalSounds;         // <- Sons dos animais (na mesma ordem dos sprites)

    public Text LevelText;                       // Texto do n�vel atual
    public GameObject EndScene;                  // Painel de fim de jogo
    public Text HighscoreText;                   // Texto da pontua��o m�xima
    public GameObject StartPanel;                // Painel inicial antes de come�ar o jogo

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
    }

    public void StartMinigame()
    {
        StartPanel.SetActive(false);  // Esconde o menu inicial
        StartCoroutine(StartGame());  // Come�a o jogo
    }

    IEnumerator StartGame()
    {
        // Limpa os previews
        foreach (var preview in PreviewAnimals)
        {
            preview.sprite = null;
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

        // Limpa previews
        for (int i = 0; i < PreviewAnimals.Count; i++)
        {
            PreviewAnimals[i].sprite = null;
        }

        // Mostra sequ�ncia de animais (boca aberta -> fechada)
        for (int i = 0; i < Sequence.Count; i++)
        {
            int id = Sequence[i];

            PreviewAnimals[i].sprite = AnimalSprites[id].openMouth;
            PlayAnimalSound(id); // <- Toca o som
            yield return new WaitForSeconds(revealSpeed);

            PreviewAnimals[i].sprite = AnimalSprites[id].closedMouth;
            yield return new WaitForSeconds(0.5f);
        }

        // Ap�s mostrar a sequ�ncia, ativa os bot�es com boca fechada
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
                // Passou o n�vel
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

