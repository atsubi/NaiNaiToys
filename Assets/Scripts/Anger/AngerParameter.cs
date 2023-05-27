using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Anger {
    public class AngerParameter
    {
        [Range(0, 100)]
        private ReactiveProperty<float> _angerValue = new ReactiveProperty<float>(0.0f);

        public IReadOnlyReactiveProperty<float> AngerValue => _angerValue;

        /// <summary>
        /// 親の怒り値をアップする
        /// </summary>
        /// <param name="addValue"></param>
        public void AddAngerValue(float addValue)
        {   
            _angerValue.Value += addValue;
        }
    }
}
