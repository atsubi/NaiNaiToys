using VContainer;
using VContainer.Unity;

namespace Manager {

    public class GameStatusLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameStatusManager>(Lifetime.Singleton);
        }
    }

}
