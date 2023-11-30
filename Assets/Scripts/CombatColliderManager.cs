using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCollider : MonoBehaviour
{
    public GameObject leftFist;
    public GameObject rightFist;
    public GameObject leftHandSword;
    public GameObject rightHandSword;
    private ThirdPersonController controller;

    private void Awake()
    {
        controller = GetComponent<ThirdPersonController>();
    }

    // Start is called before the first frame update
    public void IdleActive()
    {
        leftFist.SetActive(false);
        rightFist.SetActive(false);
        leftHandSword.SetActive(false);
        rightHandSword.SetActive(false);
    }

    public void LeftFistActive()
    {
        if (!controller.weaponEquipped)
        {
        leftFist.SetActive(true);
        }
        rightFist.SetActive(false);
        leftHandSword.SetActive(false);
        rightHandSword.SetActive(false);
    }
    public void RightFistActive()
    {
        leftFist.SetActive(false);
        if (!controller.weaponEquipped)
        {
            rightFist.SetActive(true);
        }
        leftHandSword.SetActive(false);
        rightHandSword.SetActive(false);
    }
    public void SingleHandSwordActive()
    {
        leftFist.SetActive(false);
        rightFist.SetActive(false);
        leftHandSword.SetActive(false);
        if (controller.weaponEquipped)
        {
        rightHandSword.SetActive(true);
        }
        Debug.Log("singlwe sword activated");
    }
    public void DoubleHandSwordActive()
    {
        leftFist.SetActive(false);
        rightFist.SetActive(false);
        if (controller.weaponEquipped)
        {
            rightHandSword.SetActive(true);
        }
        Debug.Log("Double sword activated");
    }
}
