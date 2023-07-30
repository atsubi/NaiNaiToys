using System.Collections;
using System.Collections.Generic;

using UniRx;

using VContainer;
using VContainer.Unity;

namespace Hold {

    public class Holdable
    {
        public BoolReactiveProperty IsHold { private set; get;}

        [Inject]
        public Holdable()
        {
            IsHold = new BoolReactiveProperty(false);
        }

        public bool TryHold()
        {
            if (IsHold.Value == true) return false;

            IsHold.Value = true;
            return true;
        }

        public void UnHold()
        {
            IsHold.Value = false;
        }
    }

}