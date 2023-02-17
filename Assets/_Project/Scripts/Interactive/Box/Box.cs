using System.Collections;
using UnityEngine;


public class Box : MonoBehaviour, ICapableMoving
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private AudioClip pushingClip;
    [SerializeField][Range(0, 1)] private float pushingLevel;
    [SerializeField] private AudioClip fallingClip;
    [SerializeField][Range(0, 1)] private float fallingLevel;
    private Coroutine coroutine;

    private void Awake() {
        rigidbody= GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Move(Vector3 vector) {
        rigidbody.AddForce(vector, ForceMode.Impulse);

        audioSource.clip= pushingClip;
        audioSource.volume = pushingLevel;
        audioSource.Play();
        if (coroutine is not null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(StopSound());
    }

    private IEnumerator StopSound() {
        yield return new WaitForSeconds(0.5f);
        coroutine = null;
        audioSource.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (!player) {
            if (collision.impulse.y > 1) {
                var level = Mathf.Clamp(collision.impulse.y, 0f, 4);
                level = Mathf.InverseLerp(0, 4, level);
                level = Mathf.Lerp(0, fallingLevel, level);

                audioSource.clip = fallingClip;
                audioSource.volume = level;
                audioSource.Play();
            }
        }
    }
}

