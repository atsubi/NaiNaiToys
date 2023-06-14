using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyCamera {

    [RequireComponent(typeof(Camera))]
    public class FollowPlayerMove : MonoBehaviour
    {
        [SerializeField]
        private GameObject player;

        private Transform _playerTransform;

        void Start()
        {
            _playerTransform = player.gameObject.transform;
        }

        /// <summary>
        /// プレイヤーの位置に追従する
        /// </summary>
        void LateUpdate()
        {
            this.transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, -10.0f);
        }
    }
}