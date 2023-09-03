using VContainer;
using VContainer.Unity;

namespace Toybox {

    public class ToyboxLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<DebugAcceptToyProvider>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<ToyboxPresenter>(Lifetime.Singleton);
            builder.Register<ToyboxParameter>(Lifetime.Singleton);
        }
    }
}