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
            _liquidControl = other.gameObject.GetComponentInChildren<LiquidControl>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.name.Contains("TubeAdapter"))
                return;
            _liquidControl = collision.gameObject.GetComponentInChildren<LiquidControl>();   
        }

        private void OnCollisionExit(Collision other)
        {
            _liquidControl = null;
        }

        private void OnTriggerExit(Collider other)
        {
            _liquidControl = null;
        }
        

        public VolumeSensorEnum id;

        [CanBeNull] private LiquidControl _liquidControl;

        public void UpdateVolume(float volume)
        {
            if (_liquidControl != null) _liquidControl.volume = volume;
        }
    }
}
