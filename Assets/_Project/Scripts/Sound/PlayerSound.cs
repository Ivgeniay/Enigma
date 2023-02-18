using Unity.VisualScripting;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private AudioSource audioSource;

    private bool isStepPlay;

    [SerializeField] private AudioClip[] stepAudioClip;
    [SerializeField][Range(0, 1)] private float stepLevel;
    [SerializeField] private AudioClip[] jumpAudioClip;
    [SerializeField][Range(0, 1)] private float jumpLevel;
    [SerializeField] private AudioClip landingAudioClip;
    [SerializeField][Range(0, 1)] private float landingLevel;


    private void Awake() {
        audioSource= GetComponent<AudioSource>();
    }
    public void Move(Vector2 moveVector)
    {
    }

    public void Landing(float volume) {
        audioSource.Stop();
        landingLevel = volume;
        audioSource.clip = landingAudioClip;
        audioSource.volume = landingLevel;
        audioSource.Play();
    }

    public void Step() {
        audioSource.Stop();
        var rnd = Random.Range(0, stepAudioClip.Length);
        audioSource.clip = stepAudioClip[rnd];
        audioSource.volume = stepLevel;
        audioSource.Play();
    }
    public void Jump() {
        audioSource.Stop();
        var rnd = Random.Range(0, jumpAudioClip.Length);
        audioSource.clip = jumpAudioClip[rnd];
        audioSource.volume = jumpLevel;
        audioSource.Play();
    }


}
