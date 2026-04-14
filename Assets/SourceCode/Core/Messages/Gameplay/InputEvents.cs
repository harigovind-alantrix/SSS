namespace Core.Messages.Gameplay
{
    public readonly struct MoveInputEvent
    {
        public readonly float Axis;
        public MoveInputEvent(float axis) => Axis = axis;
    }

    public readonly struct JumpInputEvent { }

    public readonly struct CloneInputEvent
    {
        public readonly float Axis;
        public CloneInputEvent(float axis) => Axis = axis;
    }
}