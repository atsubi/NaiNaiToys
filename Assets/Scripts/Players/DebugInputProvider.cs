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


        bool IInputProvider.InitializeInputEvent(CancellationToken token)
        {
            InitializeInputEventAsync(token).Forget();

            return true;
        }

        /// <summary>
        /// 入力の初期設定
        /// </summary>
        /// <returns></returns>
        public async UniTaskVoid InitializeInputEventAsync(CancellationToken token)
        {
            await UniTask.Delay(30, cancellationToken: token);

            this.UpdateAsObservable()
                .Select(_ => Input.GetMouseButton(0))
                .DistinctUntilChanged()
                .Subscribe(x => _holdAction.Value = x);

            this.UpdateAsObservable()
                .Select(_ => new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 2.0f))
                .Subscribe(x => _moveDirection.SetValueAndForceNotify(x));
            
            Debug.Log("Initalize Input");
        }
    }
}