using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Toybox {

    public class ToyboxUIViewer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _scoreText;


        /// <summary>
        /// おもちゃ箱のスコアを更新する
        /// </summary>
        /// <param name="score"></param>
        public void SetScoreText(int score)
        {
            if (score > 100) {
                throw new System.Exception("おもちゃ箱のスコア最大値は100です。");
            }
            _scoreText.text = score.ToString();
        }
    }
}