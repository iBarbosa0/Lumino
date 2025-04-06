using System;
using UnityEngine;
using System.Collections;



public class BubbleSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject bubble;

    private void Start()
    {
        StartCoroutine(SpawnBubble());
    }
    
    IEnumerator SpawnBubble()
    {
        
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 2));
        Vector3 SpawnPoint = new Vector3(UnityEngine.Random.Range(-7, 7), -7f);
        Instantiate(bubble,SpawnPoint, new Quaternion());
        StartCoroutine(SpawnBubble());
    }
    
}
