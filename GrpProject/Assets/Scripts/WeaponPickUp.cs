using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    //public GameObject weaponPrefab;
    //public float respawnTime = 5f;
    public GameObject[] weaponPrefabs; // equipped weapon prefabs  

    private Renderer render;
    private Collider collide;
    //private bool isPickedUp = false;
    [SerializeField] private Shooter shooterScript;
    private GameObject inventoryPanel, wpnPickupCanvas;

    void Awake()
    {
    }
    private void Start()
    {
        render = GetComponent<Renderer>();
        collide = GetComponent<Collider>();  
    }

    void OnTriggerEnter(Collider other)
    {  
        if (other.CompareTag("Player"))
        {
            inventoryPanel = other.gameObject.GetComponent<FPSInput>().inventoryPanel;
            wpnPickupCanvas = other.gameObject.GetComponent<FPSInput>().wpnPickupCanvas;
            shooterScript = other.gameObject.GetComponentInChildren<Shooter>();
            PickUp();
        }
    } 

    /*void PickUp()
    {
        if (!isPickedUp)
        {
            Debug.Log("Picking up weapon: " + weaponPrefab.name);
            isPickedUp = true;
            renderer.enabled = false;
            collider.enabled = false;

            Inventory.Instance.SwitchWeapon(weaponPrefab);

            Invoke(nameof(Respawn), respawnTime);
        }
    }

    void Respawn()
    {
        Debug.Log("Respawning weapon: " + weaponPrefab.name);
        isPickedUp = false;
        renderer.enabled = true;
        collider.enabled = true;
    }*/

    void PickUp()
    {
        if (wpnPickupCanvas != null)
        {
            shooterScript.gamePaused = true;
            wpnPickupCanvas.SetActive(true);
            // Debug.Log("Weapon canvas activated");
            Time.timeScale = 0; // pause game 

            // Unlock the cursor and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else Debug.LogError("Weapon canvas not found :(");

        // Optionally hide the weapon pickup
        render.enabled = false;
        collide.enabled = false; 

        // Optionally destroy the weapon pickup object
        Destroy(gameObject, 0.1f); // delay destroy slightly
    }

    public void RandomWeapon()
    { 
        if (weaponPrefabs.Length == 0)
        {
            Debug.LogError("No weapon prefabs assigned.");
            return;
        }

        GameObject randomWeapon;

        while (true)
        {
            // Choose a random weapon from the array
            randomWeapon = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
            Weapon randWpn = randomWeapon.GetComponent<Weapon>();
            if (randWpn != null)
            {
                if (!Inventory.Instance.IsDuplicate(randWpn))
                    break; // ensure the random weapon is new (not equipped) 
            }
            else 
            {
                Debug.LogError(randomWeapon.name + " does not have Weapon script attached");
                break;
            }
        }

        Debug.Log("Picking up weapon: " + randomWeapon.name); 

        // Add the weapon to the inventory
        Inventory.Instance.AddWeapon(randomWeapon);
    }
}
