using Unity.Sentis;
using UnityEngine;

public class SaveImage : MonoBehaviour
{
    
    private Renderer _rendererQuad;
    private DrawOnScreen _drawOnScreenScript;
    private RecognizeNumber _recognizeNumber;
    
    private void Start()
    {
        _rendererQuad = FindAnyObjectByType<Renderer>();
        _drawOnScreenScript = _rendererQuad.GetComponent<DrawOnScreen>();
        _recognizeNumber = FindAnyObjectByType<RecognizeNumber>();
    }
    public void Saveimage()
    {
        Debug.Log(_recognizeNumber);
        byte[] imageBytes = _drawOnScreenScript.texture.EncodeToPNG();
        // Save to file (optional)
        Texture2D resize =  ResizeTextureAccurate(_drawOnScreenScript.texture,28,28);
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/drawing.png", imageBytes);
       // _recognizeNumber.Infer(resize);
       MathGameManager.Instance.checkifimageiscorrect(0);
       // Debug.Log(number);
    }
    
    Texture2D ResizeTextureAccurate(Texture2D source, int targetWidth, int targetHeight)
{
    // Step 1: Center the digit by cropping to bounding box
    int minX = source.width, maxX = 0, minY = source.height, maxY = 0;
    for (int x = 0; x < source.width; x++)
    {
        for (int y = 0; y < source.height; y++)
        {
            if (source.GetPixel(x, y).grayscale < 0.5f) // Detect dark pixels
            {
                minX = Mathf.Min(minX, x);
                maxX = Mathf.Max(maxX, x);
                minY = Mathf.Min(minY, y);
                maxY = Mathf.Max(maxY, y);
            }
        }
    }

    // Handle case where no digit is drawn
    if (minX >= maxX || minY >= maxY)
    {
        Debug.LogWarning("No digit detected; returning blank 28x28 texture");
        Texture2D blank = new Texture2D(targetWidth, targetHeight);
        for (int x = 0; x < targetWidth; x++)
            for (int y = 0; y < targetHeight; y++)
                blank.SetPixel(x, y, Color.white);
        blank.Apply();
        return blank;
    }

    // Crop to bounding box
    int width = maxX - minX + 1;
    int height = maxY - minY + 1;
    Texture2D cropped = new Texture2D(width, height);
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            cropped.SetPixel(x, y, source.GetPixel(minX + x, minY + y));
        }
    }
    cropped.Apply();

    // Step 2: Pad to square (maintain aspect ratio)
    int maxDim = Mathf.Max(width, height);
    Texture2D padded = new Texture2D(maxDim, maxDim);
    for (int x = 0; x < maxDim; x++)
    {
        for (int y = 0; y < maxDim; y++)
        {
            int srcX = x * width / maxDim;
            int srcY = y * height / maxDim;
            padded.SetPixel(x, y, cropped.GetPixel(srcX, srcY));
        }
    }
    padded.Apply();
    Destroy(cropped);

    // Step 3: Resize to 28x28 with bilinear filtering
    padded.filterMode = FilterMode.Bilinear;
    RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
    RenderTexture.active = rt;
    Graphics.Blit(padded, rt);
    Texture2D resized = new Texture2D(targetWidth, targetHeight);
    resized.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
    resized.Apply();
    RenderTexture.active = null;
    RenderTexture.ReleaseTemporary(rt);
    Destroy(padded);

    // Step 4: Apply threshold to sharpen (MNIST-like contrast)
    for (int x = 0; x < targetWidth; x++)
    {
        for (int y = 0; y < targetHeight; y++)
        {
            float grayscale = resized.GetPixel(x, y).grayscale;
            Color color = grayscale < 0.5f ? Color.black : Color.white; // Binary threshold
            resized.SetPixel(x, y, color);
        }
    }
    resized.Apply();

    return resized;
}
}
