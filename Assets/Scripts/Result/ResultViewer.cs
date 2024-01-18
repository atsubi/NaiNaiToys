using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

using VContainer;
using VContainer.Unity;

namespace Result {

    public class ResultViewer : MonoBehaviour
    {
        // リサルト画面のルートオブジェクト
        [SerializeField]
        private GameObject _resultUIPanel;

        // リサルト画面テキストオブジェクト
        [SerializeField]
        private GameObject _resultText;

        // ゲームクリアテキストオブジェクト
        [SerializeField]
        private GameObject _resultClearText;

        // ゲームオーバーテキストオブジェクト
        [SerializeField]
        private GameObject _resultGameOverText;

        // もう一回のボタン
        [SerializeField]
        private Button _retryButton;

        // おしまいのボタン
        [SerializeField]
        private Button _quitButton;

        // リトライボタンクリック時のアクション通知
        public IObservable<Unit> RetryButtonAction => _retryButtonAction;
        private Subject<Unit> _retryButtonAction = new Subject<Unit>();

        // リトライボタンクリック時のアクション通知
        public IObservable<Unit> QuitButtonAction => _quitButtonAction;
        private Subject<Unit> _quitButtonAction = new Subject<Unit>();

        // UI表示フラグ
        private bool _isVisible = false;

        /// <summary>
        /// リサルト画面を表示
        /// </summary>
        public void VisibleResultUI()
        {
            if (_isVisible) return;

            _resultUIPanel.SetActive(true);

            _isVisible = true;
        }

        /// <summary>
        /// リサルト画面を非表示
        /// </summary>
        public void UnVisibleResultUI()
        {
            if (!_isVisible) return;

            _resultClearText.SetActive(false);
            _resultGameOverText.SetActive(false);

            _resultText.SetActive(false);

            _resultUIPanel.SetActive(false);
            _isVisible = false;
        }


        /// <summary>
        /// ゲームクリアテキストを表示
        /// </summary>
        public void VisibleResultClearText()
        {
            _resultText.SetActive(true);
            _resultClearText.SetActive(true);
        }


        /// <summary>
        /// ゲームオーバーのテキストを表示
        /// </summary>
        public void VisibleResultGameOverText()
        {
            _resultText.SetActive(true);
            _resultGameOverText.SetActive(true);
        }


        // Start is called before the first frame update
        void Start()
        {
            // 「もういっかい」ボタンクリック時のアクション通知
            _retryButton.OnClickAsObservable()
                .Where(_ => _isVisible == true)
                .Subscribe(_ => {
                    _retryButtonAction.OnNext(Unit.Default);
                })
                .AddTo(this.gameObject);

            // 「おしまい」ボタンクリック時のアクション通知
            _quitButton.OnClickAsObservable()
                .Where(_ => _isVisible == true)
                .Subscribe(_ => {
                    _quitButtonAction.OnNext(Unit.Default);
                })
                .AddTo(this.gameObject);
　        }
    }
}