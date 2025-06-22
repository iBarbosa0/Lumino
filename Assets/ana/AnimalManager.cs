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

        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        overrideController["idleAnim"] = animalData.idleAnim;
        overrideController["eatAnim"] = animalData.eatAnim;
        overrideController["playAnim"] = animalData.playAnim;

        animator.runtimeAnimatorController = overrideController;

        StartCoroutine(PlayIdleNextFrame());
    }

    IEnumerator PlayIdleNextFrame()
    {
        yield return null;
        animator.Play("Idle");
    }

    public void PlayEat() => animator.Play("Eat");
    public void PlayPlay() => animator.Play("Play");

    public float GetAnimationLength(string animName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animName)
                return clip.length;
        }
        return 1f;
    }
}

