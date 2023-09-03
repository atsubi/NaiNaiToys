using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace Toys {
    public class ToyCreaterLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private readonly GameObject toyPrefab;

        protected override void Configure(IContainerBuilder builder)
        {

            builder.Register<ToyCreater>(Lifetime.Singleton);
            builder.RegisterFactory<int, UnityEngine.GameObject>(container => 
            {
                System.Func<int, UnityEngine.GameObject> func = (id) => {
                    GameObject toyObject = container.Instantiate(toyPrefab);

                    ToyVisual toyVisual = toyObject.GetComponent<ToyVisual>();
                    toyVisual.InitializeToyVisual(id);

                    return toyObject;
                };
                return func;
            }, Lifetime.Scoped);
        }
    }
}