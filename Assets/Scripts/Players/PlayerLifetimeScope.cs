using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Players {
    public class PlayerLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private PlayerMover _playerMover;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<DebugInputProvider>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<PlayerPresenter>(Lifetime.Singleton);
            builder.RegisterComponent(_playerMover);
        }
    }
}