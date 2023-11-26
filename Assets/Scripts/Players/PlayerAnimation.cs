using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace Players {
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField]
        private float _baseAnimationSpeed = 2.0f;

        [SerializeField]
        private Animator _animator;

        /// <summary>
        /// 現在のプレイヤーの方向
        /// </summary> 
        public float DirectionX => _animator.GetFloat("x");
        public float DirectionY => _animator.GetFloat("y");

        private float _before_x = 0.0f;
        private float _before_y = 0.0f;

        [Inject]
        public void Construct()
        {
            _animator.SetFloat("x", 0.0f);
            _animator.SetFloat("y", -1.0f);
            _animator.SetFloat("animationSpeed", _baseAnimationSpeed);
        }

        /// <summary>
        /// アニメーションのパラメータを更新する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UpdateAnimatorParam(float x, float y)
        {
            if (x != 0.0f || y != 0.0f) {
                _animator.SetBool("Input", true);
                _animator.SetFloat("x", x);
                _animator.SetFloat("y", y);
                _before_x = x;
                _before_y = y;
            } else {
                _animator.SetBool("Input", false);
                _animator.SetFloat("before_x", _before_x);
                _animator.SetFloat("before_y", _before_y);
            }

        }


        /// <summary>
        /// アニメーションの速度を、もっているおもちゃの重さに応じて減らす
        /// </summary>
        /// <param name="velocity"></param>
        public void UpdateAnimationSpeed(float weight)
        {
            float animationSpeed = _baseAnimationSpeed - _baseAnimationSpeed * (weight/100.0f);
            _animator.SetFloat("animationSpeed", animationSpeed);
        }
    }
}