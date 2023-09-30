using VContainer;
using VContainer.Unity;

using UnityEngine;

using Toys;

namespace Toybox {

    public class ToyboxLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private ToyParamAsset _toyParamAsset;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<DebugAcceptToyProvider>().AsImplementedInterfaces();
            builder.RegisterComponent(_toyParamAsset);
            builder.RegisterEntryPoint<ToyboxPresenter>(Lifetime.Singleton);
            builder.Register<ToyboxParameter>(Lifetime.Singleton);
            builder.Register<ToyRepository>(Lifetime.Singleton);
        }
    }
}