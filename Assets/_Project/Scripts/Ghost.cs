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
        if (player) {
            if (transform.position.x > player.position.x)
                transform.rotation = Quaternion.identity;
            else
                transform.rotation = Quaternion.Euler(new Vector2(0, -180));

            transform.position = Vector3.Lerp(transform.position, player.position, speed * Time.deltaTime);

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minPosX, maxPosX),
                Mathf.Clamp(transform.position.y, minPosY, maxPosY),
                transform.position.z
                );
        }
    }

    public void SetBorder(float minPosX, float maxPosX, float minPosY, float maxPosY) {
        this.maxPosX = maxPosX;
        this.minPosX = minPosX;
        this.maxPosY = maxPosY;
        this.minPosY = minPosY;
    }
}
