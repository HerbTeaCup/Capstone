using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainScannerDEMO
{
    public class ConstantSensor : MonoBehaviour
    {
        [SerializeField] private TerrainScanner.CameraEffect _cameraEffect;

        [SerializeField] private Material _sensorMaterial;

        [SerializeField] private GameObject _sensorOrigin;

        [SerializeField] private float _maxDistance;

        public float Radius
        {
            get { return _maxDistance; }
        }

        private void Start()
        {
            _cameraEffect.material = _sensorMaterial;
            _sensorMaterial.SetFloat("_Radius", _maxDistance);
        }

        private void OnDisable()
        {
            _sensorMaterial.SetFloat("_Radius", 0);
        }

        private void Update()
        {
            _sensorMaterial.SetVector("_RevealOrigin", transform.position);
        }
    }
}

