using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private Note note;

    private bool isVisited;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player && !isVisited) {
            note.CloseMessage();
            note.ShowMessage(text);
            isVisited = true;
        }
    }
}
