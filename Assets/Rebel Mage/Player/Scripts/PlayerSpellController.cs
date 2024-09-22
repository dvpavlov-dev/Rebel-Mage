using Rebel_Mage.Spell_system;
using UnityEngine;
using Vanguard_Drone.Infrastructure;
using Zenject;

[RequireComponent(typeof(SpellsAction), typeof(ZenAutoInjecter))]
public class PlayerSpellController : MonoBehaviour
{
    private SpellsAction m_SpellsAction;
    private CameraManager m_CameraManager;
    private Camera m_Camera;

    [Inject]
    private void Constructor(CameraManager cameraManager)
    {
        m_CameraManager = cameraManager;
        m_Camera = cameraManager.CameraPlayer.GetComponent<Camera>();
    }

    public void Init()
    {
        m_SpellsAction = GetComponent<SpellsAction>();
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
                        m_SpellsAction.UseSpell(TypeSpell.BASE_ATTACK);
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        m_SpellsAction.UseSpell(TypeSpell.SUPPORT_ATTACK);
                    }

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        m_SpellsAction.UseSpell(TypeSpell.FIRST_SPELL);
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        m_SpellsAction.UseSpell(TypeSpell.SECOND_SPELL);
                    }

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        m_SpellsAction.UseSpell(TypeSpell.THIRD_SPELL);
                    }
                }
            }
        }
    }
}
