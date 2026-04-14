namespace Core.Messages.Gameplay
{
    public readonly struct CameraZoomEvent
    {
        public readonly bool ZoomIn;
        
        public CameraZoomEvent(bool zoomIn)
        {
            ZoomIn = zoomIn;
        }
    }
}