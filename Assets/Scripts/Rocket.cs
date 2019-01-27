using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {


    // SerializeField, editable in the inspector in unity, not other scripts
    [SerializeField] float rcsThrust = 100f; // rcs, reaction control system
    [SerializeField] float mainThrust = 100f;


    Rigidbody rigidBody;
    AudioSource audio;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
	}


    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene",1f); // parameterize time
                break;
            default:
                print("dead");
                SceneManager.LoadScene(0);
                break;



        }
    }



    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {

            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audio.isPlaying)
                audio.Play();
        }
        else
        {
            audio.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            //print("Rotating Left");
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
           // print("Rotating right");
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;
    }
}
