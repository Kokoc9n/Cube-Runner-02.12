using UnityEngine;

namespace Services
{
    public class MobileInputService : InputService
    {
        public override bool OnClickDown => Input.GetTouch(0).phase == TouchPhase.Began;

        public override bool OnClickUp => Input.GetTouch(0).phase == TouchPhase.Ended;

        public override Vector2 PointerPos => Input.GetTouch(0).position;
        public override bool ClickDown => Input.GetTouch(0).tapCount == 1;
    }
}