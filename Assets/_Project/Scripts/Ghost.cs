using System;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed;

    [SerializeField] private float minPosX;
    [SerializeField] private float maxPosX;
    [SerializeField] private float minPosY;
    [SerializeField] private float maxPosY;

    [SerializeField] private Vector3 offset;

    #region Ghost die

    [Header("----- Ghost Die -----")]
    [SerializeField] private float timeToFade = 1f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform objectToRemove;

    [SerializeField] private Transform effectToSpawn;
    [SerializeField] private Transform effectSpawnPosition;
    
    private Material material;
    private static readonly int FadeAmount = Shader.PropertyToID("_FadeAmount");

    #endregion
   

    private bool isDied = false;
    
    private void Awake() {
        material = spriteRenderer.material;
    }

    private void Start() {
        player = FindObjectOfType<Player>().GetComponent<Transform>();
    }

    private void Update() {
        if (isDied)
        {
            ProcessDie();
        }
        if (player & !isDied) {
            if (transform.position.x > player.position.x)
                transform.rotation = Quaternion.identity;
            else
                transform.rotation = Quaternion.Euler(new Vector2(0, -180));

            transform.position = Vector3.Lerp(transform.position, player.position + offset, speed * Time.deltaTime);

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

    public void Died() { 
        isDied = true;
        Destroy(objectToRemove.gameObject);
        Instantiate(effectToSpawn, effectSpawnPosition.position, Quaternion.identity);
        Destroy(gameObject, timeToFade);
    }
    
    private void ProcessDie()
    {
        material.SetFloat(FadeAmount, Mathf.Lerp(material.GetFloat(FadeAmount), 1, Time.deltaTime * timeToFade));
    }
    
}
