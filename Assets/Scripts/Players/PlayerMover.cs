using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace Players {

    /// <summary>
    /// プレイヤーの動きを制御するクラス
    /// </summary>
    public class PlayerMover : MonoBehaviour
    {
        // 移動速度(/s)
        [SerializeField]
        private float _velocity = 5.0f;

        /// <summary>
        /// 引数で指定した量を移動する
        /// </summary>
        /// <param name="v"></param>
        public void UpdatePlayerPosition(Vector3 v)
        {
            transform.Translate(new Vector3( v.x * Time.deltaTime * _velocity, v.y * Time.deltaTime * _velocity, 0.0f));
        }

        /// <summary>
        /// 移動速度を更新
        /// </summary>
        /// <param name="velocity"></param>
        public void UpdatePlayerMoveVelocity(float velocity)
        {
            this._velocity = velocity;
        }

    }
}