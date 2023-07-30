using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;

using UniRx;

using VContainer;
using VContainer.Unity;

using Manager;

namespace Players {

    /// <summary>
    /// プレイヤー制御フロークラス
    /// </summary>
    public class PlayerPresenter : IInitializable, IDisposable {

        private readonly IInputProvider _iInputProvider;
        private readonly PlayerMover _playerMover;
        private readonly GameStatusManager _gameStatusManager;

        private CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        public PlayerPresenter(IInputProvider iInputProvider, PlayerMover playerMover, GameStatusManager gameStatusManager)
        {
            this._iInputProvider = iInputProvider;
            this._playerMover = playerMover;
            this._gameStatusManager = gameStatusManager; 
        }

        /// <summary>
        /// ユーザーの入力に応じた制御フローを定義
        /// </summary>
        void IInitializable.Initialize()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            // 入力イベント設定
            _iInputProvider.InitializeInputEvent(token);

            // 移動処理
            _iInputProvider.IMoveDirection
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING) // 掃除中のみ入力受付
                .Select(v => v.magnitude > 0.1f ? v.normalized : v) // プレイヤーの移動量は0.1～1ユニット
                .Subscribe( v => {
                    _playerMover.UpdatePlayerPosition(v);
                })
                .AddTo(_disposable);

            // つかみ処理
            
        }

        /// <summary>
        /// オブジェクト破棄時にストリームを全てDisposeする
        /// </summary>
        /// <returns></returns>
        void IDisposable.Dispose() => _disposable.Dispose();

    }
}