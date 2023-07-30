using UnityEngine;

using VContainer;
using VContainer.Unity;

using Manager;

namespace TimeManager {
    public class TimeLifetimeScope : LifetimeScope
    {
        [SerializeField]
        public TimeViewer _timeViewer;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TimeParameter>(Lifetime.Singleton).WithParameter("timeValue", 200).WithParameter("readyTimeValue", 3);
            builder.RegisterEntryPoint<TimePresenter>(Lifetime.Singleton);
            builder.RegisterComponent(_timeViewer);
        }
    }
}