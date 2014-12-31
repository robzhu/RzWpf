using RzAspects;

namespace RzWpf
{
    public class WpfUpdateService : UpdateService
    {
        public WpfUpdateService() : base( new WpfFrameEventProvider() )
        {
            Unpause();
        }
    }
}
