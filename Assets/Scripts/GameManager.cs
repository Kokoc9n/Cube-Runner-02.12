using UnityEngine;
using Zenject;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using Services;

namespace Core
{
    public class GameManager : MonoBehaviour
    {

        private const float MOVE_SPEED = 10f;
        public GameState State { get; private set; }
        public bool AllowMovement { get => allowMovement; set => allowMovement = value; }
        public Transform PlayerGroup { get => playerGroup; private set => playerGroup = value; }

        public Action OnStateChange;
        [SerializeField] Transform playerGroup;
        [SerializeField] Transform player;
        [SerializeField] ParticleSystem warpEffect;

        private InputService inputService;
        private bool allowMovement = true;

        public enum GameState
        {
            Start = 0,
            GameLoop = 1,
            GameEnd = 2
        }

        [Inject]
        public void Construct(InputService inputService)
        {
            this.inputService = inputService;
        }
        private void OnEnable()
        {
            OnStateChange += ToggleWarpEffect;
        }
        private void OnDisable()
        {
            OnStateChange -= ToggleWarpEffect;
        }
        private void Start()
        {
            StartCoroutine(WaitForInteract());
        }
        private void ToggleWarpEffect()
        {
            if (State == GameState.GameLoop)
                warpEffect.Play();
            else warpEffect.Stop();
        }
        
        private void HorizontalMovement()
        {
            if (allowMovement == false 
                || inputService.ClickDown == false) return;
            Vector3 _targetPos = Vector3.right
                * Camera.main.ScreenToViewportPoint(inputService.PointerPos).x
                * Constants.WIDTH_OF_TRACKGROUND * 2 
                - new Vector3(Constants.WIDTH_OF_TRACKGROUND, 0);
            _targetPos = Vector3.ClampMagnitude(_targetPos, Constants.WIDTH_OF_TRACKGROUND);
            player.localPosition = Vector3.Lerp(player.localPosition, _targetPos, 0.1f);
        }
        
        private IEnumerator WaitForInteract()
        {
            while (true)
            {
                if (inputService.OnClickDown)
                {
                    ChangeGameState(GameState.GameLoop);
                    break;
                }
                yield return null;
            }
        }
        public void ChangeGameState(GameState state)
        {
            State = state;
            OnStateChange?.Invoke();
        }
        public void Restart() => SceneManager.LoadScene("GameScene", LoadSceneMode.Single);

        private void Update()
        {
            if (State != GameState.GameLoop) return;
            playerGroup.Translate(Vector3.forward * MOVE_SPEED * Time.deltaTime);
            HorizontalMovement();
        }
    }
}