using UnityEngine;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour {
	public Rigidbody2D bulletPrefab;
	public float speed = 2.0f;

    void Start() 
	{
		// TODO+ attach transform to local player gameobject
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
			Rigidbody2D bPrefab = Instantiate(bulletPrefab, transform.position, transform.rotation) as Rigidbody2D;
			bPrefab.AddForce(transform.up * speed);
        }


    }
}
