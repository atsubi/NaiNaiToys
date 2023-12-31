using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using UniRx;

using VContainer;
using VContainer.Unity;

using Manager;

namespace Players {

    /// <summary>
    /// プレイヤー制御フロークラス
    /// </summary>
    public class PlayerPresenter : IAsyncStartable, IDisposable {

        private readonly IInputProvider _iInputProvider;
        private readonly PlayerMover _playerMover;
        private readonly PlayerToyHolder _playerToyHolder;
        private readonly PlayerAnimation _playerAnimation;
        private readonly GameStatusManager _gameStatusManager;

        private CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        public PlayerPresenter(IInputProvider iInputProvider, PlayerMover playerMover, PlayerToyHolder playerToyHolder, PlayerAnimation playerAnimation, GameStatusManager gameStatusManager)
        {
            this._iInputProvider = iInputProvider;
            this._playerMover = playerMover;
            this._playerToyHolder = playerToyHolder;
            this._playerAnimation = playerAnimation;
            this._gameStatusManager = gameStatusManager; 

            _iInputProvider.InitializeInputEvent();
        }

        /// <summary>
        /// ユーザーの入力に応じた制御フローを定義
        /// </summary>
        async UniTask IAsyncStartable.StartAsync(CancellationToken token)
        {
            // 入力イベント設定
            await _iInputProvider.CompleteInputEventSetting;

            // 移動処理
            _iInputProvider.IMoveDirection
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING) // 掃除中のみ入力受付
                .Where(_ => _playerMover.CanMove.Value == true) // プレイヤーは移動可能か確認
                .Select(v => v.magnitude > 0.1f ? v.normalized : v) // プレイヤーの移動量は0.1～1ユニット
                .Subscribe( v => {
                    _playerMover.UpdatePlayerPosition(v);
                    _playerAnimation.UpdateAnimatorParam(v.x, v.y);
                })
                .AddTo(_disposable);

            
            // プレイヤーが移動できない場合はアニメーションをストップ
            _playerMover.CanMove
                .Where(value => value == false)
                .Subscribe(_ => {
                    _playerAnimation.UpdateAnimatorParam(0.0f, 0.0f);
                })
                .AddTo(_disposable);
            
            // ゲームオーバー時は歩行アニメーションをストップ
            _gameStatusManager.IGameStatus
                .Where(status => status == GameStatus.RESULT)
                .Subscribe(_ => {
                    _playerAnimation.UpdateAnimatorParam(0.0f, 0.0f);
                })
                .AddTo(_disposable);
            
            // つかみ処理
            _iInputProvider.IHoldAction
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING) // 掃除中のみ入力受付
                .Where(_ => _playerMover.CanMove.Value == true) // 　プレイヤーは移動可能か確認
                .Subscribe( v => {
                    _playerToyHolder.HoldAction(v);
                })
                .AddTo(_disposable);

            // 掴んでいるおもちゃの重さに応じて移動速度とアニメーション速度を変更
            _playerToyHolder.HoldingToyWeight
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING) // 掃除中のみ入力受付
                .Subscribe( weight => {
                    _playerMover.ReducePlayerMoveVelocity(weight);
                    _playerAnimation.UpdateAnimationSpeed(weight);
                })
                .AddTo(_disposable);

            
        }

        /// <summary>
        /// オブジェクト破棄時にストリームを全てDisposeする
        /// </summary>
        /// <returns></returns>
        void IDisposable.Dispose() => _disposable.Dispose();

    }
}