using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [SerializeField] float EnemySpeed;
    [SerializeField] GameObject target;
    private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Player"))
        {
            Debug.Log("Lun");
            transform.LookAt(target.transform);
            transform.position -= transform.forward * EnemySpeed;
        }
    }
}
