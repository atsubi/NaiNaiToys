using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using UniRx;

using Cysharp.Threading.Tasks;

using VContainer;
using VContainer.Unity;
using UnityEngine;
using Hold;

using Manager;
using Toys;
using Anger;

namespace Toybox {

    public class ToyboxPresenter : IAsyncStartable, IDisposable
    {
        private readonly GameStatusManager _gameStatusManager;
        private readonly IAcceptToyProvider _iAcceptToyProvider;
        private readonly ToyboxUIViewer _toyboxUIViewer;
        private readonly ToyboxParameter _toyboxParameter;
        private readonly ToyRepository _toyRepository;
        private readonly AngerParameter _angerParameter;

        private  CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        public ToyboxPresenter(GameStatusManager gameStatusManager, IAcceptToyProvider iAcceptToyProvider, ToyboxParameter toyboxParameter, AngerParameter angerParameter, ToyRepository toyRepository, ToyboxUIViewer toyboxUIViewer)
        {
            _gameStatusManager  = gameStatusManager;
            _iAcceptToyProvider = iAcceptToyProvider;
            _toyboxParameter    = toyboxParameter;
            _angerParameter     = angerParameter;
            _toyRepository      = toyRepository;
            _toyboxUIViewer     = toyboxUIViewer;
            _iAcceptToyProvider.InitializeAcceptToyEvent();
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken token)
        {
            await _iAcceptToyProvider.CompleteAcceptToyEventSetting;

            // おもちゃが入れられたら、
            // 怒りゲージを減らし
            // おもちゃ箱に入っているおもちゃリストを更新し、おもちゃを削除する
            _iAcceptToyProvider.IAcceptToy
                .Where(collision => collision != null)
                .Do(collision => Debug.Log(collision.gameObject.name))
                .Where(collision => collision.gameObject.GetComponent<IHoldable>() != null)
                .Where(collision => collision.gameObject.GetComponent<ToyIdGettter>() != null)
                .Subscribe(collision => {
                    int id = collision.gameObject.GetComponent<ToyIdGettter>().ReferenceToyID().Value;
                    _angerParameter.ReduceAngerValue(_angerParameter.AngerValue.Value * _toyRepository.GetToyAngerCareRate(id) / 100);
                    _toyboxParameter.AddToy(id);
                    UnityEngine.Object.Destroy(collision.gameObject);
                })
                .AddTo(_disposable);

            // おもちゃ箱のおもちゃリストが更新されたら、スコアを更新する
            _toyboxParameter.ToyboxChangeEvent()
                .Subscribe(_ => {
                    _toyboxParameter.UpdateScore();
                })
                .AddTo(_disposable);

            // スコアが更新されたら、UIを更新する
            _toyboxParameter.Score
                .Subscribe(value => {
                    _toyboxUIViewer.SetScoreText(value);
                })
                .AddTo(_disposable);

            // スコアが100になったら、ゲームクリア
            _toyboxParameter.Score
                .Where(score => score == 100)
                .Subscribe(_ => {
                    _gameStatusManager.ChangeGameStatus(GameStatus.RESULT);
                    Debug.Log("CLEAR!");
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose() => _disposable.Dispose();
    }

}