using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainScannerDEMO
{
    public class Move : MonoBehaviour
    {
        private CharacterController _controller;
        [SerializeField] private float _playerSpeed;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _controller.Move(move * Time.deltaTime * _playerSpeed);
        }

    }
}
   
