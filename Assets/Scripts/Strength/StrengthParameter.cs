using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;
using System;

namespace Strength 
{
    public class StrengthParameter
    {
        public IReadOnlyReactiveProperty<float> StrengthValue => _strengthValue;
        private FloatReactiveProperty _strengthValue = new FloatReactiveProperty(100.0f);


        /// <summary>
        /// 持っているおもちゃの重さに応じて、ストレングスゲージを減らす
        /// </summary>
        /// <param name="weight"></param>
        public void ConsumeStrength(float weight, float delta)
        {
            if (weight <= 3.0f) return;

            if (weight >= 100.0f) {
                _strengthValue.Value = 0.0f;
            }

            float consume = ( Mathf.Pow((weight - 3.0f), 2) / Mathf.Pow(weight, 2) ) * 30.0f * delta;
            _strengthValue.Value -= consume;

            if (_strengthValue.Value < 0.0f) {
                _strengthValue.Value = 0.0f;
            }

        }

        /// <summary>
        /// ストレングスゲージをリセットする
        /// </summary>
        public void ResetStrength()
        {
            _strengthValue.Value = 100.0f;
        }


        /// <summary>
        /// ストレングスゲージを回復させる(動ける時)
        /// </summary>
        /// <returns></returns>
        public void RecoveryStrengthCanMove() {
            _strengthValue.Value += 1.0f;

            if (_strengthValue.Value >= 100.0f) {
                _strengthValue.Value = 100.0f;
            }
        }

        /// <summary>
        /// ストレングスゲージを回復させる(動けない時)
        /// </summary>
        /// <returns></returns>
        public void RecoveryStrengthCannotMove() {
            _strengthValue.Value += 0.3f;

            if (_strengthValue.Value >= 100.0f) {
                _strengthValue.Value = 100.0f;
            }
        }
    }
}