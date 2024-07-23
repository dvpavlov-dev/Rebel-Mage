using System.Collections;
using PushItOut.Spell_system;
using PushItOut.UI.Gameplay;
using UnityEngine;
using Vanguard_Drone.Infrastructure;
using Zenject;

namespace Vanguard_Drone.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(ZenAutoInjecter))]
    public class PlayerMoveController : MonoBehaviour, IImpact
    {
        public bool IsBlockedControl {
            get => m_IsBlockedControl;
            set => m_IsBlockedControl = value;
        }

        // public GameObject MousePoint;
        private SpellsAction m_SpellsAction;

        private Camera _camera;
        private CameraManager _cameraManager;
        private float _currentSpeed;
        private GameplayUI _gameplayUI;

        private bool m_IsBlockedControl;
        private bool _isPlayerSetup;
        private Vector3 _movement;
        private float _moveSpeed;
        private Rigidbody _rb;

        private IRoundProcess _roundProcess;

        private Coroutine _timerForSpeedEffects;

        [Inject]
        private void Constructor(CameraManager cameraManager)
        {
            _cameraManager = cameraManager;
        }

        public void Init(float moveSpeed)
        {
            _moveSpeed = moveSpeed;

            _rb = GetComponent<Rigidbody>();

            _cameraManager.SwitchCamera(TypeCamera.PLAYER_CAMERA);
            _camera = _cameraManager.CameraPlayer.GetComponent<Camera>();

            _currentSpeed = _moveSpeed;

            _isPlayerSetup = true;
        }

        private void Update()
        {
            if (m_IsBlockedControl || !_isPlayerSetup) return;

            if (_roundProcess is { IsRoundInProgress: false })
            {
                _movement.Set(0, 0, 0);
                _rb.velocity = _movement;
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ActivateDash();
            }

            RotatePlayer();
            CalcMove();
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;

            StopAllCoroutines();
            _cameraManager.SwitchCamera(TypeCamera.ENVIRONMENT_CAMERA);
        }

        private void ActivateDash()
        {
            float x = Input.GetAxis("Horizontal") == 0 ? 0 : Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float z = Input.GetAxis("Vertical") == 0 ? 0 : Input.GetAxis("Vertical") > 0 ? 1 : -1;
            Vector3 moveDictionary = new(x, 0, z);

            Push(moveDictionary);
        }

        private void Push(Vector3 dictionary)
        {
            m_IsBlockedControl = true;

            _rb.AddExplosionForce(100000, transform.position - dictionary, 10);
            Invoke(nameof(UnblockControl), 0.1f);
        }

        public void ExplosionImpact(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            m_IsBlockedControl = true;
            _rb.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
            Invoke(nameof(UnblockControl), 0.1f);
        }

        public void ChangeSpeedImpact(float slowdown, float timeSlowdown)
        {
            if (!gameObject.activeSelf) return;

            _currentSpeed = _moveSpeed - _moveSpeed * slowdown;

            if (_timerForSpeedEffects != null)
            {
                StopCoroutine(_timerForSpeedEffects);
            }

            _timerForSpeedEffects = StartCoroutine(ReturnSpeed(timeSlowdown));
        }

        private void CalcMove()
        {
            float deltaX = Input.GetAxis("Horizontal");
            float deltaZ = Input.GetAxis("Vertical");
            Vector3 move = new(deltaX, 0, deltaZ);

            move.Normalize();

            _movement.Set(_currentSpeed * move.x, 0, _currentSpeed * move.z);
            _rb.velocity = _movement;
        }

        private void RotatePlayer()
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                // MousePoint.transform.position = new Vector3(worldPosition.x, worldPosition.y + 0.5f, worldPosition.z);
                transform.rotation = LookAt2D_Y(new Vector3(worldPosition.x, worldPosition.y + 0.5f, worldPosition.z));
            }
        }

        private Quaternion LookAt2D_Y(Vector3 target)
        {
            var rotation = Quaternion.LookRotation(target - transform.position,
                transform.TransformDirection(Vector3.up));
            return new Quaternion(0, rotation.y, 0, rotation.w);
        }

        private void UnblockControl()
        {
            m_IsBlockedControl = false;
        }

        private IEnumerator ReturnSpeed(float timeWhenReturn)
        {
            yield return new WaitForSeconds(timeWhenReturn);
            _currentSpeed = _moveSpeed;
        }
    }
}
