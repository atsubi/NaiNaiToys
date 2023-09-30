using UnityEngine;
using UnityEngine.Tilemaps;

using VContainer;
using VContainer.Unity;

namespace Toys {
    public class ToyCreatorLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private ToyParamAsset _toyParamAsset;

        [SerializeField]
        private GameObject _toyPrefab;

        [SerializeField]
        private Tilemap _floorTileMap;

        [Header("生成位置と他のおもちゃとの最低距離"), SerializeField]
        private float _checkRadius = 1.0f;

        [Header("生成間隔"), SerializeField]
        private int _createTimeSpan = 5;

        protected override void Configure(IContainerBuilder builder)
        {

            builder.RegisterEntryPoint<ToyCreator>(Lifetime.Singleton).WithParameter("checkRadius", _checkRadius).WithParameter("createTimeSpan", _createTimeSpan);
            builder.Register<ToyParameter>(Lifetime.Scoped);
            builder.Register<ToyRepository>(Lifetime.Singleton);

            builder.RegisterComponent(_toyParamAsset);
            builder.RegisterComponent(_floorTileMap);

            builder.RegisterFactory<int, float, float, ToyPresenter>(container => 
            {
                System.Func<int, float, float, ToyPresenter> func = (id, x, y) => {
                    GameObject toyObject = container.Instantiate(_toyPrefab, new Vector3(x, y, 2.0f), Quaternion.identity);

                    ToyParameter toyParameter = container.Resolve<ToyParameter>();
                    toyParameter.SetToyId(id);

                    return new ToyPresenter(toyObject, toyParameter);
                };
                return func;
            }, Lifetime.Scoped);

            Debug.Log("ToyCreateLifetimeScope");
        }
    }
}