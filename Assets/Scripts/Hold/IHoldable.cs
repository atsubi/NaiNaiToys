using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace Hold {
    public interface IHoldable
    {
        public bool TryHold();

        public void UnHold();
    }
}
