using System;
using System.Threading;

using VContainer;
using VContainer.Unity;

using UniRx;

using Cysharp.Threading.Tasks;

using Hold;

namespace Toys {

    /// <summary>
    /// おもちゃ単体の制御フロー
    /// </summary>
    public class ToyPresenter : IDisposable, IAsyncStartable {

        private readonly ToyVisual _toyVisual;

        private CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        public ToyPresenter(ToyVisual toyVisual)
        {
            _toyVisual = toyVisual;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _toyVisual.CompleteSetVisualAsync;
        }

        void IDisposable.Dispose() => _disposable.Dispose();

    }
}