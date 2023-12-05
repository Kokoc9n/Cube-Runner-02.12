using UnityEngine;

namespace Services
{
    public abstract class InputService : IInputService
    {
        public abstract bool OnClickDown { get; }
        public abstract bool OnClickUp { get; }
        public abstract Vector2 PointerPos { get; }
        public abstract bool ClickDown { get; }
    }
}