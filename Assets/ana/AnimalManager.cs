using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    void Start()
    {
        AnimalData animalData = AnimalHolder.SelectedAnimal;



        var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController["Idle"] = animalData.idleAnim;
        overrideController["Eat"] = animalData.eatAnim;
        overrideController["Play"] = animalData.playAnim;
        animator.runtimeAnimatorController = overrideController;

        animator.Play("Idle");
    }

    public void PlayEat() => animator.Play("Eat");
    public void PlayPlay() => animator.Play("Play");
}
