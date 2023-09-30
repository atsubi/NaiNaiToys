using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using VContainer;
using VContainer.Unity;

using UniRx;

using TimeManager;
using Manager;

namespace Toys {

    /// <summary>
    /// おもちゃを生成するファクトリークラス
    /// </summary>
    public class ToyCreator : IStartable {

        System.Func<int, float, float, ToyPresenter> createToy;

        private readonly TimeParameter _timeParameter;

        private readonly ToyRepository _toyRepository;

        private readonly GameStatusManager _gameStatusManager;

        private readonly Tilemap _floorTileMap;

        private readonly float _checkRadius;

        /// <summary>
        /// おもちゃの生成間隔
        /// </summary>
        private readonly int _createTimeSpan;

        [Inject]
        public ToyCreator(System.Func<int, float, float, ToyPresenter> func,
            TimeParameter timeParameter,
            ToyRepository toyRepository,
            GameStatusManager gameStatusManager,
            Tilemap floorTileMap,
            float checkRadius,
            int createTimeSpan)
        {
            createToy = func;
            _timeParameter = timeParameter;
            _toyRepository = toyRepository;
            _gameStatusManager = gameStatusManager;
            _floorTileMap = floorTileMap;
            _checkRadius = checkRadius;
            _createTimeSpan = createTimeSpan;
        }

        private ToyPresenter Create(int id, float x, float y) => createToy(id, x, y);


        void IStartable.Start()
        {
            
            int lastCreateToyTime = _timeParameter.TimeValue.Value;

            Vector3 nextPosition = nextCreateToyPosition();

            // オブジェクト生成
            Observable
                .EveryUpdate()
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING)
                .Where(_ => lastCreateToyTime - _timeParameter.TimeValue.Value > _createTimeSpan)
                .Subscribe(_ => {
                    Debug.Log("Create:" + _toyRepository.GetToyName(_toyRepository.GetToyId()) + " x:" + nextPosition.x + " y:" + nextPosition.y + " z:" + nextPosition.z);
                    //Create(_toyRepository.GetToyId(), x, 0.0f);
                    //x += 0.2f;
                    lastCreateToyTime = _timeParameter.TimeValue.Value;
                    nextPosition = nextCreateToyPosition(); 
                });

        }


        private Vector3 nextCreateToyPosition()
        {
            int checkMask = 1 << LayerMask.NameToLayer("Toy") | 1 << LayerMask.NameToLayer("ToyBox");

            BoundsInt tileBounds = _floorTileMap.cellBounds;
            Vector3Int nextCellPosition = Vector3Int.zero;
            Vector3 nextWorldPosition = Vector3.zero;
            
            while(true) {

                // ランダムに抽出したセルの位置に床があるか確認                
                nextCellPosition = new Vector3Int(Random.Range(tileBounds.min.x, tileBounds.max.x + 1), 
                                            Random.Range(tileBounds.min.y, tileBounds.max.y + 1),
                                            0);
                if (!_floorTileMap.HasTile(nextCellPosition)) continue;

                // その床の位置周辺に他のおもちゃとおもちゃ箱がないか確認
                nextWorldPosition = _floorTileMap.GetCellCenterWorld(nextCellPosition);
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
    }
}