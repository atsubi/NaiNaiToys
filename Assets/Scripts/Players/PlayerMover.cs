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
        public IReadOnlyReactiveProperty<bool> CanMove;
        private BoolReactiveProperty _canMove = new BoolReactiveProperty(true);


        void Start()
        {
            _velocity.Value = _baseVelocity;
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


        public void setCanMoveFlag(bool flag)
        {
            _canMove.Value = flag;
        }

    }
}