using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI
{
    public class RootScope :LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();
            
            builder.RegisterBuildCallback(c =>
            {
                var options = c.Resolve<MessagePipeOptions>();
                options.EnableCaptureStackTrace = false;
            });
        }
    }
}