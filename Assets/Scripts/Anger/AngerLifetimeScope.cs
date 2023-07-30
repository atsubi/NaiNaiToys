using UnityEngine;
using VContainer;
using VContainer.Unity;

using Manager;

namespace Anger {

    public class AngerLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private AngerViewer _angerViwer;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<AngerPresenter>(Lifetime.Singleton);
            builder.Register<AngerParameter>(Lifetime.Singleton).WithParameter("initAngerValue", 0.0f).WithParameter("initAddAngerValue", 0.2f);
            builder.RegisterComponent(_angerViwer);
        }
    }
}