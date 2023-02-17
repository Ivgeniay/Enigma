using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private float duration;

    private Coroutine animationCoroutine;

    private Animator animator;
    private int start = Animator.StringToHash("Start");
    private int fade = Animator.StringToHash("Fade");


    private void Awake() {
        if (!textMeshPro) textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }

    public void ShowMessage(string text)
    {
        if (animator.GetBool(start)) {
            textMeshPro.text = text;
            animationCoroutine = StartCoroutine(WaitingClose(duration));
        }
        else {
            textMeshPro.text = text;
            animationCoroutine = StartCoroutine(WaitingClose(duration));
            animator.SetBool(start, true);
        }
    }

    public void CloseMessage() {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animator.SetTrigger(fade);
        animator.SetBool(start, false);
    }

    private IEnumerator WaitingClose(float duration)
    {
        yield return new WaitForSeconds(duration);
        CloseMessage();
    }
}
