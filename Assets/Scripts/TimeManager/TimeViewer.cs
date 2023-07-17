using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TimeManager {
    public class TimeViewer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _timeText;

        /// <summary>
        /// 表示する残り時間を更新する
        /// </summary>
        /// <param name="timeValue"></param>
        public void SetTimeText(int timeValue) {
            _timeText.text = timeValue.ToString();
        }
    }
}