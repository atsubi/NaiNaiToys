using UnityEngine;
using VContainer;
using VContainer.Unity;

using Manager;

namespace Anger {

    public class AngerLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private AngerViewer _angerViwer;

        [SerializeField]
        private GameStatusManager _gameStatusManager;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<AngerPresenter>();
            builder.Register<AngerParameter>(Lifetime.Singleton);
            builder.RegisterComponent(_angerViwer);
            builder.RegisterComponent(_gameStatusManager);
        }
    }
}