using System.Threading;
using System.Collections;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Utility {

    /// <summary>
    /// サウンド制御用インターフェイス
    /// </summary>
    public interface ISoundProvider
    {
        public UniTask<bool> LoadSoundEffect(string addressName, CancellationToken token);
        public UniTask<bool> LoadSoundBGM(string addressName, CancellationToken token);

        public void PlaySoundEffect(string addressName);
        public void PlaySoundBGM(string addressName, float fadeInTime = 0.0f);

        public void StopSoundBGM(float fadeOutTime = 0.0f);
    }

}