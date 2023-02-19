using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecklaceDestroy : MonoBehaviour
{
    private void Awake()
    {
        if (Player.isNecllace)
            SelfDestroy();
    }
    private void SelfDestroy() =>
        Destroy(gameObject);
}
