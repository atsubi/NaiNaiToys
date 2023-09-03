using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Cysharp.Threading.Tasks;
using System.Threading;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Toys;

namespace Utility {

    public sealed class SoundManager : SingletonMonoBehaviour<SoundManager>, ISoundProvider
    {
        [Header("効果音の最大同時再生数")]
        private int _maxSoundEffects = 5;

        [SerializeField]
        private AudioSource _soundSourceEffect;

        [SerializeField]
        private AudioSource _soundSourceBGM;

        private Dictionary<string, AudioClip> _audioClipSEDic, _audioClipBGMDic;

        /// <summary>
        /// 効果音音源を読み込む
        /// </summary>
        /// <param name="addressName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask<bool> LoadSoundEffect(string addressName, CancellationToken token)
        {
            if (_audioClipSEDic.Any(kvp => kvp.Key == addressName)) {
                Debug.Log(addressName + " is alreadly loaded.");
                return false;
            }

            AudioClip audioClip = await Addressables.LoadAssetAsync<AudioClip>(addressName).WithCancellation(token);
            _audioClipSEDic.Add(addressName, audioClip);
            return true;
        }

        /// <summary>
        /// BGM音源を読み込む
        /// </summary>
        /// <param name="addressName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask<bool> LoadSoundBGM(string addressName, CancellationToken token)
        {
            if (_audioClipBGMDic.Any(kvp => kvp.Key == addressName)) {
                Debug.Log(addressName + " is alreadly loaded.");
                return false;
            }

            AudioClip audioClip = await Addressables.LoadAssetAsync<AudioClip>(addressName).WithCancellation(token);
            _audioClipBGMDic.Add(addressName, audioClip);
            return true;

        }


        /// <summary>
        /// 効果音を再生する
        /// </summary>
        /// <param name="addressName"></param>
        public void PlaySoundEffect(string addressName)
        {
            if (_audioClipSEDic.Any(kvp => kvp.Key == addressName)!) {
                Debug.Log(addressName + " is not loaded in Sound Manager.");
                return;
            }
            
            _soundSourceEffect.PlayOneShot(_audioClipSEDic[addressName]);
        }


        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="addressName"></param>
        /// <param name="fadeTime"></param>
        public void PlaySoundBGM(string addressName, float fadeInTime = 0.0f)
        {
            if (_soundSourceBGM.isPlaying) {
                Debug.Log("Other BGM is playing now.");
                return;
            }

            if (_audioClipBGMDic.Any(kvp => kvp.Key == addressName)!) {
                Debug.Log(addressName + " is not loaded in Sound Manager.");
                return;
            }
            
            _soundSourceBGM.clip = _audioClipBGMDic[addressName];
            _soundSourceBGM.Play();
        }

        /// <summary>
        /// BGMを停止する
        /// </summary>
        /// <param name="fadeOutTime"></param>
        public void StopSoundBGM(float fadeOutTime = 0.0f)
        {
            if (!_soundSourceBGM.isPlaying) {
                Debug.Log("BGM is not playing now.");
                return;
            }

            _soundSourceBGM.Stop();
        }

    }
}