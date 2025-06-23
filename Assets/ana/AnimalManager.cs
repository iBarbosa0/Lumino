using UnityEngine;
using System.Collections;

public class AnimalManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    private AnimatorOverrideController overrideController;

    void Start()
    {
        AnimalData animalData = AnimalHolder.SelectedAnimal;

        // Create a new AnimatorOverrideController based on the current controller
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        // Replace the clips with animal-specific clips
        overrideController["idleAnim"] = animalData.idleAnim;
        overrideController["eatAnim"] = animalData.eatAnim;
        overrideController["playAnim"] = animalData.playAnim;

        // Assign the override controller to animator
        animator.runtimeAnimatorController = overrideController;

        // Play idle animation with a tiny delay to ensure controller is applied
        StartCoroutine(PlayIdleNextFrame());
    }

    IEnumerator PlayIdleNextFrame()
    {
        yield return null;  // wait for one frame
        animator.Play("Idle");
    }

    public void PlayEat() => animator.Play("Eat");
    public void PlayPlay() => animator.Play("Play");
}
