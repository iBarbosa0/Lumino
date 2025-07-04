using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Bubble : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector3 _startPosition;
    private Animator _animator;
    public bool WasItPoped { get; private set; } = true;
    [SerializeField] private GameObject letterbubblechild;
    [SerializeField] private GameObject splashAnimation;

    private float _XAxisdeviation = 0 ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>(); // Animator to control the animation of the bubble popping
        _startPosition = transform.position; // get the position
        _rigidbody = GetComponent<Rigidbody2D>();// get the rigidbody of the bubble
        StartCoroutine(Xdeviation()); // starting the coroutine to periodically spawn bubbles
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Bubble Movement 
        _rigidbody.transform.position += new Vector3(_XAxisdeviation, 0.04f);

        //Destroy bubble when it reaches a certain Y axis value (9 in this case)
    }
    
    void OnMouseDown()
    {
        _animator.SetTrigger("Bubble Pop"); // Activate this animation when the mouse is clicked on top of the bubble;
    }

    //This function is called at a certain frame in the animation "Bubble Pop" so it is only destroyed when the animation finishes
    void Destroybubble()
    {
        //_bubbleSoundSource.resource = bubblepopSound;
        SFXManager.SfxManagerInstance.PlayBubblePop(transform.position);
        predestroyBubble();
    }
    
    //this is a Enumerator to make the bubble change its X axis to give the feeling of floating every x seconds
    IEnumerator Xdeviation()
    {
        _XAxisdeviation += UnityEngine.Random.Range(-0.0075f, 0.0075f);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f,0.5f));
        //_XAxisdeviation = UnityEngine.Random.Range(-0.025f, 0.02f);
        StartCoroutine(Xdeviation());
    }
    
    private void predestroyBubble()
    {
        letterbubblechild.transform.SetParent(null);
        letterbubblechild.GetComponent<BubbleLetter>().destroyBubble();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ceiling"))
        {
            Destroy(gameObject);
        }
    }
}
