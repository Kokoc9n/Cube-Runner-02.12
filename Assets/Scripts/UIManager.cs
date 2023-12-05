using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Core
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject startScreen;
        [SerializeField] GameObject endScreen;
        [SerializeField] Transform touchImageTransform;
        [SerializeField] Image endBackgroundImage;
        private GameManager gameManager;
        private float imageStartPosX;
        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            imageStartPosX = touchImageTransform.localPosition.x;
        }
        private void OnEnable()
        {
            gameManager.OnStateChange += StateChangeHandle;
            touchImageTransform.DOLocalMoveX(-imageStartPosX, 2f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
        private void OnDisable()
        {
            gameManager.OnStateChange -= StateChangeHandle;
        }
        private void StateChangeHandle()
        {
            if(gameManager.State == GameManager.GameState.Start)
            {
                startScreen.SetActive(true);
                endScreen.SetActive(false);
            }
            else if(gameManager.State == GameManager.GameState.GameEnd)
            {
                startScreen.SetActive(false);
                endScreen.SetActive(true);
                endBackgroundImage.DOFade(0.5f, 1f)
                    .SetEase(Ease.Linear);
            }
            else
            {
                startScreen.SetActive(false);
            }
        }

    }
}