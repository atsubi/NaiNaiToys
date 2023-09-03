using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Players {
    public class PlayerLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private PlayerMover _playerMover;

        [SerializeField]
        private PlayerToyHolder _playerToyHolder;

        [SerializeField]
        private PlayerAnimation _playerAnimation;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<DebugInputProvider>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<PlayerPresenter>(Lifetime.Singleton);
            builder.RegisterComponent(_playerToyHolder);
            builder.RegisterComponent(_playerMover);
            builder.RegisterComponent(_playerAnimation);
        }
    }
}