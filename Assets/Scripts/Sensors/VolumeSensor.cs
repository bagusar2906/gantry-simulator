using System;
using EventArgs;
using JetBrains.Annotations;
using SimulatorHub;
using UnityEngine;

namespace Sensors
{
    public class VolumeSensor : MonoBehaviour
    {

        public VolumeSensorEnum id;
        public double measuredWeight = 0;
        
        [CanBeNull] private Tube _tube;
        
        public event EventHandler<LoadCellValueChangedEventArgs> OnInitialWeightChanged;

        private void OnTriggerEnter(Collider other)
        {
            var  tube = other.gameObject.GetComponentInParent<Tube>();
            if (tube == null) return;
            
            measuredWeight = tube.Weight;
            tube.WeightChanged -= OnTubeWeightChanged;
            tube.WeightChanged += OnTubeWeightChanged;
            
            OnInitialWeightChanged?.Invoke(this, new LoadCellValueChangedEventArgs()
            {
                Id = (short)id,
                Weight = measuredWeight
            });
            _tube = tube;
        }

        private void OnTubeWeightChanged(object sender, WeightChangedEventArgs e)
        {
            measuredWeight = e.Weight;
            OnInitialWeightChanged?.Invoke(this, new LoadCellValueChangedEventArgs()
            {
                Id = (short)id,
                Weight = measuredWeight
            });
        }

        private void OnTriggerExit(Collider other)
        {
            var  tube = other.gameObject.GetComponentInParent<Tube>();
            if (tube == null) return;
            tube.WeightChanged -= OnTubeWeightChanged;
            OnInitialWeightChanged?.Invoke(this, new LoadCellValueChangedEventArgs()
            {
                Id = (short)id,
                Weight = 0
            });
            _tube = null;
            measuredWeight = 0;
        }
        

        public void UpdateWeight(float volume)
        {
            if (_tube == null) return;
            _tube.Fill(volume - _tube.InitialWeight);
            measuredWeight = _tube.Weight;
        }

        
        private void OnCollisionEnter(Collision collision)
        {
            var  tube = collision.gameObject.GetComponentInParent<Tube>();
            if (tube == null) return;
            var rgb = tube.GetComponent<Rigidbody>();
            if (Math.Abs(measuredWeight - rgb.mass) < 0.001) return;
            
            measuredWeight = tube.Weight;
            tube.WeightChanged -= OnTubeWeightChanged;
            tube.WeightChanged += OnTubeWeightChanged;
            
            OnInitialWeightChanged?.Invoke(this, new LoadCellValueChangedEventArgs()
            {
                Id = (short)id,
                Weight = measuredWeight
            });
            measuredWeight = rgb.mass;
            _tube = tube;
        }

        private void OnCollisionExit(Collision other)
        {
            var  tube = other.gameObject.GetComponentInParent<Tube>();
            if (tube == null) return;
            if (measuredWeight == 0) return;
            tube.WeightChanged -= OnTubeWeightChanged;
            measuredWeight = 0;
            OnInitialWeightChanged?.Invoke(this, new LoadCellValueChangedEventArgs()
            {
                Id = (short)id,
                Weight = measuredWeight
            });
            
            _tube = null;
        }
    }
}
