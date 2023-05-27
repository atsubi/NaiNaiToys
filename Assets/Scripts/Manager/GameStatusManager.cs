using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Manager {

    public class GameStatusManager : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<GameStatus> IGameStatus => _gameStatus;
        private readonly ReactiveProperty<GameStatus> _gameStatus = new ReactiveProperty<GameStatus>(GameStatus.INITIALIZE);

        /// <summary>
        /// ゲームのステータスを更新する
        /// </summary>
        /// <param name="status"></param>
        public void ChangeGameStatus(GameStatus status)
        {
            _gameStatus.Value = status;
        }

    }
}