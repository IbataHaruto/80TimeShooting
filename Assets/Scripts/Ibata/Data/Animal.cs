using UnityEngine;

public class Animal : MonoBehaviour
{
    public AnimalData data;
    public AnimalStatusManager manager; //  ’Ç‰Á

    private void OnTriggerEnter(Collider other)
    {
        Fruit fruit = other.GetComponent<Fruit>();
        if (fruit != null)
        {
            manager.Feed(data, fruit.data);
            Destroy(other.gameObject);
        }
    }
}