using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Anger {

    public class AngerViewer : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        private const float _max_value = 100.0f;
        private const float _min_value = 0.0f;

        public void UpdateSliderValue(float value)
        {
            if (value > _max_value || value < _min_value) {
                throw new System.ArgumentException("value is 0 or more and 100 or less.");
            }
            _slider.value = value;
        }
    }
}