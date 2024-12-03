using UnityEngine;
using UnityEngine.Animations.Rigging;
public class mParent : MonoBehaviour
{
    public GameObject mParentCon;

    private Mode m_Mode;

    public void Start()
    {
        m_Mode = Mode.Ground;
        Debug.Log("ground");
    }

    public void Update()
    {
        if (m_Mode != Mode.Idle)
        {
            MultiParentConstraint constraint = mParentCon.GetComponent<MultiParentConstraint>();
            WeightedTransformArray sourceObjects = constraint.data.sourceObjects;

            sourceObjects.SetWeight(0, m_Mode == Mode.Ground ? 1f : 0f);
            sourceObjects.SetWeight(1, m_Mode == Mode.Hand ? 1f : 0f);
            sourceObjects.SetWeight(2, m_Mode == Mode.Back ? 1f : 0f);
            constraint.data.sourceObjects = sourceObjects;

            m_Mode = Mode.Idle;
        }
    }
    public void hand()
    {
        m_Mode = Mode.Hand;
        Debug.Log("hand");
    }

    public void back()
    {
        m_Mode = Mode.Back;
        Debug.Log("back");
    }

    private enum Mode
    {
        Idle,
        Ground,
        Hand,
        Back
    }
}
