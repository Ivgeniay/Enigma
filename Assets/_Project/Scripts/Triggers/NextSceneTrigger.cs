using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<Player>();
        if (player)
            NextScene(sceneName);
    }

    public void NextScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
