using System.Collections;
using TMPro;
using UnityEngine;

public class GoodGhostTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private float delay;
    [SerializeField] private string text;

    private bool isActive = true;

    private Coroutine coroutine;

    private int appearance = Animator.StringToHash("Appearance");
    private int fade = Animator.StringToHash("Fade");

    private void Awake() {
        textMeshPro.text = text;
    }

    private void OnTriggerEnter(Collider other) {
        if (string.IsNullOrEmpty(text)) return;

        var player = other.GetComponent<Player>();
        if (player && isActive) {
            isActive = false;
            animator.SetTrigger(appearance);
            coroutine = StartCoroutine(StopAnimation(delay));
        }
    }

    private IEnumerator StopAnimation(float delay) {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(fade);
        isActive = true;
    }
}
