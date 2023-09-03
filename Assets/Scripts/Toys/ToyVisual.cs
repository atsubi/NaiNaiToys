using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Toys {
    public class ToyVisual : MonoBehaviour
    {
        public UniTask CompleteSetVisualAsync => _uniTaskCompletionSource.Task;
        private readonly UniTaskCompletionSource _uniTaskCompletionSource = new UniTaskCompletionSource();

        public void InitializeToyVisual(int id)
        {
            _uniTaskCompletionSource.TrySetResult();
        }

        private void OnDestroy()
        {
            _uniTaskCompletionSource.TrySetCanceled();
        }
    }
}