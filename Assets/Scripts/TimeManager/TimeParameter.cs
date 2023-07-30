using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using Cysharp.Threading.Tasks;

using VContainer;
using VContainer.Unity;

namespace TimeManager {

    /// <summary>
    /// 時間管理クラス
    /// </summary>
    public class TimeParameter
    {
        IntReactiveProperty _timeValue;
        public IReadOnlyReactiveProperty<int> TimeValue => _timeValue;

        IntReactiveProperty _readyTimeValue;
        public IReadOnlyReactiveProperty<int> ReadyTimeValue => _readyTimeValue;

        private int _initTimeValue;
        private int _initReadyTimeValue;

        [Inject]
        public TimeParameter(int timeValue, int readyTimeValue)
        {
            if (timeValue <= 0) {
                throw new System.ArgumentException("timeValue is 1 or more.");
            }
            if (readyTimeValue <= 0) {
                throw new System.ArgumentException("readyTimeValue is 1 or more.");
            }

            _timeValue = new IntReactiveProperty(timeValue);
            _readyTimeValue = new IntReactiveProperty(readyTimeValue);

            _initTimeValue = timeValue;
            _initReadyTimeValue = readyTimeValue;
        }
        
        /// <summary>
        /// 準備カウントダウンを非同期で開始
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTaskVoid PlayReadyTimeCountDownAsync(CancellationToken token) 
        {
            await UniTask.Delay(500, cancellationToken: token);
            _readyTimeValue.SetValueAndForceNotify(_readyTimeValue.Value);

            while (!token.IsCancellationRequested) {
                await UniTask.Delay(1000, cancellationToken: token);
                _readyTimeValue.Value--;
                if (_readyTimeValue.Value == 0) return;
            }
        }


        /// <summary>
        /// カウントダウンを非同期で開始
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTaskVoid PlayTimeCountDownAsync(CancellationToken token) 
        {
            await UniTask.Delay(500, cancellationToken: token);
            _readyTimeValue.SetValueAndForceNotify(_timeValue.Value);

            while (!token.IsCancellationRequested) {
                
                await UniTask.Delay(1000, cancellationToken: token);
                _timeValue.Value--;
                if (_timeValue.Value == 0) return;
            }
        }

        /// <summary>
        /// カウントダウンのデータをリセットする
        /// </summary>
        public void ResetTimeValue()
        {
            _timeValue.Value = _initTimeValue;
            _readyTimeValue.Value = _initReadyTimeValue;
        }


        public void UpdateTimeValue(int timeValue)
        {
            _timeValue.Value = timeValue;
        }
    }
}