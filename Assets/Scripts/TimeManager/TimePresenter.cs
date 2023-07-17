using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using VContainer;
using VContainer.Unity;

using Manager;

namespace TimeManager {

    public class TimePresenter : IStartable, IDisposable
    {
        readonly TimeViewer _timeViwer;
        readonly TimeParameter _timeParameter;
        readonly GameStatusManager _gameStatusManager;

        private CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        public TimePresenter(TimeViewer timeViewer, TimeParameter timeParameter, GameStatusManager gameStatusManager) 
        {
            _timeViwer = timeViewer;
            _timeParameter = timeParameter;
            _gameStatusManager = gameStatusManager;
        }

        void IStartable.Start()
        {
            _timeParameter.TimeValue
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING)
                .Subscribe(timeValue => {
                    _timeViwer.SetTimeText(timeValue);
                })
                .AddTo(_disposable);

            Observable                
                .Interval(TimeSpan.FromSeconds(1))
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING)
                .Subscribe(_ => {
                    _timeParameter.UpdateTimeValue(_timeParameter.TimeValue.Value - 1);
                })
                .AddTo(_disposable);
                
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}