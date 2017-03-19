using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class playerControl : MonoBehaviour {

    public RawImage myRawImage;

    private float playerInputQundE;
    private Rigidbody myBody;
    private cameraScript myCameraScript;

    // Use this for initialization
    void Start () {
        myBody = GetComponent<Rigidbody>();
        myCameraScript = Camera.main.GetComponent<cameraScript>();
        myCameraScript.setPlayer(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        playerInputQundE = Input.GetAxis("QundE");

        var x = Input.GetAxis("Horizontal") * 1.5f;
        var z = Input.GetAxis("Vertical") * 0.1f;
        transform.Rotate(0, x, 0);
        myCameraScript.followRotation(x);
        transform.Translate(0, 0, z);

        if (playerInputQundE < 0)
        {
            myBody.transform.position = myBody.transform.position + transform.right * 0.1f;
        }
        else if (playerInputQundE > 0)
        {
            myBody.transform.position = myBody.transform.position - transform.right * 0.1f;
        }

        if (Input.GetKey("space"))
        {
            myBody.transform.position = myBody.transform.position + (Vector3.up * 0.1f);
        }
        if (Input.GetKey("c"))
        {
            myBody.transform.position = myBody.transform.position - (Vector3.up * 0.1f);
        }

        if (Input.GetKeyDown("1"))
        {

        }
    }
}
