using UnityEngine;

namespace Services
{
    public class StandaloneInputService : InputService
    {
        public override bool OnClickDown => IsClicked();
        public override bool OnClickUp => IsUp();
        public override Vector2 PointerPos => Input.mousePosition;
        public override bool ClickDown => Input.GetMouseButton(0);

        private static bool IsClicked() => Input.GetMouseButtonDown(0);
        private static bool IsUp() => Input.GetMouseButtonUp(0);
    }
}