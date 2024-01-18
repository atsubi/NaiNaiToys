using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using VContainer;
using VContainer.Unity;

namespace Anger {
    public class AngerParameter : IDisposable
    {
        public IReadOnlyReactiveProperty<float> AngerValue => _angerValue;
        
        [Range(0, 100)]
        private ReactiveProperty<float> _angerValue;
        
        public IReadOnlyReactiveProperty<float> AddAngerValue => _addAngerValue;
        private ReactiveProperty<float> _addAngerValue;

        private  CompositeDisposable _disposable = new CompositeDisposable();


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
            _angerValue = new ReactiveProperty<float>(initAngerValue);
            _addAngerValue = new ReactiveProperty<float>(initAddAngerValue);

            _angerValue.AddTo(_disposable);
            _addAngerValue.AddTo(_disposable);
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
        /// 親の怒り値を鎮める
        /// </summary>
        /// <param name="reduceValue"></param>
        public void ReduceAngerValue(float reduceValue)
        {
            if (_angerValue.Value - reduceValue < 0.0f) {
                _angerValue.Value = 0.0f;
            } else {
                _angerValue.Value -= reduceValue;
            }
        }


        /// <summary>
        /// 親の怒り値を加算する
        /// </summary>
        /// <param name="addValue"></param>
        public void IncrementAngerValue()
        {
            if (_angerValue.Value + _addAngerValue.Value > 100.0f) {
                _angerValue.Value = 100.0f;
            } else {
                _angerValue.Value += _addAngerValue.Value;
            }
        }


        void IDisposable.Dispose() => _disposable.Dispose();

    }
}
