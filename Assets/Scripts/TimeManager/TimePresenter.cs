using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using VContainer;
using VContainer.Unity;

using Cysharp.Threading.Tasks;

using Manager;

namespace TimeManager {

    public class TimePresenter : IStartable, IDisposable
    {
        readonly TimeViewer _timeViwer;
        readonly TimeParameter _timeParameter;
        readonly GameStatusManager _gameStatusManager;

        private CompositeDisposable _disposable = new CompositeDisposable();
        
        private CancellationToken token = new CancellationTokenSource().Token;

        [Inject]
        public TimePresenter(TimeViewer timeViewer, TimeParameter timeParameter, GameStatusManager gameStatusManager) 
        {
            _timeViwer = timeViewer;
            _timeParameter = timeParameter;
            _gameStatusManager = gameStatusManager;
        }

        void IStartable.Start()
        {
            // ゲームステータスがReadyとなったら、ReadyTimeValueのカウントダウン開始
            _gameStatusManager.IGameStatus
                .Where(status => status == GameStatus.READYCLEANING)
                .Subscribe(_ => {
                    _timeViwer.ClearReadyTimeText();
                    _timeViwer.VisibleReadyTimeText(true);
                    _timeParameter.PlayReadyTimeCountDownAsync(token).Forget();
                })
                .AddTo(_disposable);

            // ReadTimeValueの変化に伴って表示するテキスト情報を更新
            _timeParameter.ReadyTimeValue
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.READYCLEANING)
                .Subscribe(value => {
                    _timeViwer.SetReadyTimeText(value);
                })
                .AddTo(_disposable);

            // ReadTimeValueが0となったら、ゲームステータスをCLEANINGに変更
            _timeParameter.ReadyTimeValue
                .Where(value => value == 0)
                .Subscribe(_ => {
                    _gameStatusManager.ChangeGameStatus(GameStatus.CLEANING);
                    _timeViwer.VisibleReadyTimeText(false);
                })
                .AddTo(_disposable);

            // ゲームステータスがCLEANINGとなったら、TimeValueのカウントダウン開始
            _gameStatusManager.IGameStatus
                .Where(status => status == GameStatus.CLEANING)
                .Subscribe(_ => {
                    _timeParameter.PlayTimeCountDownAsync(token).Forget();
                })
                .AddTo(_disposable);
            
            // TimeValueの変化に伴って表示するテキスト情報を更新
            _timeParameter.TimeValue
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING)
                .Subscribe(value => {
                    _timeViwer.SetTimeText(value);
                })
                .AddTo(_disposable);

            // TimeValue0になったら、ゲームステータスをRESULTに変更
            _timeParameter.TimeValue
                .Where(value => value == 0)
                .Subscribe(timeValue => {
                    _gameStatusManager.ChangeGameStatus(GameStatus.RESULT);
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}