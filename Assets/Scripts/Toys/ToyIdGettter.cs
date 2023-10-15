using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

namespace Toys {

    public class ToyIdGettter : MonoBehaviour
    {
        private ToyParameter _toyParameter;

        public void SetToyParamter(ToyParameter toyParameter)
        {
            _toyParameter = toyParameter;
        }

        public IReadOnlyReactiveProperty<int> ReferenceToyID()
        {
            return _toyParameter.Id;
        }
    }
}