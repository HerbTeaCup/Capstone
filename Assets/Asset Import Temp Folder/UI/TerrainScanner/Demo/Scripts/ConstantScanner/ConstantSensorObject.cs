using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainScannerDEMO
{
    public class ConstantSensorObject : MonoBehaviour
    {
        [SerializeField] private ConstantSensor _detector;

        private AudioSource _audioTrigger;
        [SerializeField] AudioClip _sensorExit;

        [SerializeField] private Material _detectedMaterial;
        private Material _cachedMaterial;

        private MeshRenderer _meshRenderer;

        private bool _detected;

        [SerializeField] private float _timeToReset;
        private float _timerToReset;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _cachedMaterial = _meshRenderer.sharedMaterial;
            _audioTrigger = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _detector.transform.position) < (_detector.Radius - 1f))
            {
                if (_detected)
                {
                    _timerToReset = 0f;
                    return;
                }
                Detected();
            }
            else
            {
                if (_detected)
                {
                    if (_timerToReset > _timeToReset)
                    {
                        _detected = false;
                        _timerToReset = 0f;
                        _meshRenderer.sharedMaterial = _cachedMaterial;
                        _audioTrigger.PlayOneShot(_sensorExit);
                    }

                    _timerToReset += Time.deltaTime;
                }
            }
        }

        private void Detected()
        {
            _detected = true;
            _audioTrigger.Play();
            _meshRenderer.sharedMaterial = _detectedMaterial;
        }
    }
}
