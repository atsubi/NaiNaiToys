using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hold;
using VContainer;

namespace Toys {

    public class ToyHoldable : MonoBehaviour, IHoldable
    {
        private Holdable _holdable = new Holdable();

        bool IHoldable.TryHold() =>  _holdable.TryHold();

        void IHoldable.UnHold() => _holdable.UnHold();
    }
}
