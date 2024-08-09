using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestDialogue : MonoBehaviour
{
    [SerializeField] public GameObject obj;
    private void Start()
    {
       // obj = GameObject.FindWithTag("Dialogue");
    }
    public void appear()
    {
        if (obj != null) {
            obj.SetActive(true);
            obj.transform.LeanMoveLocal(new Vector2(57, 479), 1).setEaseOutQuart();
            StartCoroutine(DisableAfterDelay(1.5f));
        }
        else
        {
            obj = GameObject.FindWithTag("Dialogue");
        }
        
    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        if (obj != null) {
            yield return new WaitForSeconds(delay);
            obj.SetActive(false);
        }
        else
        {
            obj = GameObject.FindWithTag("Dialogue");
        }

    }
}
