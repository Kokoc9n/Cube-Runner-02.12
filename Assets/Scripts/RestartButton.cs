using UnityEngine;

namespace Core
{
    public class RestartButton : ButtonClickHandler
    {
        private GameManager gameManager;
        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        public override void OnButtonClicked()
        {
            gameManager.Restart();
        }
    }
}