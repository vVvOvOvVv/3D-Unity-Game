using UnityEngine;

public class ArmorPickup : MonoBehaviour
{
    public int addArmor = 10; 

    private void OnTriggerEnter(Collider other)
    {
        FPSInput fps = other.GetComponent<FPSInput>();
        if (fps != null && fps.shield != fps.maxShield)
        {
            fps.shield += addArmor; 
            fps.UpdateArmorBar();
            Destroy(gameObject); // Destroy the health pickup object
        }
    }
}
