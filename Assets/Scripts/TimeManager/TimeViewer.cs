using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TimeManager {
    public class TimeViewer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _timeText;

        [SerializeField]
        private TextMeshProUGUI _readyTimeText;

        /// <summary>
        /// 表示する残り時間を更新する
        /// </summary>
        /// <param name="timeValue"></param>
        public void SetTimeText(int timeValue) {
            _timeText.text = timeValue.ToString();
        }

        /// <summary>
        /// 表示する開始前カウントダウン時間を更新する
        /// </summary>
        /// <param name="timeValue"></param>
        public void SetReadyTimeText(int readyTimeValue) {
            _readyTimeText.text = readyTimeValue == 0 ? "GO!" : readyTimeValue.ToString();
        }

        /// <summary>
        /// 表示する開始前カウントダウンの文字列を消す
        /// </summary>
        /// <param name="isVisible"></param>
        public void ClearReadyTimeText() {
            _readyTimeText.text = "";
        }

        /// <summary>
        /// 開始前カウントダウンの表示有無を設定する
        /// </summary>
        /// <param name="isVisible"></param>
        public void VisibleReadyTimeText(bool isVisible)
        {
            _readyTimeText.enabled = isVisible;
        }
    }
}