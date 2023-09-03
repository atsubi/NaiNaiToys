using Toys;
using UnityEngine;
using VContainer;
using VContainer.Unity;

using Hold;

namespace Toys {
    public class ToyLifetimeScope : LifetimeScope
    {   
        [SerializeField]
        private ToyVisual _toyVisual;

        [SerializeField]
        private ToyHoldable _toyHoldable;

        [SerializeField]
        private ToyParamAsset _toyParamAsset;


        /// <summary>
        /// おもちゃ単体のDIコンテナ
        /// </summary>
        /// <param name="builder"></param>
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<Holdable>(Lifetime.Singleton);
            builder.RegisterComponent(_toyParamAsset);
            builder.RegisterComponent(_toyVisual);
            builder.RegisterComponent(_toyHoldable);
            builder.RegisterEntryPoint<ToyPresenter>(Lifetime.Singleton);
        }   
    }
}