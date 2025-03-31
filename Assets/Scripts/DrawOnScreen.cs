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
    private Texture2D _texture;
    private RectTransform _rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        _quadPosition = FindAnyObjectByType<Renderer>().transform.position;
       // Debug.Log("Quad Position"+ _quadPosition);
        _rectTransform = GetComponent<RectTransform>();
        _texture =  new Texture2D(100,100);
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                _texture.SetPixel(i,j,Color.black);
            }
        }
       _texture.Apply();
        GetComponent<Renderer>().material.mainTexture = _texture;
    }

    private void FixedUpdate()
    {
        
        Vector3 worldPosition =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,1f));
       // Debug.Log("World Position "+worldPosition);
        Vector3 relativePosition = worldPosition - _quadPosition;
        
        //Debug.Log("Relative Quad Position " + relativePosition);
        Vector3 scalePosition = new Vector3(relativePosition.x /3 +0.5f,relativePosition.y /3 + 0.5f ,relativePosition.z);
        //Debug.Log("Scale Position" + scalePosition);

        if (Input.GetMouseButton(0))
        {
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
                        _texture.SetPixel(pixelX+i,pixelY+j,Color.white);
                    }
                }
                _texture.Apply();
                Debug.Log("Entrou");
            }
            GetComponent<Renderer>().material.mainTexture = _texture;
        }
    }
}
