using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using VContainer;
using VContainer.Unity;
using System;
using Players;

namespace Strength {

    public class StrengthPresenter : IStartable, IDisposable
    {
        private StrengthParameter _strengthParameter;
        private StrengthViewer _strengthViewer;
        private PlayerMover _playerMover;

        private CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        public StrengthPresenter(StrengthViewer strengthViewer, StrengthParameter strengthParameter, PlayerMover playerMover)
        {
            _strengthViewer = strengthViewer;
            _strengthParameter = strengthParameter;
            _playerMover = playerMover;
        }


        void IStartable.Start()
        {
            
            // ストレングスゲージが0になったら、満タンになるまで動けなくする
            _strengthParameter.StrengthValue
                .Where(value => value == 0.0f)
                .Subscribe(_ => {
                    _playerMover.setCanMoveFlag(false);
                });

            // 動けない間はストレングスゲージを回復させる
            Observable
                .EveryUpdate()
                .Where(_ => _playerMover.CanMove.Value == false)
                .Subscribe(_ => {

                });

            // おもちゃを持っている時はストレングスゲージを消費する
            

        }

        void IDisposable.Dispose() => _disposable.Dispose();
    }

}