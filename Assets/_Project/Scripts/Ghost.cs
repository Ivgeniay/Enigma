using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed;

    [SerializeField] private float minPosX;
    [SerializeField] private float maxPosX;
    [SerializeField] private float minPosY;
    [SerializeField] private float maxPosY;

    private void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Transform>();
    }

    private void Update() {
        if (player)
            transform.position = Vector3.Lerp(transform.position, player.position, speed * Time.deltaTime);
    }
}
