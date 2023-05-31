using UnityEngine;

using Cysharp.Threading.Tasks;

using UniRx;

namespace Players {
    
    /// <summary>
    /// ユーザーの入力を受け付けるインターフェイス
    /// </summary>
    public interface IInputProvider
    {
        public IReadOnlyReactiveProperty<bool> IHoldAction { get; }
        public IReadOnlyReactiveProperty<Vector3> IMoveDirection { get; }
    }

}
