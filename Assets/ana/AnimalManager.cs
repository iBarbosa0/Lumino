using UnityEngine;
using System.Collections;

public class AnimalManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AudioSource audioSource;

    private AnimatorOverrideController overrideController;
    private AnimalData animalData;

    void Start()
    {
        animalData = AnimalHolder.SelectedAnimal;

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
        PlayIdle();
    }

    public void PlayIdle()
    {
        StopAllCoroutines(); // Stop any ongoing animation/audio coroutines
        animator.Play("Idle");
        PlaySound(animalData.idleAudio);
    }

    public void PlayEat()
    {
        StopAllCoroutines();
        StartCoroutine(PlayAnimationThenReturnToIdle("Eat", animalData.eatAudio));
    }

    public void PlayPlay()
    {
        StopAllCoroutines();
        StartCoroutine(PlayAnimationThenReturnToIdle("Play", animalData.playAudio));
    }

    private IEnumerator PlayAnimationThenReturnToIdle(string animationName, AudioClip clip)
    {
        // Stop idle sound
        if (audioSource.isPlaying)
            audioSource.Stop();

        // Play the new animation
        animator.Play(animationName);

        // Play audio
        PlaySound(clip);

        // Wait one frame to ensure Animator updates
        yield return null;

        // Get the length of the currently playing state
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float waitTime = stateInfo.length;

        yield return new WaitForSeconds(waitTime);

        // Return to Idle
        PlayIdle();
    }


    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public float GetAnimationLength(string name)
    {
        RuntimeAnimatorController rac = animator.runtimeAnimatorController;
        foreach (var clip in rac.animationClips)
        {
            if (clip.name == name)
                return clip.length;
        }
        return 1f;
    }
}
