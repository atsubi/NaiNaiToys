using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using VContainer;
using VContainer.Unity;

using Manager;

namespace Anger {

    public class AngerPresenter : IStartable, ITickable, IDisposable
    {
        private readonly AngerViewer _angerViewer;
        private readonly AngerParameter _angerParameter;
        private readonly GameStatusManager _gameStatusManager;

        private  CompositeDisposable _disposable = new CompositeDisposable();
        
        [Inject]
        public AngerPresenter(AngerViewer angerViewer, AngerParameter angerParameter, GameStatusManager gameStatusManager)
        {
            _angerViewer = angerViewer;
            _angerParameter = angerParameter;
            _gameStatusManager = gameStatusManager;
        }


        void IStartable.Start()
        {
            // 怒りゲージが更新されたら、ゲージの表示を更新
            _angerParameter.AngerValue
                .Subscribe(angerValue => {
                    _angerViewer.UpdateSliderValue(angerValue);
                })
                .AddTo(_disposable);

            // ゲーム中は怒りゲージが上昇する
            Observable
                .EveryUpdate()
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING)
                .Subscribe(angerValue => {
                    _angerParameter.UpdateAngerValue();
                })
                .AddTo(_disposable);
            
            // 怒りゲージがMaxになるとゲームオーバー
            _angerParameter.AngerValue
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING)
                .Where(value => value == 100.0f)
                .Subscribe(value => {
                    UnityEngine.Debug.Log("GameOver!");
                    _gameStatusManager.ChangeGameStatus(GameStatus.RESULT);
                })
                .AddTo(_disposable);
        }

        void ITickable.Tick()
        {
            
        }

        void IDisposable.Dispose() => _disposable.Dispose();
    }
}