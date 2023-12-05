using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonClickHandler : MonoBehaviour
    {
        private Button button;
        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
        }
        public abstract void OnButtonClicked();
    }
}