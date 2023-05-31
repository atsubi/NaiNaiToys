using System.Collections;
using System.Collections.Generic;
using UniRx;
using VContainer;
using VContainer.Unity;

using Manager;

namespace Anger {

    public class AngerPresenter : IStartable, ITickable
    {
        private readonly AngerViewer _angerViewer;
        private readonly AngerParameter _angerParameter;
        private readonly GameStatusManager _gameStatusManager;
        
        [Inject]
        public AngerPresenter(AngerViewer angerViewer, AngerParameter angerParameter, GameStatusManager gameStatusManager)
        {
            _angerViewer = angerViewer;
            _angerParameter = angerParameter;
            _gameStatusManager = gameStatusManager;
        }


        void IStartable.Start()
        {
            _angerParameter.AngerValue
                .Subscribe(angerValue => {
                    _angerViewer.UpdateSliderValue(angerValue);
                })
                .AddTo(_angerViewer.gameObject);
            
            _angerParameter.AngerValue
                .Where(_ => _gameStatusManager.IGameStatus.Value == GameStatus.CLEANING)
                .Where(value => value >= 100.0f)
                .Subscribe(value => {
                    UnityEngine.Debug.Log("GameOver!");
                });
        }


        void ITickable.Tick()
        {
            
            _angerParameter.AddAngerValue(0.05f);
        }
    }
}