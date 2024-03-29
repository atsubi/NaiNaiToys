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
using Strength;
using Result;
using UnityEditor.AddressableAssets.Build.Layout;
using Unity.VisualScripting;

namespace Manager {

    /// <summary>
    /// ゲーム全体のクラス間の依存性を解決するDIコンテナ
    /// </summary>
    public class GameLifetimeScope : LifetimeScope
    {

        ///////////
        
        // Time
        [Header("1ゲームの時間"), SerializeField]
        private int _timeValue = 200;

        ///////////
        
        ///////////
        
        // Anger
        [Header("怒りの上昇単位"), SerializeField]
        private float _addAngerValue = 0.02f;

        ///////////
        
        // Toybox
        [SerializeField]
        private ToyboxUIViewer _toyboxUIViewer;

        [Header("クリアに必要なスコア"), SerializeField]
        private float _cleardScore = 100;


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
            builder.Register<TimeParameter>(Lifetime.Singleton).WithParameter("timeValue", _timeValue).WithParameter("readyTimeValue", 3);
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
            builder.Register<ToyboxParameter>(Lifetime.Singleton).WithParameter("cleardScore", _cleardScore);

            // TileMapList
            builder.RegisterComponentInHierarchy<TilemapProvider>();

            // Toys
            builder.RegisterEntryPoint<ToyCreator>(Lifetime.Singleton).WithParameter("checkRadius", _checkRadius).WithParameter("createTimeSpan", _createTimeSpan);
            builder.Register<ToyParameter>(Lifetime.Transient);
            builder.Register<ToySpriteLoader>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<ToyPointListViewer>();

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

            // Strength
            builder.RegisterComponentInHierarchy<StrengthViewer>();
            builder.RegisterEntryPoint<StrengthPresenter>(Lifetime.Singleton);
            builder.Register<StrengthParameter>(Lifetime.Singleton);

            // Repository
            builder.RegisterComponent(_toyParamAsset);
            builder.Register<ToyRepository>(Lifetime.Singleton);

            // Result
            builder.RegisterComponentInHierarchy<ResultViewer>();
            builder.RegisterEntryPoint<ResultPresenter>(Lifetime.Singleton);
        }
    }
}