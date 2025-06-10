using UnityEngine;

public class DestroySplash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SFXManager.SfxManagerInstance.PlayLetterFallingOnWater(transform.position);
        Destroy(gameObject,2f);
    }
}
