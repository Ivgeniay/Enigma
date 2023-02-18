using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reloaded : MonoBehaviour
{
    private float delay = 1;
    private Player player;

    private Dictionary<int, Vector3> checkpointSceneStore = new Dictionary<int, Vector3>();
    private List<CheckpointZoneTrigger> CheckPointList = new List<CheckpointZoneTrigger>();

    private static Reloaded _instance = null;
    public static Reloaded Instance {
        get {
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

        var rebirthPlace = GetRebirthPlace(SceneManager.GetActiveScene().buildIndex);

        if (rebirthPlace == Vector3.zero)
            SetRebirthPlace(player.transform.position);
        else
            player.transform.position = rebirthPlace;
    }

    public void SetRebirthPlace(Vector3 place, CheckpointZoneTrigger zone = null) {

        if (zone != null) {
            if(CheckPointList.Contains(zone))
                return;
            else CheckPointList.Add(zone);
        }

        var sceneId = SceneManager.GetActiveScene().buildIndex;

        if (checkpointSceneStore.TryGetValue(sceneId, out var scene))
            checkpointSceneStore[sceneId] = place;
        else
            checkpointSceneStore.Add(sceneId, place);
    }

    public Vector3 GetRebirthPlace(int sceneId) {
        if (checkpointSceneStore.TryGetValue(sceneId, out var place))
            return place;
        else 
            return Vector3.zero;
    }
    

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

