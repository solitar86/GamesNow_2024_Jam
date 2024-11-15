using UnityEngine;

public class GravityObject : MonoBehaviour
{
    public Rigidbody rb;
    
    void Awake() {
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb.useGravity == false)
        {
            rb.AddForce(new Vector3 (Random.Range(-1f,1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
        }
    }
    void Update() {
        //if (Input.GetKeyDown(KeyCode.G)) {
        //GravityEnabled();}
    }


    public void GravityEnabled() {
        rb.useGravity = true;
    }

}
