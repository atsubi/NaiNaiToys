using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using VContainer;
using VContainer.Unity;

using UniRx;

namespace Players {

    /// <summary>
    /// プレイヤーの動きを制御するクラス
    /// </summary>
    public class PlayerMover : MonoBehaviour
    {
    
        // 移動速度の初期値(/s)
        [SerializeField]
        private float _baseVelocity = 8.0f;

        // 現在の移動速度
        public IReadOnlyReactiveProperty<float> Velocity => _velocity;
        private FloatReactiveProperty _velocity = new FloatReactiveProperty(0.0f);

        // 移動可能フラグ
        public IReadOnlyReactiveProperty<bool> CanMove => _canMove;
        private BoolReactiveProperty _canMove = new BoolReactiveProperty(true);

        [Inject]
        public void Construct()
        {
            _velocity.Value = _baseVelocity;
        }

        /// <summary>
        /// プレイヤーを初期位置に移動させる
        /// </summary>
        public void InitializePlayerPosition()
        {
            transform.position = new Vector3(0.0f, 0.0f, 2.0f);
        }

        /// <summary>
        /// 引数で指定した量を移動する
        /// </summary>
        /// <param name="v"></param>
        public void UpdatePlayerPosition(Vector3 v)
        {
            transform.Translate(new Vector3( v.x * Time.deltaTime * _velocity.Value, v.y * Time.deltaTime * _velocity.Value, 0.0f));
        }

        /// <summary>
        /// 移動速度を、もっているおもちゃの重さに応じて減らす
        /// </summary>
        /// <param name="velocity"></param>
        public void ReducePlayerMoveVelocity(float weight)
        {
            this._velocity.Value = _baseVelocity - _baseVelocity * (weight/100.0f);
        }

        /// <summary>
        /// 移動フラグを更新する
        /// </summary>
        /// <param name="flag"></param>
        public void SetCanMoveFlag(bool flag)
        {
            _canMove.Value = flag;
        }

    }
}