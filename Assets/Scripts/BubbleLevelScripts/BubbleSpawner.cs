using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;



public class BubbleSpawner : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    public GameObject bubble;

    private void Start()
    {
        StartCoroutine(SpawnBubble());
    }

    private void FixedUpdate()
    {
        if(Input.GetMouseButton(0))
        {
            //Debug.Log("awsdf");
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        Debug.Log(eventData);
        
    }

    IEnumerator SpawnBubble()
    {
        
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 2));
        Vector3 SpawnPoint = new Vector3(UnityEngine.Random.Range(-7, 7), -7f);
        Instantiate(bubble,SpawnPoint, new Quaternion());
        StartCoroutine(SpawnBubble());
    }
    
}
