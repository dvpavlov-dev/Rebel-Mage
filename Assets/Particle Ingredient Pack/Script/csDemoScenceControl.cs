using TMPro;
using UnityEngine;
public class csDemoScenceControl : MonoBehaviour
{

    public GameObject[] AllEffect;
    public TextMeshProUGUI Text;
    public Transform mg;
    public bool isIngredientScene;
    public bool isExampleScene;
    private csColorChangerinSampleScene cs;
    private int i;
    private GameObject MakedObject;
    private void Start()
    {
        i = 1;
        MakedObject = Instantiate(AllEffect[i - 1], AllEffect[i - 1].transform.position, Quaternion.identity);
        Text.text = "(" + i + "/" + AllEffect.Length + ") " + AllEffect[i - 1].name;
    }

    private void Update()
    {
        if (mg)
            cs = mg.GetComponent<csColorChangerinSampleScene>();


        if (Input.GetKeyDown(KeyCode.X))
        {
            if (i - 1 <= AllEffect.Length - 2)
                i++;
            else
                i = 1;
            Destroy(MakedObject);
            MakedObject = Instantiate(AllEffect[i - 1], AllEffect[i - 1].transform.position, AllEffect[i - 1].transform.rotation);
            Text.text = "(" + i + "/" + AllEffect.Length + ") " + AllEffect[i - 1].name;

            if (isIngredientScene)
            {
                if (cs.Saved)
                    cs.ChangeColor(cs.SaveColor);
                if (i >= 415 && i <= 425)
                    Text.text = "(NEW)" + Text.text;
            }

            if (isExampleScene)
            {
                if (i >= 30 && i <= 35)
                    Text.text = "(NEW)" + Text.text;
            }

        }

        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (i - 1 > 0)
                i--;
            else
                i = AllEffect.Length;
            Destroy(MakedObject);
            MakedObject = Instantiate(AllEffect[i - 1], AllEffect[i - 1].transform.position, AllEffect[i - 1].transform.rotation);
            Text.text = "(" + i + "/" + AllEffect.Length + ") " + AllEffect[i - 1].name;

            if (isIngredientScene)
            {
                if (cs.Saved)
                    cs.ChangeColor(cs.SaveColor);
                if (i >= 415 && i <= 425)
                    Text.text = "(NEW)" + Text.text;
            }

            if (isExampleScene)
            {
                if (i >= 30 && i <= 35)
                    Text.text = "(NEW)" + Text.text;
            }
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            Destroy(MakedObject);
            MakedObject = Instantiate(AllEffect[i - 1], AllEffect[i - 1].transform.position, AllEffect[i - 1].transform.rotation);
            Text.text = "(" + i + "/" + AllEffect.Length + ") " + AllEffect[i - 1].name;

            if (isIngredientScene)
            {
                if (cs.Saved)
                    cs.ChangeColor(cs.SaveColor);
                if (i >= 415 && i <= 425)
                    Text.text = "(NEW)" + Text.text;
            }

            if (isExampleScene)
            {
                if (i >= 30 && i <= 35)
                    Text.text = "(NEW)" + Text.text;
            }
        }
    }
}
