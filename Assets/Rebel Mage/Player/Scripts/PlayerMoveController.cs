using System.Collections;
using Rebel_Mage.Configs;
using Rebel_Mage.Spell_system;
using UnityEngine;
using Vanguard_Drone.Infrastructure;
using Zenject;

namespace Vanguard_Drone.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(ZenAutoInjecter))]
    public class PlayerMoveController : MonoBehaviour, IImpact, ICastSpells
    {
        public bool IsBlockedControl 
        {
            get => m_IsBlockedControl;
            set => m_IsBlockedControl = value;
        }

        public Animator AnimMove;

        // public GameObject MousePoint;

        private Camera m_Camera;
        private CameraManager m_CameraManager;
        private float m_CurrentSpeed;
        private float m_CooldownDash;

        private bool m_IsBlockedControl;
        private bool m_IsPlayerSetup;
        private Vector3 m_Movement;
        private float m_MoveSpeed;
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

            m_CurrentSpeed = m_MoveSpeed;

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
            m_IsBlockedControl = true;

            m_Rb.AddExplosionForce(100000, transform.position - dictionary, 10);
            Invoke(nameof(UnblockControl), 0.1f);
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

            m_CurrentSpeed = m_MoveSpeed - m_MoveSpeed * slowdown;

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

            if (AnimMove != null)
            {
                AnimTransform(move);
            }

            m_Movement.Set(m_CurrentSpeed * move.x, 0, m_CurrentSpeed * move.z);
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
            m_CurrentSpeed = m_MoveSpeed;
        }
        public void OnCastSpell(float castTime)
        {
            m_IsBlockedControl = true;
            Invoke(nameof(UnblockControl), castTime);
        }
    }
}
