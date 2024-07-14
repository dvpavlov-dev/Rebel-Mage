using System.Collections;
using Bizniz;
using PushItOut.Configs;
using PushItOut.Spell_system;
using PushItOut.Spell_system.Configs;
using PushItOut.UI.Gameplay;
using UnityEngine;
using Vanguard_Drone.Infrastructure;
using Zenject;

namespace Vanguard_Drone.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour, IImpact
    {
        public GameObject MousePoint;
        public SpellsAction SpellsAction;

        private Camera _camera;
        private CameraManager _cameraManager;
        private float _currentSpeed;
        private GameplayUI _gameplayUI;

        private Coroutine _timerForSpeedEffects;

        private bool _isBlockedControl;
        private bool _isPlayerSetup;
        private Vector3 _movement;
        private float _moveSpeed;
        private Rigidbody _rb;

        private RoundProcess _roundProcess;
        
        [Inject]
        private void Constructor(RoundProcess roundProcess, GameplayUI gameplayUI, CameraManager cameraManager)
        {
            _roundProcess = roundProcess;
            _gameplayUI = gameplayUI;
            _cameraManager = cameraManager;
        }

        public void SetupPlayerController(float moveSpeed)
        {
            _moveSpeed = moveSpeed;

            _rb = GetComponent<Rigidbody>();
            _cameraManager.SwitchCamera(TypeCamera.PLAYER_CAMERA);
            _camera = _cameraManager.CameraPlayer.GetComponent<Camera>();
            _camera.GetComponent<CameraController>().FollowTarget = transform;
            _currentSpeed = _moveSpeed;

            SpellsAction.OnSpellActivate += ActivateSpellOnSelf;

            _isPlayerSetup = true;
        }

        private void Update()
        {
            if (!_isPlayerSetup || !_roundProcess.IsRoundInProgress || _isBlockedControl)
            {
                if (!_isPlayerSetup || !_roundProcess.IsRoundInProgress)
                {
                    _movement.Set(0, 0, 0);
                    _rb.velocity = _movement;
                }
                
                return;
            }

            RotatePlayer();
            CalcMove();

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SpellsAction.UseSpell(TypeSpell.SHIFT_SPELL);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _gameplayUI.Menu.SetActive(!_gameplayUI.Menu.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            if(!gameObject.scene.isLoaded) return;
            
            StopAllCoroutines();
            _cameraManager.SwitchCamera(TypeCamera.ENVIRONMENT_CAMERA);
        }

        public void ExplosionImpact(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            _isBlockedControl = true;
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
        
        private void ActivateSpellOnSelf(SpellConfig spell)
        {
            if (spell is DashConfigSource)
            {
                ActivateDash();
            }
        }

        private void CalcMove()
        {
            float deltaX = Input.GetAxis("Horizontal");
            float deltaZ = Input.GetAxis("Vertical");
            Vector3 move = new (deltaX, 0, deltaZ);

            move.Normalize();

            _movement.Set(_currentSpeed * move.x, 0, _currentSpeed * move.z);
            _rb.velocity = _movement;
        }

        private void Push(Vector3 dictionary)
        {
            _isBlockedControl = true;

            Debug.Log($"dictionary: {dictionary}, explosionPosition: {transform.position - dictionary}");
            _rb.AddExplosionForce(100000, transform.position - dictionary, 10);
            Invoke(nameof(UnblockControl), 0.1f);
        }
        
        private void ActivateDash()
        {
            float x = Input.GetAxis("Horizontal") == 0 ? 0 : Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float z = Input.GetAxis("Vertical") == 0 ? 0 : Input.GetAxis("Vertical") > 0 ? 1 : -1;
            Vector3 moveDictionary = new (x, 0, z);

            Push(moveDictionary);
        }

        private void RotatePlayer()
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                MousePoint.transform.position = new Vector3(worldPosition.x, worldPosition.y + 0.5f, worldPosition.z);
                transform.rotation = LookAt2D_Y(MousePoint.transform);

                /* || Input.GetTouch(0).phase == TouchPhase.Began*/
                if (Input.GetKeyDown(KeyCode.Mouse0) ||
                    Input.GetKeyDown(KeyCode.Mouse1) ||
                    Input.GetKeyDown(KeyCode.Q) ||
                    Input.GetKeyDown(KeyCode.E) ||
                    Input.GetKeyDown(KeyCode.R))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(_camera.transform.position, ray.direction, out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(_camera.transform.position, ray.direction * hit.distance, Color.yellow);

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            SpellsAction.UseSpell(TypeSpell.BASE_ATTACK);
                        }

                        if (Input.GetKeyDown(KeyCode.Mouse1))
                        {
                            SpellsAction.UseSpell(TypeSpell.SUPPORT_ATTACK);
                        }

                        if (Input.GetKeyDown(KeyCode.Q))
                        {
                            SpellsAction.UseSpell(TypeSpell.FIRST_SPELL);
                        }

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            SpellsAction.UseSpell(TypeSpell.SECOND_SPELL);
                        }

                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            SpellsAction.UseSpell(TypeSpell.THIRD_SPELL);
                        }
                    }
                }
            }
        }

        private Quaternion LookAt2D_Y(Transform target)
        {
            var rotation = Quaternion.LookRotation(target.position - transform.position,
                transform.TransformDirection(Vector3.up));
            return new Quaternion(0, rotation.y, 0, rotation.w);
        }

        private void UnblockControl()
        {
            _isBlockedControl = false;
        }

        private IEnumerator ReturnSpeed(float timeWhenReturn)
        {
            yield return new WaitForSeconds(timeWhenReturn);
            _currentSpeed = _moveSpeed;
        }
    }
}
