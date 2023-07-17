using UnityEngine;

using VContainer;
using VContainer.Unity;

using Manager;

namespace TimeManager {
    public class TimeLifetimeScope : LifetimeScope
    {
        [SerializeField]
        public TimeViewer _timeViewer;

        [SerializeField]
        private GameStatusManager _gameStatusManager;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TimeParameter>(Lifetime.Singleton).WithParameter<int>(200);
            builder.RegisterEntryPoint<TimePresenter>(Lifetime.Singleton);
            builder.RegisterComponent(_timeViewer);
            builder.RegisterComponent(_gameStatusManager);
        }
    }
}