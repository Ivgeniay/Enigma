using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodGhost : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int idle1 = Animator.StringToHash("Idle1");
    private int idle2 = Animator.StringToHash("Idle2");

    private void Awake()
    {
        var rndNum = Random.Range(0, 2);
        if (rndNum == 0)
            animator.SetBool(idle1, true);
        else 
            animator.SetBool(idle2, true);
    }
}
