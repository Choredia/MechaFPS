using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public static int selectedWeapon = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponInput();

    }

    private void WeaponInput()
    {
        int previosSelectedWeapon = selectedWeapon;

        WeaponInputMouseWheel();

        WeaponInputNumberKeys();

        if (previosSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    private void WeaponInputMouseWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else { selectedWeapon++; }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else { selectedWeapon--; }
        }
    }

    private void WeaponInputNumberKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }
    }

    private void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true); 
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
