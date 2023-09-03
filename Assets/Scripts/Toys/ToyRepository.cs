using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace Toys {

    public class ToyRepository : IDisposable
    {
        private CompositeDisposable _disposable = new CompositeDisposable();

        private ToyParamAsset _toyParamAsset;

        public ToyRepository(ToyParamAsset toyParamAsset)
        {
            _toyParamAsset = toyParamAsset;
        }

        public int GetToyWeight(int id)
        {
            
            return 0;
        }

        void IDisposable.Dispose() => _disposable.Dispose();
    }
}