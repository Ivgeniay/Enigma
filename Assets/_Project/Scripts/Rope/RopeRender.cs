using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RopeRender : MonoBehaviour
{
    [SerializeField] private Transform[] transforms;

    private List<Vector3> positions = new List<Vector3>();
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = transforms.Length;
        for (int i = 0; i < transforms.Length; i++)
            positions.Add(transforms[i].position);
    }

    private void Update() {
        for (int i = 0; i < transforms.Length; i++)
            positions[i] = transforms[i].position;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
