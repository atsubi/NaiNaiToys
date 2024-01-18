using System.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


using VContainer;
using VContainer.Unity;

using UniRx;

using Cysharp.Threading.Tasks;

using TimeManager;
using TilemapList;
using Manager;
using System;

namespace Toys {

    /// <summary>
    /// おもちゃを生成するファクトリークラス
    /// </summary>
    public class ToyCreator : IAsyncStartable, IDisposable {
        
        System.Func<int, Sprite, float, float, ToyPresenter> createToy;

        private readonly TimeParameter _timeParameter;

        private readonly ToyRepository _toyRepository;

        private readonly ToySpriteLoader _toySpriteLoader;

        private readonly GameStatusManager _gameStatusManager;

        private readonly TilemapProvider _tileMapProvider;

        private CompositeDisposable _disposable = new CompositeDisposable();

        /// <summary>
        /// 生成予定地に他のおもちゃの存在をチェックする半径
        /// </summary>
        private readonly float _checkRadius;

        /// <summary>
        /// おもちゃの生成間隔
        /// </summary>
        private readonly int _createTimeSpan;


        [Inject]
        public ToyCreator(System.Func<int, Sprite, float, float, ToyPresenter> func,
            TimeParameter timeParameter,
            ToyRepository toyRepository,
            ToySpriteLoader toySpriteLoader,
            GameStatusManager gameStatusManager,
            TilemapProvider tileMapProvider,
            float checkRadius,
            int createTimeSpan)
        {
            createToy = func;
            _timeParameter = timeParameter;
            _toyRepository = toyRepository;
            _toySpriteLoader = toySpriteLoader;
            _gameStatusManager = gameStatusManager;
            _tileMapProvider = tileMapProvider;
            _checkRadius = checkRadius;
            _createTimeSpan = createTimeSpan;
        }

        private ToyPresenter Create(int id, Sprite sprite, float x, float y) => createToy(id, sprite, x, y);


        async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
            // おもちゃのスプライトを読み込む
            _toySpriteLoader.LoadToySprite(cancellation);

            // スプライト読み込み完了まで待機
            await _toySpriteLoader.CompleteToySpriteLoading;
            
            int lastCreateToyTime = _timeParameter.TimeValue.Value;

            Vector3 nextPosition = nextCreateToyPosition();

            // _createTimeSpan毎におもちゃをランダムな位置に生成
            Observable
                .EveryUpdate()
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING)
                .Where(_ => lastCreateToyTime - _timeParameter.TimeValue.Value > _createTimeSpan)
                .Subscribe(_ => {
                    int id = _toyRepository.GetToyId();
                    Debug.Log("Create:" + id + " x:" + nextPosition.x + " y:" + nextPosition.y + " z:" + nextPosition.z);
                    Create(id, _toySpriteLoader.GetToySprite(id), nextPosition.x, nextPosition.y);
                    lastCreateToyTime = _timeParameter.TimeValue.Value;
                    nextPosition = nextCreateToyPosition(); 
                })
                .AddTo(_disposable);
                

        }


        /// <summary>
        /// 次におもちゃを生成する位置を設定する
        /// </summary>
        /// <returns></returns>
        private Vector3 nextCreateToyPosition()
        {
            int checkMask = 1 << LayerMask.NameToLayer("Toy") | 1 << LayerMask.NameToLayer("ToyBox");

            Tilemap floorTileMap = _tileMapProvider.FloorTileMap;
            Tilemap wallTileMap = _tileMapProvider.WallTileMap;
            Tilemap itemTileMap = _tileMapProvider.ItemTileMap;

            BoundsInt floorTileBounds = floorTileMap.cellBounds;
            Vector3Int nextCellPosition = Vector3Int.zero;
            Vector3 nextWorldPosition = Vector3.zero;
            
            while(true) {

                // ランダムに抽出したセルの位置に床があるか確認         
                nextCellPosition = new Vector3Int(UnityEngine.Random.Range(floorTileBounds.min.x, floorTileBounds.max.x + 1), 
                                            UnityEngine.Random.Range(floorTileBounds.min.y, floorTileBounds.max.y + 1),
                                            0);
                if (!floorTileMap.HasTile(nextCellPosition)) continue;

                // 抽出した位置にアイテムのタイル、壁のタイルが無いか確認
                nextWorldPosition = floorTileMap.GetCellCenterWorld(nextCellPosition);
                
                Vector3Int nextWallCellPosition = wallTileMap.WorldToCell(new Vector3(nextWorldPosition.x,
                                                                                nextWorldPosition.y,
                                                                                wallTileMap.transform.position.z));
                if (wallTileMap.HasTile(nextWallCellPosition)) continue;

                Vector3Int nextItemCellPosition = itemTileMap.WorldToCell(new Vector3(nextWorldPosition.x,
                                                                                nextWorldPosition.y,
                                                                                itemTileMap.transform.position.z));
                if (itemTileMap.HasTile(nextItemCellPosition)) continue;

                // その床の位置周辺に他のおもちゃとおもちゃ箱がないか確認
                nextWorldPosition = floorTileMap.GetCellCenterWorld(nextCellPosition);
                nextWorldPosition.z = 2.0f;
                List<Collider2D> findObjects = Physics2D.OverlapCircleAll(nextWorldPosition, _checkRadius, checkMask).ToList();
                if (findObjects.Count == 0) {
                    break;
                } else {
                    continue;
                }
            }                
            
            return nextWorldPosition;
        }

        void IDisposable.Dispose() => _disposable.Dispose();
    }
}