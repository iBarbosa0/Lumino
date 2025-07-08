using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Object = System.Object;


public class DrawOnScreen : MonoBehaviour
{
    private Vector3 _quadPosition;
    public Texture2D texture;
    private RectTransform _rectTransform;
    private Renderer _renderQuad;
    private int _lastPixelX;
    private int _lastPixelY;
    private bool _isdrawing = false;
    public Color drawColor = Color.white; // White for chalk digits
    public Color backgroundColor = new Color(0.1f, 0.2f, 0.1f); // Dark green chalkboard
    public Texture2D chalkTexture;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderQuad = FindAnyObjectByType<Renderer>();
        _renderQuad.material = new Material(Shader.Find("Unlit/Texture"));
        PaintBoardBlack();
    }

    private void FixedUpdate()
    {
        //Get the mouse position in relation to the center of the screen
        Vector3 worldPosition =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,1f));
       //make it so the center of the screen is in relation to the "Quad" which means that if the mouse is in the center of the "Quad" Game object the mouse position should be (x = 0,y = 0)
        Vector3 relativePosition = worldPosition - _quadPosition;
        //Make it so the center of the quad is not (0,0), it makes the bottom left of the quad is the (0,0) position and top right is the furthest position(it is easier to work with pixels if we setup it up like this);
        Vector3 scalePosition = new Vector3(relativePosition.x /5 +0.5f,relativePosition.y /5 + 0.5f ,relativePosition.z);

        //called when mouse button is down
        if (Input.GetMouseButton(0))
        {
            //transform the x and y position into pixels (100*100)
            int pixelX = Mathf.FloorToInt(scalePosition.x * 100);
            //Debug.Log("Pixel X: " + pixelX);
            int pixelY = Mathf.FloorToInt(scalePosition.y * 100);
            //Debug.Log("Pixel Y: " + pixelY);

            if (_isdrawing == false)
            {
                _lastPixelX = pixelX; 
                _lastPixelY = pixelY;
                _isdrawing = true;
            }
            DrawLine(_lastPixelX, _lastPixelY, pixelX, pixelY);
            _renderQuad.material.mainTexture = texture;
            _lastPixelX = pixelX;
            _lastPixelY = pixelY;
        }
        else
        {
            _isdrawing = false;
        }
    }
  
    //Function To draw. The loop is for brush size instead of 1*1 pixel size without, with the loop is 5*5
    void DrawBrush(int pixelX, int pixelY)
    {
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++) 
            {
                // This is not to draw out of bounds of the blackboard
                if (!(pixelX + i >= 100 || pixelX + i <= 0) && !(pixelY + j >= 100 || pixelY + j <= 0))
                {
                    // Perlin noise for chalk irregularity
                    float noise = Mathf.PerlinNoise((pixelX + i + Time.time) * 0.2f, (pixelY + j + Time.time) * 0.2f);
                    float opacity = Mathf.Lerp(0.7f, 1.0f, noise);
                    Color chalkColor = new Color(drawColor.r, drawColor.g, drawColor.b, opacity);

                    // Optional: Blend with chalk texture if assigned
                    if (chalkTexture != null)
                    {
                        int texX = Mathf.FloorToInt((i + 2) * chalkTexture.width / 5f);
                        int texY = Mathf.FloorToInt((j + 2) * chalkTexture.height / 5f);
                        Color textureColor = chalkTexture.GetPixel(texX, texY);
                        chalkColor = new Color(chalkColor.r * textureColor.r, chalkColor.g * textureColor.g, chalkColor.b * textureColor.b, opacity * textureColor.a);
                    }

                    // Blend with existing pixel for chalk dust effect
                    Color current = texture.GetPixel(pixelX + i, pixelY + j);
                    Color blended = Color.Lerp(current, chalkColor, chalkColor.a);
                    texture.SetPixel(pixelX + i, pixelY + j, blended);
                }
            }
        }
        texture.Apply();
        _renderQuad.material.mainTexture = texture;
    }
    //Bresenham’s algorithm to help draw a line between to 2 points
    void DrawLine(int x0, int y0, int x1, int y1)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        int x = x0;
        int y = y0;

        while (true)
        {
            DrawBrush(x, y);
            if (x == x1 && y == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x += sx; }
            if (e2 < dx) { err += dx; y += sy; }
        }
    }
    
    //Reset Blackboard to Black
    public void PaintBoardBlack()
    {
        _quadPosition = transform.position;
        texture = new Texture2D(100, 100, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                texture.SetPixel(i,j,Color.black);
            }
        }
        texture.Apply();
        _renderQuad.GetComponent<Renderer>().material.mainTexture = texture;  
    }
}
