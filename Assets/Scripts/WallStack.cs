using UnityEngine;

namespace Core
{
    public class WallStack : MonoBehaviour
    {
        [SerializeField] GameObject wall;
        [SerializeField] bool[] walls;
        private Player player;
        private void Awake()
        {
            player = FindObjectOfType<Player>();
        }
        void Start()
        {
            InitWall();
        }
        private void InitWall()
        {
            for (int i = 0; i < walls.Length; i++)
            {
                if (walls[i] != false)
                {
                    var wallTransform = Instantiate(wall, transform).transform;
                    wallTransform.position = transform.position + new Vector3(0, i);
                }
            }
        }
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Player"))
                player.WallCollision(walls);
        }
    }
}