using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject pistol;   // Arma antiga
    public GameObject shotgun;  // Escopeta nova

    private GameObject activeWeapon;

    void Start()
    {
        // Começa com a pistola ativada
        activeWeapon = pistol;

        pistol.SetActive(true);
        shotgun.SetActive(false);
    }

    void Update()
    {
        // Aperta 1 → Pistola
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(pistol);
        }

        // Aperta 2 → Escopeta
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(shotgun);
        }
    }

    void SwitchWeapon(GameObject newWeapon)
    {
        if (activeWeapon == newWeapon)
            return;

        activeWeapon.SetActive(false);
        newWeapon.SetActive(true);

        activeWeapon = newWeapon;
    }
}
