using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] public Slider hpSlider;
    private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    public void UpdateHPBar(int hp, int maxHP)
    {
        hpSlider.value = (float)hp / maxHP;
        // Debug.Log("Slider value: " + hpSlider.value);
    }

    public void Update()
    {
        transform.rotation = cam.transform.rotation; // ensure hp bar always faces camera
        if (target != null) 
            transform.position = target.position + offset; // keep hp bar on top of enemy
    }
}
