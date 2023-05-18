using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace IHoldable {
    public interface IHoldable
    {
        public IReadOnlyReactiveProperty<bool> IsHolded { get; }

        public bool TryHold();

        public void UnHold();
    }
}
