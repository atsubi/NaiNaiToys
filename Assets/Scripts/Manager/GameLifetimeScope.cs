using UnityEngine;
using UnityEngine.Tilemaps;

using VContainer;
using VContainer.Unity;

using TimeManager;
using TilemapList;
using Anger;
using Players;
using Toybox;
using Toys;

namespace Manager {

    /// <summary>
    /// ゲーム全体のクラス間の依存性を解決するDIコンテナ
    /// </summary>
    public class GameLifetimeScope : LifetimeScope
    {
        ///////////
        
        // Anger
        [Header("怒りの上昇単位"), SerializeField]
        private float _addAngerValue = 0.02f;

        ///////////
        
        // Toybox
        [SerializeField]
        private ToyboxUIViewer _toyboxUIViewer;

        ///////////
        
        // Toys
        [Header("おもちゃオブジェクトのプレハブ"), SerializeField]
        private GameObject _toyPrefab;

        [Header("おもちゃオブジェクトの生成位置と他のおもちゃとの最低距離"), SerializeField]
        private float _checkRadius = 1.0f;

        [Header("おもちゃオブジェクトの生成間隔"), SerializeField]
        private int _createTimeSpan = 5;
        
        ///////////
        
        // Repository
        [Header("おもちゃデータのアセット"), SerializeField]
        private ToyParamAsset _toyParamAsset;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // GameStatus
            builder.Register<GameStatusManager>(Lifetime.Singleton);

            // TimeManager
            builder.Register<TimeParameter>(Lifetime.Singleton).WithParameter("timeValue", 200).WithParameter("readyTimeValue", 3);
            builder.RegisterEntryPoint<TimePresenter>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<TimeViewer>();

            // Anger
            builder.RegisterEntryPoint<AngerPresenter>(Lifetime.Singleton);
            builder.Register<AngerParameter>(Lifetime.Singleton).WithParameter("initAngerValue", 0.0f).WithParameter("initAddAngerValue", _addAngerValue);
            builder.RegisterComponentInHierarchy<AngerViewer>();

            // Player
            builder.RegisterComponentInHierarchy<DebugInputProvider>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<PlayerPresenter>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<PlayerToyHolder>();
            builder.RegisterComponentInHierarchy<PlayerMover>();
            builder.RegisterComponentInHierarchy<PlayerAnimation>();

            // Toybox
            builder.RegisterComponentInHierarchy<DebugAcceptToyProvider>().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<ToyboxUIViewer>();
            builder.RegisterEntryPoint<ToyboxPresenter>(Lifetime.Singleton);
            builder.Register<ToyboxParameter>(Lifetime.Singleton);

            // TileMapList
            builder.RegisterComponentInHierarchy<TilemapProvider>();

            // Toys
            builder.RegisterEntryPoint<ToyCreator>(Lifetime.Singleton).WithParameter("checkRadius", _checkRadius).WithParameter("createTimeSpan", _createTimeSpan);
            builder.Register<ToyParameter>(Lifetime.Transient);

            builder.RegisterFactory<int, Sprite, float, float, ToyPresenter>(container => 
            {
                System.Func<int, Sprite, float, float, ToyPresenter> func = (id, sprite, x, y) => {
                    GameObject toyObject = container.Instantiate(_toyPrefab, new Vector3(x, y, 2.0f), Quaternion.identity);
                    toyObject.GetComponent<SpriteRenderer>().sprite = sprite;

                    ToyParameter toyParameter = container.Resolve<ToyParameter>();
                    toyParameter.SetToyId(id);
                    toyObject.GetComponent<ToyIdGettter>().SetToyParamter(toyParameter);

                    return new ToyPresenter(toyObject, toyParameter);
                };
                return func;
            }, Lifetime.Scoped);

            // Repository
            builder.RegisterComponent(_toyParamAsset);
            builder.Register<ToyRepository>(Lifetime.Singleton);
        }
    }
}