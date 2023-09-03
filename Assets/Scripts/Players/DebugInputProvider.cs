using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;

using Cysharp.Threading.Tasks;


namespace Players {

    public class DebugInputProvider :  MonoBehaviour, IInputProvider
    {
        private ReactiveProperty<bool> _holdAction = new ReactiveProperty<bool>(false);
        public IReadOnlyReactiveProperty<bool> IHoldAction => _holdAction;

        private ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();
        public IReadOnlyReactiveProperty<Vector3> IMoveDirection => _moveDirection;

        public UniTask CompleteInputEventSetting => _uniTaskCompletionSource.Task;
        private readonly UniTaskCompletionSource _uniTaskCompletionSource = new UniTaskCompletionSource();

        /// <summary>
        /// 入力の初期設定
        /// </summary>
        /// <returns></returns>
        void IInputProvider.InitializeInputEvent()
        {
            this.UpdateAsObservable()
                .Select(_ => Input.GetMouseButton(0))
                .DistinctUntilChanged()
                .Subscribe(x => {
                    Debug.Log("Hold");
                    _holdAction.Value = x;
                });

            this.UpdateAsObservable()
                .Select(_ => new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 2.0f))
                .Subscribe(x => _moveDirection.SetValueAndForceNotify(x));
            
            _uniTaskCompletionSource.TrySetResult();
        }
    }
}