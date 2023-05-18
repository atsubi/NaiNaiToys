using UnityEngine;
using UniRx;

namespace Players {
    
    public interface IInputProvider
    {
        IReadOnlyReactiveProperty<bool> IHoldAction { get; }
        IReadOnlyReactiveProperty<Vector3> IMoveDirection { get; }
    }

}
