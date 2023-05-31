using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players {

    /// <summary>
    /// プレイヤーの動きを制御するクラス
    /// </summary>
    public class PlayerMover : MonoBehaviour
    {

        // 移動速度(/s)
        private float _velocity = 5.0f;

        /// <summary>
        /// 引数で指定した量を移動する
        /// </summary>
        /// <param name="v"></param>
        public void UpdatePlayerPosition(Vector3 v)
        {
            transform.Translate(v * Time.deltaTime * _velocity);
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