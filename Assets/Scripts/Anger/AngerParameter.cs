using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using VContainer;
using VContainer.Unity;

namespace Anger {
    public class AngerParameter
    {
        public IReadOnlyReactiveProperty<float> AngerValue => _angerValue;
        
        [Range(0, 100)]
        private ReactiveProperty<float> _angerValue;
        
        public IReadOnlyReactiveProperty<float> AddAngerValue => _addAngerValue;
        private ReactiveProperty<float> _addAngerValue;

        [Inject]
        public AngerParameter(float initAngerValue, float initAddAngerValue)
        {
            if (initAngerValue < 0 
                || initAngerValue > 100.0f) {
                throw new ArgumentException("initAngerValue is 0 or more and 100 or less.");
            }

            if (initAddAngerValue < 0 
                || initAddAngerValue > 10.0f) {
                throw new ArgumentException("initAddAngerValue is 0 or more and 10 or less.");
            }

            Debug.Log(initAngerValue);
            Debug.Log(initAddAngerValue);

            _angerValue = new ReactiveProperty<float>(initAngerValue);
            _addAngerValue = new ReactiveProperty<float>(initAddAngerValue);
        }

        /// <summary>
        /// 親の怒り値の上昇速度を更新する
        /// </summary>
        /// <param name="addValue"></param>
        public void UpdateAddAngerValue(float newAddAngerValue)
        {   
            _angerValue.Value = newAddAngerValue;
        }
        
        /// <summary>
        /// 親の怒り値をアップする
        /// </summary>
        /// <param name="addValue"></param>
        public void UpdateAngerValue()
        {
            if (_angerValue.Value + _addAngerValue.Value > 100.0f) {
                _angerValue.Value = 100.0f;
            } else {
                _angerValue.Value += _addAngerValue.Value;
            }
        }

    }
}
