using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;
using System;

namespace Strength 
{
    public class StrengthParameter : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<float> StrengthValue => _strengthValue;
        private FloatReactiveProperty _strengthValue = new FloatReactiveProperty(100.0f);


        /// <summary>
        /// ストレングスゲージを回復させる 
        /// </summary>
        /// <returns></returns>
        public void RecoveryStrength() {

            _strengthValue.Value += 1.0f;

            if (_strengthValue.Value >= 100.0f) {
                _strengthValue.Value = 100.0f;
            }
            
        }
    }
}