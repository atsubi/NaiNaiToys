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
        private readonly StrengthParameter _strengthParameter;
        private readonly StrengthViewer _strengthViewer;
        private readonly PlayerMover _playerMover;
        private readonly PlayerToyHolder _playerToyHolder;

        private CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        public StrengthPresenter(StrengthViewer strengthViewer, StrengthParameter strengthParameter, PlayerMover playerMover, PlayerToyHolder playerToyHolder)
        {
            this._strengthViewer = strengthViewer;
            this._strengthParameter = strengthParameter;
            this._playerMover = playerMover;
            this._playerToyHolder = playerToyHolder;
        }


        void IStartable.Start()
        {
            
            // ストレングスゲージが0になったら、おもちゃを落として、満タンになるまで動けなくする
            this._strengthParameter.StrengthValue
                .Where(value => value == 0.0f)
                .Subscribe(_ => {
                    this._playerToyHolder.HoldAction(false);
                    this._playerMover.setCanMoveFlag(false);
                    this._strengthViewer.GrayUIPanel();
                })
                .AddTo(_disposable);

            
            // ストレングスゲージが100になったら、再び動けるようになる
            this._strengthParameter.StrengthValue
                .Where(value => value == 100.0f)
                .Subscribe(_ => {
                    this._playerMover.setCanMoveFlag(true);
                    this._strengthViewer.WhiteUIPanel();
                })
                .AddTo(_disposable);


            // おもちゃを持っていない且つ動ける時はストレングスゲージを急速回復させる
            Observable
                .EveryUpdate()
                .Where(_ => this._playerToyHolder.HoldToy.Value == false)
                .Where(_ => _playerMover.CanMove.Value == true)
                .Subscribe(_ => {
                    _strengthParameter.RecoveryStrengthCanMove();
                })
                .AddTo(_disposable);

            // おもちゃを持っていない且つ動けない時はストレングスゲージを緩やかに回復させる
            Observable
                .EveryUpdate()
                .Where(_ => this._playerToyHolder.HoldToy.Value == false)
                .Where(_ => _playerMover.CanMove.Value == false)
                .Subscribe(_ => {
                    _strengthParameter.RecoveryStrengthCannotMove();
                })
                .AddTo(_disposable);


            // おもちゃを持っている時はストレングスゲージを消費する
            Observable
                .EveryUpdate()
                .Where(_ => this._playerToyHolder.HoldToy.Value == true)
                .Subscribe(_ => {
                    Debug.Log("Weight = " + _playerToyHolder.HoldingToyWeight.Value);
                    _strengthParameter.ConsumeStrength(_playerToyHolder.HoldingToyWeight.Value, UnityEngine.Time.deltaTime);
                    
                })
                .AddTo(_disposable);


            // ストレグスゲージの値に応じて、表示も更新する
            _strengthParameter.StrengthValue
                .Subscribe(value => {
                    _strengthViewer.AdjustForgage(value);
                    _strengthViewer.UpdateForgageImageSprite(value);
                })
                .AddTo(_disposable);

        }

        void IDisposable.Dispose() => _disposable.Dispose();
    }

}