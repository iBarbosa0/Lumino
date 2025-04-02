using System;
using UnityEngine;

public class CleanBoard : MonoBehaviour
{
    private Renderer _rendererQuad;
    private DrawOnScreen _drawOnScreenScript;

    private void Start()
    {
        _rendererQuad = FindAnyObjectByType<Renderer>();
        _drawOnScreenScript = _rendererQuad.GetComponent<DrawOnScreen>();
        Debug.Log(_rendererQuad);
        Debug.Log(_drawOnScreenScript);
    }

    public void Clean()
    {
        Vector3 quadPosition = _rendererQuad.transform.position;
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                _drawOnScreenScript.texture.SetPixel(i,j,Color.black);
            }
        }
        _drawOnScreenScript.texture.Apply();
        _rendererQuad.GetComponent<Renderer>().material.mainTexture = _drawOnScreenScript.texture;
    }
}
