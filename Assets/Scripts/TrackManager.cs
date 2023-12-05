using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class TrackManager : MonoBehaviour
    {
        [SerializeField] GameObject groundPrefab;
        [SerializeField] GameObject pickupPrefab;
        private GameManager gameManager;
        private Queue<GameObject> trackList = new();
        private int trackgroundIndex = 1;
        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        private void Start()
        {
            PreloadTracks();
        }
        private void PreloadTracks()
        {
            for (int i = 0; i < Constants.DEFAULT_TRACK_CAPACITY; i++)
            {
                var track = Instantiate(groundPrefab);
                track.transform.position = new Vector3(0,
                                                       0,
                                                       i * Constants.LENGTH_OF_TRACKGROUND);
                trackList.Enqueue(track);

                // Ignore first track.
                if(i != 0)
                {
                    ChooseRandomWall(track.transform);
                    SpawnPickups(track.transform);
                }

            }
        }
        private void TrackSwapLogic()
        {
            if ((gameManager.PlayerGroup.transform.position.z - Constants.LENGTH_OF_TRACKGROUND) / trackgroundIndex
                >= Constants.LENGTH_OF_TRACKGROUND)
            {
                var track = trackList.First();
                trackList.Dequeue();
                // Reset.
                var wallsContainer = track.transform.GetChild(0).GetChild(0);
                var pickupsContainer = track.transform.GetChild(1);
                foreach (Transform child in wallsContainer) child.gameObject.SetActive(false);
                foreach (Transform child in pickupsContainer) Destroy(child.gameObject);

                track.transform.position = new Vector3(0,
                                                       0,
                                                       (Constants.DEFAULT_TRACK_CAPACITY - 1 + trackgroundIndex++) 
                                                       * Constants.LENGTH_OF_TRACKGROUND);
                track.transform.DOMoveY(-50, Constants.TRACK_MOVE_DURATION)
                    .From()
                    .SetEase(Ease.Linear);
                trackList.Enqueue(track);

                ChooseRandomWall(track.transform);
                SpawnPickups(track.transform);
            }
        }
        private void SpawnPickups(Transform track)
        {
            for (int i = 0; i <= Random.Range(3, 7); i++)
            {
                if(Random.value > 0.5f)
                {
                    var position = track.position
                                                + new Vector3(0, 0, Constants.PICKUPS_EDGE_POSITION)
                                                + Vector3.forward * (i * Constants.PICKUPS_PADDING)
                                                + Vector3.right * Random.Range(-1, 2);
                    Instantiate(pickupPrefab, position, Quaternion.identity, track.GetChild(1));
                }
            }
            
        }
        private void ChooseRandomWall(Transform track)
        {
            var list = new List<Transform>();
            foreach (Transform child in track.GetChild(0).GetChild(0))
            {
                list.Add(child);
            }
            list[Random.Range(0, list.Count)].gameObject.SetActive(true);
        }
        private void Update()
        {
            TrackSwapLogic();
        }
    }
}