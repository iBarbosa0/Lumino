using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XylophoneMinigame : MonoBehaviour
{
    public List<Color> ColorList; // Lista de cores
    public List<Image> PreviewColors; // 4 imagens de PreviewColors
    public List<Button> ColorButtons; // Botões clicáveis

    public GameObject DoNotPressImage;
    public Text LevelText;
    public GameObject EndScene;
    public Text HighscoreText;

    private List<int> Sequence = new List<int>();
    private int ColorNumber = 0;
    private int ShowColor = 0;
    private int StillMissing = 0;
    private int Highscore = 0;

    void Start()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        // Inicializa PreviewColors a branco
        foreach (var preview in PreviewColors)
        {
            preview.color = Color.white;
        }

        // Desativa os ColorButtons enquanto mostra sequência
        SetColorButtonsInteractable(false);

        yield return new WaitForSeconds(0.5f);

        Generator();
    }

    void Generator()
    {
        ColorNumber++;
        LevelText.text = "Level: " + ColorNumber;

        int sequenceLength = Mathf.Min(1 + (ColorNumber - 1) / 2, 4); // Ex: niv.1-2: 1 cor, 3-4: 2 cores, etc.

        Sequence.Clear();
        for (int i = 0; i < sequenceLength; i++)
        {
            Sequence.Add(Random.Range(0, ColorList.Count));
        }

        ShowColor = 0;
        StillMissing = sequenceLength;

        StartCoroutine(RevealSequence());
    }

    IEnumerator RevealSequence()
    {
        float revealSpeed = Mathf.Clamp(1.0f - (ColorNumber * 0.05f), 0.3f, 1.0f); // Fica mais rápido com o nível

        // Quando o jogo começa, as PreviewColors começam a branco
        for (int i = 0; i < PreviewColors.Count; i++)
        {
            PreviewColors[i].color = Color.white;
        }

        for (int i = 0; i < Sequence.Count; i++)
        {
            // Mostra a cor da sequência
            PreviewColors[i].color = ColorList[Sequence[i]];

            yield return new WaitForSeconds(revealSpeed);

            // Volta a branco
            PreviewColors[i].color = Color.white;

            yield return new WaitForSeconds(0.2f);
        }

        // Quando a sequência termina, o jogador pode clicar
        SetColorButtonsInteractable(true);
    }

    public void ColorButton(int ID)
    {
        if (ID == Sequence[ShowColor])
        {
            ShowColor++;
            StillMissing--;

            if (StillMissing == 0)
            {
                SetColorButtonsInteractable(false);
                StartCoroutine(StartGame());
            }
        }
        else
        {
            EndScene.SetActive(true);
            Highscore = PlayerPrefs.GetInt("Highscore", 0);
            if (ColorNumber > Highscore)
            {
                Highscore = ColorNumber;
                PlayerPrefs.SetInt("Highscore", Highscore);
            }

            HighscoreText.text = "Highscore: " + Highscore;
        }
    }

    public void TryAgain()
    {
        Sequence.Clear();
        ColorNumber = 0;
        EndScene.SetActive(false);
        StartCoroutine(StartGame());
    }

    void SetColorButtonsInteractable(bool active)
    {
        foreach (Button b in ColorButtons)
        {
            b.interactable = active;
        }
    }
}
