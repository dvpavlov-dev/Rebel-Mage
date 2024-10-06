using Rebel_Mage.Spell_system;
using UnityEngine;
using Vanguard_Drone.Infrastructure;
using Zenject;

[RequireComponent(typeof(ZenAutoInjecter))]
public class PlayerSpellController : MonoBehaviour
{
    public Transform SpellPoint;
    public Animator AnimatorCastSpell;
    
    private CameraManager m_CameraManager;
    private Camera m_Camera;
    private Spells m_Spells;

    [Inject]
    private void Constructor(Spells spells, CameraManager cameraManager)
    {
        m_Spells = spells;
        m_CameraManager = cameraManager;
        m_Camera = cameraManager.CameraPlayer.GetComponent<Camera>();
    }

    void Update()
    {
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);

            /* || Input.GetTouch(0).phase == TouchPhase.Began*/
            if (Input.GetKeyDown(KeyCode.Mouse0) ||
                Input.GetKeyDown(KeyCode.Mouse1) ||
                Input.GetKeyDown(KeyCode.Q) ||
                Input.GetKeyDown(KeyCode.E) ||
                Input.GetKeyDown(KeyCode.R))
            {
                RaycastHit hit;
                if (Physics.Raycast(m_Camera.transform.position, ray.direction, out hit, Mathf.Infinity))
                {
                    Debug.DrawRay(m_Camera.transform.position, ray.direction * hit.distance, Color.yellow);

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        UseSpell(TypeSpell.BASE_ATTACK);
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        UseSpell(TypeSpell.SUPPORT_ATTACK);
                    }

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        UseSpell(TypeSpell.FIRST_SPELL);
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        UseSpell(TypeSpell.SECOND_SPELL);
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        UseSpell(TypeSpell.THIRD_SPELL);
                    }
                }
            }
        }
    }
    
    private void UseSpell(TypeSpell typeSpell)
    {
        m_Spells.CastSpell(typeSpell, AnimatorCastSpell, SpellPoint, gameObject);
    }
}
