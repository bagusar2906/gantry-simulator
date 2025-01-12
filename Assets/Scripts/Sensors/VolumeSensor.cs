using System;
using JetBrains.Annotations;
using SimulatorHub;
using UnityEngine;

namespace Sensors
{
    public class VolumeSensor : MonoBehaviour
    {
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Selectable"))
                return;
            _tube = other.gameObject.GetComponent<Tube>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.name.Contains("TubeAdapter"))
                return;
            _tube = collision.gameObject.GetComponentInChildren<Tube>();   
        }

        private void OnCollisionExit(Collision other)
        {
            _tube = null;
        }

        private void OnTriggerExit(Collider other)
        {
            _tube = null;
        }
        

        public VolumeSensorEnum id;

        [CanBeNull] private Tube _tube;

        public void UpdateVolume(float volume)
        {
            if (_tube != null) _tube.Fill(volume);
        }
    }
}
