using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using UniRx;
using UniRx.Triggers;

namespace Toybox {

    public class DebugAcceptToyProvider : MonoBehaviour, IAcceptToyProvider
    {
        public IReadOnlyReactiveProperty<Collision2D> IAcceptToy => _acceptToy;
        private ReactiveProperty<Collision2D> _acceptToy = new ReactiveProperty<Collision2D>(null);

        public UniTask CompleteAcceptToyEventSetting => _uniTaskCompletionSource.Task;
        private readonly UniTaskCompletionSource _uniTaskCompletionSource = new UniTaskCompletionSource();

        void IAcceptToyProvider.InitializeAcceptToyEvent()
        {
            this.OnCollisionEnter2DAsObservable()
                .Subscribe(collision => {
                    Debug.Log("Toy Enter");
                    _acceptToy.SetValueAndForceNotify(collision);
                });
            
            _uniTaskCompletionSource.TrySetResult();
            Debug.Log("CompleteAcceptToySetting");
        }
    }
}