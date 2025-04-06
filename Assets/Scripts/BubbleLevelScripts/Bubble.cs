using System;
using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector3 _startPosition;

    private float _XAxisdeviation = 0 ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody2D>();
        StartCoroutine(Xdeviation());
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _rigidbody.transform.position += new Vector3(_XAxisdeviation, 0.04f);

        if (_rigidbody.transform.position.y>9)
        {
            //Destroy(gameObject);
        }
    }
    
    IEnumerator Xdeviation()
    {
        _XAxisdeviation += UnityEngine.Random.Range(-0.0075f, 0.0075f);
        Debug.unityLogger.Log(_XAxisdeviation);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f,0.5f));
        //_XAxisdeviation = UnityEngine.Random.Range(-0.025f, 0.02f);
        StartCoroutine(Xdeviation());
    }
}
