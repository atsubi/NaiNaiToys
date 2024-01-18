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
        
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        [Inject]
        public TimePresenter(TimeViewer timeViewer, TimeParameter timeParameter, GameStatusManager gameStatusManager) 
        {
            _timeViwer = timeViewer;
            _timeParameter = timeParameter;
            _gameStatusManager = gameStatusManager;
        }

        void IStartable.Start()
        {
            CancellationToken token = _tokenSource.Token;

            // ゲームステータスがReadyとなったら、ReadyTimeValueのカウントダウン開始
            _gameStatusManager.IGameStatus
                .FirstOrDefault(status => status == GameStatus.READYCLEANING)
                .Subscribe(_ => {
                    _timeViwer.ClearReadyTimeText();
                    _timeViwer.VisibleReadyTimeText(true);
                    _timeParameter.PlayReadyTimeCountDownAsync(token).Forget();
                })
                .AddTo(_disposable);

            // ReadyTimeValueの変化に伴って表示するテキスト情報を更新
            _timeParameter.ReadyTimeValue
                .Subscribe(value => {
                    _timeViwer.SetReadyTimeText(value);
                })
                .AddTo(_disposable);

            // ReadyTimeValueが0となったら、ゲームステータスをCLEANINGに変更
            _timeParameter.ReadyTimeValue
                .FirstOrDefault(value => value == 0)
                .Subscribe(_ => {
                    _gameStatusManager.ChangeGameStatus(GameStatus.CLEANING);
                    _timeViwer.VisibleReadyTimeText(false);
                })
                .AddTo(_disposable);

            // ゲームステータスがCLEANINGとなったら、TimeValueのカウントダウン開始
            _gameStatusManager.IGameStatus
                .FirstOrDefault(status => status == GameStatus.CLEANING)
                .Subscribe(_ => {
                    _timeParameter.PlayTimeCountDownAsync(token).Forget();
                })
                .AddTo(_disposable);
            
            // TimeValueの変化に伴って表示するテキスト情報を更新
            _timeParameter.TimeValue                
                .Subscribe(value => {
                    _timeViwer.SetTimeText(value);
                })
                .AddTo(_disposable);

            // TimeValue0になったら、ゲームステータスをRESULTに変更
            _timeParameter.TimeValue
                .FirstOrDefault(value => value == 0)
                .Subscribe(timeValue => {
                    _gameStatusManager.ChangeGameStatus(GameStatus.RESULT);
                })
                .AddTo(_disposable);

            // RESULTになったらキャンセルのシグナルを送信する
            _gameStatusManager.IGameStatus
                .FirstOrDefault(status => status == GameStatus.RESULT)
                .Subscribe(_ => {
                    _tokenSource.Cancel();
                })
                .AddTo(_disposable);                
        }

        void IDisposable.Dispose()
        {            
            _disposable.Dispose();
        }
    }
}