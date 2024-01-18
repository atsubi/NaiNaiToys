using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VContainer;
using VContainer.Unity;

using UniRx;
using System;

using Manager;
using Toybox;

using UnityEngine.SceneManagement;

namespace Result {

    public class ResultPresenter : IStartable, IDisposable
    {
        private readonly ResultViewer _resultViewer;
        private readonly ToyboxParameter _toyboxParameter;
        private readonly GameStatusManager _gameStatusManager;

        private CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        public ResultPresenter(ResultViewer resultViewer, GameStatusManager gameStatusManager, ToyboxParameter toyboxParameter)
        {
            _resultViewer = resultViewer;
            _gameStatusManager = gameStatusManager;
            _toyboxParameter   = toyboxParameter;
        }


        // Start is called before the first frame update
        void IStartable.Start()
        {
            // ゲームステータスがRESULTとなったら、ResultUIを表示する
            _gameStatusManager.IGameStatus
                .FirstOrDefault(status => status == GameStatus.RESULT)
                .Subscribe(_ => {
                    Debug.Log("RESULT");

                    _resultViewer.VisibleResultUI();
                    if (_toyboxParameter.IsCleard.Value == true) {
                        _resultViewer.VisibleResultClearText();
                    } else {
                        _resultViewer.VisibleResultGameOverText();
                    }

                })
                .AddTo(_disposable);

            // 「もういっかい」のボタンがおされたら、画面を再ロードする
            _resultViewer.RetryButtonAction
                .First()
                .Subscribe(_ => {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                })
                .AddTo(_disposable);

            // 「おしまいのボタン」のボタンがされたら、ゲームを終了する 
                   
            
        }

        // Update is called once per frame
        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}