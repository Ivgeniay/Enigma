using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reloaded : MonoBehaviour
{
    private float delay = 1;
    private Vector3 rebirthPlace;
    private Player player;

    private static Reloaded _instance = null;
    public static Reloaded Instance {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("Reloaded");
                _instance = go.AddComponent<Reloaded>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public void Instantiate(Player player) {
        this.player = player;
        player.OnPlayerDeadEvent += OnPlayerDeadHandler;
        if (rebirthPlace == Vector3.zero)
            SetRebirthPlace(player.transform.position);
        else
            player.transform.position = rebirthPlace;
    }

    public void SetRebirthPlace(Vector3 place) =>
        rebirthPlace = place;
    

    private void OnPlayerDeadHandler() {
        player.OnPlayerDeadEvent -= OnPlayerDeadHandler;
        player = null;
        StartCoroutine(Restart(delay));
    }

    private IEnumerator Restart(float delay) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

