using System;

using VContainer;
using VContainer.Unity;

using UniRx;

using Cysharp.Threading.Tasks;

using Hold;

namespace Toys {

    /// <summary>
    /// おもちゃ単体の制御フロー
    /// </summary>
    public class ToyPresenter : IStartable, IDisposable, IHoldable {

        private readonly Holdable _holdable;

        private CompositeDisposable _disposable;

        [Inject]
        public ToyPresenter(Holdable holdable)
        {
            _holdable = holdable;
        }
        
        void IStartable.Start()
        {
            
        }

        bool IHoldable.TryHold() => _holdable.TryHold();

        void IHoldable.UnHold() => _holdable.UnHold();

        void IDisposable.Dispose() => _disposable.Dispose();

    }
}