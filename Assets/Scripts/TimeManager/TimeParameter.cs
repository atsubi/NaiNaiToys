using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using VContainer;
using VContainer.Unity;

namespace TimeManager {

    public class TimeParameter
    {
        IntReactiveProperty _timeValue;
        public IReadOnlyReactiveProperty<int> TimeValue => _timeValue;

        [Inject]
        public TimeParameter(int timeValue)
        {
            if (timeValue <= 0) {
                throw new System.ArgumentException("timeValue is 1 or more.");
            }
            _timeValue = new IntReactiveProperty(timeValue);
        }

        public void UpdateTimeValue(int timeValue)
        {
            _timeValue.Value = timeValue;
        }
    }
}