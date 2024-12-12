using System.Collections;
using Rebel_Mage.Configs;
using Rebel_Mage.Infrastructure;
using Rebel_Mage.Spell_system;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(ZenAutoInjecter))]
    public class PlayerMoveController : MonoBehaviour, IImpact, ICastSpells
    {
        [SerializeField] private Animator _animMove;
        [SerializeField] private GameObject _model;
        [SerializeField] private GameObject _dashEffectPrefab;
        
        private Camera _camera;
        private CameraManager _cameraManager;
        private float _moveCoefficient = 1;
        private float _moveSpeed;
        private float _cooldownDash;
        private bool _isBlockedControl;
        private bool _isPlayerSetup;
        private Vector3 _movement;
        private Rigidbody _rb;
        private IRoundProcess _roundProcess;
        private Coroutine _timerForSpeedEffects;
        private PlayerConfigSource _playerConfig;
        private Vector3 _rotatePlayerTo;

        [Inject]
        private void Constructor(CameraManager cameraManager)
        {
            _cameraManager = cameraManager;
        }

        public void Init(PlayerConfigSource playerConfig)
        {
            _playerConfig = playerConfig;
            _moveSpeed = _playerConfig.MoveSpeed;

            _rb = GetComponent<Rigidbody>();

            _cameraManager.SwitchCamera(TypeCamera.PLAYER_CAMERA);
            _camera = _cameraManager.CameraPlayer.GetComponent<Camera>();
            
            _isPlayerSetup = true;
        }

        private void Update()
        {
            if (_isBlockedControl || !_isPlayerSetup) return;

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

        private bool CheckCooldown()
        {
            if (_cooldownDash <= Time.time)
            {
                _cooldownDash = Time.time + _playerConfig.Dash_CooldownTime;
                return true;
            }

            return false;
        }

        private void ActivateDash()
        {
            if (!CheckCooldown()) return;
            
            float x = Input.GetAxis("Horizontal") == 0 ? 0 : Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float z = Input.GetAxis("Vertical") == 0 ? 0 : Input.GetAxis("Vertical") > 0 ? 1 : -1;
            Vector3 moveDictionary = new(x, 0, z);

            Push(moveDictionary);
        }

        private void Push(Vector3 dictionary)
        {
            StartDash();

            _rb.AddExplosionForce(100000, transform.position - dictionary, 10);
            Invoke(nameof(EndDash), 0.1f);
        }

        private void StartDash()
        {
            Instantiate(_dashEffectPrefab, transform.position, Quaternion.identity);
            
            _model.SetActive(false);
            _isBlockedControl = true;
        }

        private void EndDash()
        {
            Instantiate(_dashEffectPrefab, transform.position, Quaternion.identity);
            
            _model.SetActive(true);
            UnblockControl();
        }

        void ICastSpells.OnCastSpell(float castTime)
        {
            _isBlockedControl = true;
            _movement.Set(0, 0, 0);
            _rb.velocity = _movement;
            
            Invoke(nameof(UnblockControl), castTime);
        }
        
        void IImpact.ExplosionImpact(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            _isBlockedControl = true;
            _rb.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
            Invoke(nameof(UnblockControl), 0.1f);
        }

        void IImpact.ChangeSpeedImpact(float slowdown, float timeSlowdown)
        {
            if (!gameObject.activeSelf) return;

            _moveCoefficient = 1 - _moveCoefficient * slowdown;

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
            move *= _moveCoefficient;

            if (_animMove != null)
            {
                AnimTransform(move);
            }

            _movement.Set(_moveSpeed * move.x, 0, _moveSpeed * move.z);
            _rb.velocity = _movement;
        }

        private void AnimTransform(Vector3 directionMove)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            float animVert = directionMove.z * forward.z + directionMove.x * forward.x;
            float animHor = directionMove.x * right.x + directionMove.z * right.z;
            
            _animMove.SetFloat("Vertical", animVert);
            _animMove.SetFloat("Horizontal", animHor);
        }

        private void RotatePlayer()
        {
            if(_isBlockedControl)
                return;
            
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                _rotatePlayerTo.Set(worldPosition.x, worldPosition.y + 0.5f, worldPosition.z);
                transform.rotation = LookAt2D_Y(_rotatePlayerTo);
            }
        }

        private Quaternion LookAt2D_Y(Vector3 target)
        {
            var rotation = Quaternion.LookRotation(target - transform.position, transform.TransformDirection(Vector3.up));
            return new Quaternion(0, rotation.y, 0, rotation.w);
        }

        private void UnblockControl()
        {
            _isBlockedControl = false;
        }

        private IEnumerator ReturnSpeed(float timeWhenReturn)
        {
            yield return new WaitForSeconds(timeWhenReturn);
            _moveCoefficient = 1;
        }
    }
}
