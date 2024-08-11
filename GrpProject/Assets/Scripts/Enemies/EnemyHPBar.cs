using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset; 

    public void UpdateHPBar(int hp, int maxHP)
    {
        hpSlider.value = hp / maxHP;
    }

    private void Update()
    {
        transform.rotation = cam.transform.rotation; // ensure hp bar always faces camera
        transform.position = target.position + offset; // keep hp bar on top of enemy
    }
}
