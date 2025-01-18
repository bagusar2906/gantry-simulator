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
            var  tube = other.gameObject.GetComponentInParent<Tube>();
            if (tube == null) return;
            _tube = tube;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var  tube = collision.gameObject.GetComponentInParent<Tube>();
            if (tube == null) return;
            _tube = tube;
        }

        private void OnCollisionExit(Collision other)
        {
            var  tube = other.gameObject.GetComponentInParent<Tube>();
            if (tube == null) return;
            _tube = null;
        }

        private void OnTriggerExit(Collider other)
        {
            var  tube = other.gameObject.GetComponentInParent<Tube>();
            if (tube == null) return;
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
