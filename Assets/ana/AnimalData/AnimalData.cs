using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Animal/Create New Animal")]
public class AnimalData : ScriptableObject
{
    public string animalName;

    public AnimationClip idleAnim;
    public AnimationClip eatAnim;
    public AnimationClip playAnim;

    public GameObject foodPrefab;
    public GameObject toyPrefab;

    public AudioClip idleAudio;
    public AudioClip eatAudio;
    public AudioClip playAudio;
}
