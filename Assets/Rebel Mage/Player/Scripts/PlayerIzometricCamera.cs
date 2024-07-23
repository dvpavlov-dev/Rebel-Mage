using UnityEngine;

namespace Rebel_Mage.Player
{
    public class PlayerIzometricCamera : MonoBehaviour
    {
        private Transform _target;
        private readonly Vector3 _cameraPosition = new Vector3(0, 20, -12);

        void Update()
        {
            if (_target != null)
            {
                transform.position = _target.position + _cameraPosition;
            }
        }

        private void OnEnable()
        {
            _target = GameObject.FindWithTag("Player").transform;
        }
    }
}
