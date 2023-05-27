using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Anger {

    public class AngerViewer : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        public void UpdateSliderValue(float value)
        {
            _slider.value = value;
        }
    }
}