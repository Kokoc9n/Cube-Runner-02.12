using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] Transform cubeHolder;
        [SerializeField] Transform stickman;
        [SerializeField] GameObject cube;
        [SerializeField] GameObject textPopup;
        [SerializeField] AnimationCurve bounceAnimationCurve;
        [SerializeField] ParticleSystem cubeStackEffect;
        private int height = 1;
        private GameManager gameManager;
        private CameraScript cameraScript;
        private new Collider collider;
        private List<GameObject> stack = new();
        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            collider = GetComponent<Collider>();
            cameraScript = FindObjectOfType<CameraScript>();
        }
        private void Start()
        {
            // Count first box.
            stack.Add(cubeHolder.GetChild(0).gameObject);
        }
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Pickup"))
            {
                stickman.transform.position += Vector3.up;
                var _cube = Instantiate(cube, cubeHolder.position + Vector3.up * height, Quaternion.identity, cubeHolder);
                stack.Add(_cube);
                height++;
                animator.Play("Jumping");
                for (int i = 0; i < stack.Count; i++)
                {
                    stack[i].transform.DOLocalJump(Vector3.up * i, (i * 3 / stack.Count) / 2, 1, 0.3f).SetEase(Ease.Linear); 
                }
                stickman.transform.DOLocalJump(Vector3.up * height, (height + 1) * 3 / stack.Count / 2, 1, 0.5f);
                cubeStackEffect.Play();

                var popup = Instantiate(textPopup, stickman);
                popup.transform.DOLocalMove(new Vector3(1, 3, 10), 3);
                StartCoroutine(DestroyWithDelay(5, popup));
            }
        }
        public void WallCollision(bool[] toRemove)
        {
            List<GameObject> blockedCubes = new();
            if (toRemove.Length >= height)
            {
                gameManager.ChangeGameState(GameManager.GameState.GameEnd);
                // Ragdoll.
                // No ragdoll, sorry.
                return;
            }
            for (int i = 0; i < toRemove.Length; i++)
            {
                if (toRemove[i])
                {
                    var cube = stack[i];
                    blockedCubes.Add(cube);
                    cube.transform.SetParent(gameManager.transform.root);
                    StartCoroutine(DestroyWithDelay(Constants.CUBE_CLEANUP_DELAY, cube.gameObject));
                    height--;
                }
            }
            foreach (var cube in blockedCubes) stack.Remove(cube);
            CubeStackFalling(blockedCubes);
            CubeStackFalling(stack);
            StartCoroutine(cameraScript.Shake(shakeDuration: 0.1f,
                                  decreaseFactor: 0.5f,
                                  shakeAmount: 0.1f));
        }
        private void CubeStackFalling(List<GameObject> stack)
        {
            collider.enabled = false;
            gameManager.AllowMovement = false;
            for (int i = 0; i < stack.Count; i++)
            {
                stack[i].transform.DOLocalMoveY(i, Constants.DROP_ANIMATION_DURATION)
                    .SetEase(bounceAnimationCurve)
                    .SetDelay(Constants.DROP_ANIMATION_DURATION / 3);
            }
            stickman.transform.DOLocalMoveY(height, Constants.DROP_ANIMATION_DURATION)
                .SetEase(bounceAnimationCurve)
                .SetDelay(Constants.DROP_ANIMATION_DURATION / 3)
                .OnComplete(() =>
                {
                    gameManager.AllowMovement = true;
                    collider.enabled = true;
                });
        }
        private IEnumerator DestroyWithDelay(float delay, GameObject targetGameObject)
        {
            yield return GetWait(delay);
            Destroy(targetGameObject);
        }
        private readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
        private WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }
    }
}