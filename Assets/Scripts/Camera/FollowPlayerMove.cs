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
        void Update()
        {
            this.transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, this.transform.position.z);
            // Vector3 targetPos = new Vector3(_playerTransform.position.x, _playerTransform.position.y, this.transform.position.z);
            // Vector3 velocity = (targetPos - this.transform.position);
            // this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPos, ref velocity, 1.0f, Time.deltaTime);
        }
    }
}