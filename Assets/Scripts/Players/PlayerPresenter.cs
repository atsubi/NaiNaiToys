using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;

using UniRx;

using VContainer;
using VContainer.Unity;

namespace Players {

    /// <summary>
    /// プレイヤー制御フロークラス
    /// </summary>
    public class PlayerPresenter : IInitializable, IDisposable {

        private readonly IInputProvider _iInputProvider;
        private readonly PlayerMover _playerMover;

        private CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        public PlayerPresenter(IInputProvider iInputProvider, PlayerMover playerMover)
        {
            this._iInputProvider = iInputProvider;
            this._playerMover = playerMover;
        }

        /// <summary>
        /// ユーザーの入力に応じた制御フローを定義
        /// </summary>
        void IInitializable.Initialize()
        {
            _iInputProvider.IMoveDirection
                .Select(v => v.magnitude > 0.1f ? v.normalized : v) // プレイヤーの移動量は0.1～1ユニット
                .Subscribe( v => {
                    _playerMover.UpdatePlayerPosition(v);
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