using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using UniRx;

using Cysharp.Threading.Tasks;

using VContainer;
using VContainer.Unity;
using UnityEngine;
using Hold;

namespace Toybox {

    public class ToyboxPresenter : IAsyncStartable
    {
        private readonly IAcceptToyProvider _iAcceptToyProvider;
        private readonly ToyboxParameter _toyboxParameter;

        [Inject]
        public ToyboxPresenter(IAcceptToyProvider iAcceptToyProvider, ToyboxParameter toyboxParameter)
        {
            _iAcceptToyProvider = iAcceptToyProvider;
            _toyboxParameter    = toyboxParameter;

            _iAcceptToyProvider.InitializeAcceptToyEvent();
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken token)
        {
            await _iAcceptToyProvider.CompleteAcceptToyEventSetting;

            _iAcceptToyProvider.IAcceptToy
                .Where(collision => collision != null)
                .Do(collision => Debug.Log(collision.gameObject.name))
                .Where(collision => collision.gameObject.GetComponent<IHoldable>() != null)
                .Subscribe(collision => {
                    UnityEngine.Object.Destroy(collision.gameObject);
                });
        }
    }

}