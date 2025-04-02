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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _renderQuad = FindAnyObjectByType<Renderer>();
        PaintBoardBlack();
    }

    private void FixedUpdate()
    {
        //Get the mouse position in relation to the center of the screen
        Vector3 worldPosition =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,1f));
       //make it so the center of the screen is in relation to the "Quad" which means that if the mouse is in the center of the "Quad" Game object the mouse position should be (x = 0,y = 0)
        Vector3 relativePosition = worldPosition - _quadPosition;
        //Make it so the center of the quad is not (0,0), it makes the bottom left of the quad is the (0,0) position and top right is the furthest position(it is easier to work with pixels if we setup it up like this);
        Vector3 scalePosition = new Vector3(relativePosition.x /3 +0.5f,relativePosition.y /3 + 0.5f ,relativePosition.z);

        //called when mouse button is down
        if (Input.GetMouseButton(0))
        {
            //transform the x and y position into pixels (100*100)
            int pixelX = Mathf.FloorToInt(scalePosition.x * 100);
            Debug.Log("Pixel X: " + pixelX);
            int pixelY = Mathf.FloorToInt(scalePosition.y * 100);
            Debug.Log("Pixel Y: " + pixelY);
            if (!(pixelX>100 || pixelX<0) && !(pixelY>100 || pixelY<0))
            {
                for (int i = -2; i < 2; i++)
                {
                    for (int j = -2; j < 2; j++)
                    {
                        texture.SetPixel(pixelX+i,pixelY+j,Color.white);
                    }
                }
                texture.Apply();
            }
            GetComponent<Renderer>().material.mainTexture = texture;
        }
        
    }
    
    public void PaintBoardBlack()
    {
        _quadPosition = transform.position;
        texture =  new Texture2D(100,100);
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
