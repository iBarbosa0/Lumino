using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XylophoneMinigame : MonoBehaviour
{
    public Image PreviewImage;

    public GameObject DoNotPressImage;

    public List<Color> ColorList;

    public int ColorNumber;
    public int ShowColor;

    public int StillMissing;
    public Text StillMissingText;

    public List<int> Sequence;

    public Text LevelText;

    public GameObject EndScene;

    public int Highscore;
    public Text HighscoreText;

   void Start()
    {
        Sequence = new List<int>();
        StartCoroutine(StartGame());
    }

    public void Generator()
    {
        ColorNumber++;

        LevelText.text = "Level: " + ColorNumber;

        Sequence.Add(Random.Range(0, 4));

        ShowPreview();
    }

    public void ShowPreview()
    {
        if (Sequence.Count <= ShowColor)
        {
            PreviewImage.color = Color.white;
            ShowColor = 0;
            StillMissing = Sequence.Count;
            StillMissingText.text = StillMissing.ToString();
            DoNotPressImage.SetActive(false);
        }
        else
        {
            PreviewImage.color = ColorList[Sequence[ShowColor]];

            StartCoroutine(ShowNext());
        }
    }

    public void ColorButton(int ID)
    {
        if(ID == Sequence[ShowColor])
        {
            ShowColor++;
            StillMissing--;
            StillMissingText.text = StillMissing.ToString();

            if (StillMissing == 0)
            {
                DoNotPressImage.SetActive(true);
                StillMissingText.text = "";

                ShowColor = 0;

                StartCoroutine(StartGame());
            }

        }
        else
        {
            EndScene.SetActive(true);
            DoNotPressImage.SetActive(true);
            Highscore = PlayerPrefs.GetInt("Highscore", Highscore);
            if(ColorNumber > Highscore)
            {
                Highscore = ColorNumber;
                PlayerPrefs.SetInt("Highscore: ", Highscore);
            }
            HighscoreText.text = "Highscore: " + Highscore;
            StillMissingText.text = "";
            StillMissing = 0;
            ShowColor = 0;
        }
    }

    public void TryAgain()
    {
        Sequence = new List<int>();
        ColorNumber = 0;
        LevelText.text = "Level: " + ColorNumber;   
        EndScene.SetActive(false);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);

        Generator();
    }

    IEnumerator ShowNext()
    {
        yield return new WaitForSeconds(0.3f);

        PreviewImage.color = Color.white;

        yield return new WaitForSeconds(0.7f);

        ShowColor++;

        ShowPreview();
    }
}
