using System;
using System.Threading;

using UnityEngine;

using VContainer;
using VContainer.Unity;

using UniRx;

using Cysharp.Threading.Tasks;

using Hold;

namespace Toys {

    /// <summary>
    /// おもちゃ単体の制御フロー
    /// </summary>
    public class ToyPresenter : IStartable, IDisposable {

        private readonly GameObject _toyObject;
        private readonly ToyParameter _toyParameter;

        private CompositeDisposable _disposable = new CompositeDisposable();

        public ToyPresenter(GameObject toyObject, ToyParameter toyParameter)
        {
            _toyObject = toyObject;            
            _toyParameter = toyParameter;
        }


        void IStartable.Start()
        {
            
        }

        void IDisposable.Dispose() => _disposable.Dispose();
    }
}