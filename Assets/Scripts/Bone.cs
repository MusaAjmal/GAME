using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    [SerializeField] GameObject particleEffect;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bone")
        {
            Instantiate(particleEffect, collision.contacts[0].point, Quaternion.identity);
            SoundPlayer.PlayOneShotSound("bone");

        }
    }
}
