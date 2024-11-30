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
        public bool IsBlockedControl 
        {
            get => m_IsBlockedControl;
            set => m_IsBlockedControl = value;
        }

        [SerializeField] private Animator AnimMove;

        [SerializeField] private GameObject model;
        [SerializeField] private GameObject dashEffectPrefab;
        
        // public GameObject MousePoint;

        private Camera m_Camera;
        private CameraManager m_CameraManager;
        private float m_MoveCoefficient = 1;
        private float m_MoveSpeed;
        private float m_CooldownDash;
        private bool m_IsBlockedControl;
        private bool m_IsPlayerSetup;
        private Vector3 m_Movement;
        private Rigidbody m_Rb;

        private IRoundProcess m_RoundProcess;

        private Coroutine m_TimerForSpeedEffects;
        private PlayerConfigSource m_PlayerConfig;

        [Inject]
        private void Constructor(CameraManager cameraManager)
        {
            m_CameraManager = cameraManager;
        }

        public void Init(PlayerConfigSource playerConfig)
        {
            m_PlayerConfig = playerConfig;
            m_MoveSpeed = m_PlayerConfig.MoveSpeed;

            m_Rb = GetComponent<Rigidbody>();

            m_CameraManager.SwitchCamera(TypeCamera.PLAYER_CAMERA);
            m_Camera = m_CameraManager.CameraPlayer.GetComponent<Camera>();
            
            m_IsPlayerSetup = true;
        }

        private void Update()
        {
            if (m_IsBlockedControl || !m_IsPlayerSetup) return;

            if (m_RoundProcess is { IsRoundInProgress: false })
            {
                m_Movement.Set(0, 0, 0);
                m_Rb.velocity = m_Movement;
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
            m_CameraManager.SwitchCamera(TypeCamera.ENVIRONMENT_CAMERA);
        }
        
        private bool CheckCooldown()
        {
            if (m_CooldownDash <= Time.time)
            {
                m_CooldownDash = Time.time + m_PlayerConfig.Dash_CooldownTime;
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

            m_Rb.AddExplosionForce(100000, transform.position - dictionary, 10);
            Invoke(nameof(EndDash), 0.1f);
        }

        private void StartDash()
        {
            Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);
            
            model.SetActive(false);
            m_IsBlockedControl = true;
        }
        
        private void EndDash()
        {
            Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);
            
            model.SetActive(true);
            UnblockControl();
        }

        void IImpact.ExplosionImpact(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            m_IsBlockedControl = true;
            m_Rb.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
            Invoke(nameof(UnblockControl), 0.1f);
        }

        void IImpact.ChangeSpeedImpact(float slowdown, float timeSlowdown)
        {
            if (!gameObject.activeSelf) return;

            m_MoveCoefficient = 1 - m_MoveCoefficient * slowdown;

            if (m_TimerForSpeedEffects != null)
            {
                StopCoroutine(m_TimerForSpeedEffects);
            }

            m_TimerForSpeedEffects = StartCoroutine(ReturnSpeed(timeSlowdown));
        }

        private void CalcMove()
        {
            float deltaX = Input.GetAxis("Horizontal");
            float deltaZ = Input.GetAxis("Vertical");
            
            Vector3 move = new(deltaX, 0, deltaZ);
            move.Normalize();
            move *= m_MoveCoefficient;

            if (AnimMove != null)
            {
                AnimTransform(move);
            }

            m_Movement.Set(m_MoveSpeed * move.x, 0, m_MoveSpeed * move.z);
            m_Rb.velocity = m_Movement;
        }

        private void AnimTransform(Vector3 directionMove)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            float animVert = directionMove.z * forward.z + directionMove.x * forward.x;
            float animHor = directionMove.x * right.x + directionMove.z * right.z;
            
            AnimMove.SetFloat("Vertical", animVert);
            AnimMove.SetFloat("Horizontal", animHor);
        }

        private void RotatePlayer()
        {
            if(m_IsBlockedControl)
                return;
            
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);

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
            m_MoveCoefficient = 1;
        }
        public void OnCastSpell(float castTime)
        {
            m_IsBlockedControl = true;
            m_Movement.Set(0, 0, 0);
            m_Rb.velocity = m_Movement;
            
            Invoke(nameof(UnblockControl), castTime);
        }
    }
}
