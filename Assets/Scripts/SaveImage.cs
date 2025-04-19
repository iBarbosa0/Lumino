using Unity.Sentis;
using UnityEngine;

public class SaveImage : MonoBehaviour
{
    
    private Renderer _rendererQuad;
    private DrawOnScreen _drawOnScreenScript;

    private void Start()
    {
        _rendererQuad = FindAnyObjectByType<Renderer>();
        _drawOnScreenScript = _rendererQuad.GetComponent<DrawOnScreen>();
    }
    public void Saveimage()
    {
        byte[] imageBytes = _drawOnScreenScript.texture.EncodeToPNG();
        // Save to file (optional)
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/drawing.png", imageBytes);
        // Use imageBytes for number recognition
    }
}
