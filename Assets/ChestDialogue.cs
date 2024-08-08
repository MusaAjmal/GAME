using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestDialogue : MonoBehaviour
{
    public void appear()
    {
        gameObject.SetActive(true);
        transform.LeanMoveLocal(new Vector2(57, 479), 1).setEaseOutQuart();
        StartCoroutine(DisableAfterDelay(1.5f));
    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
