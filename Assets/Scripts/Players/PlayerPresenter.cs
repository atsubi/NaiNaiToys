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
    public class PlayerPresenter : IInitializable {

        private readonly IInputProvider _iInputProvider;
        private readonly PlayerMover _playerMover;

        [Inject]
        public PlayerPresenter(IInputProvider iInputProvider, PlayerMover playerMover)
        {
            this._iInputProvider = iInputProvider;
            this._playerMover = playerMover;
        }

        // Update is called once per frame
        void IInitializable.Initialize()
        {
            
            
            _iInputProvider.IMoveDirection
                .Select(v => v.magnitude > 0.1f ? v.normalized : v) // プレイヤーの移動量は0.1～1ユニット
                .Subscribe( v => {
                    v.z = 2.0f;
                    UnityEngine.Debug.Log("move" + v);
                    _playerMover.UpdatePlayerPosition(v);
                });

            
        }
    }
}