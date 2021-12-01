using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    //33

    [Header("Ship Movement")]

    [SerializeField] float flySpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem rocketParticle;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deathParticle;

   

    Rigidbody rigidBody;
    AudioSource audioSource;

    public bool collisionDisabled = false;

    enum STATE { ALIVE, DYING,TRASCENDING}

    STATE state = STATE.ALIVE;

    void Start()
    {
        rocketParticle.Play();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (state == STATE.ALIVE)
        {
            Thrust();
            Rotate();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (state != STATE.ALIVE || collisionDisabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Platform":
                //todo
                break;

            case "Land":
                StartSuccessSequence();
                break;

            case "Fuel":
                //todo
                break;

            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = STATE.TRASCENDING;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticle.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = STATE.DYING;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticle.Play();
        Invoke("RocketExplode", 2.5f);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(transform.up * flySpeed );

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rocketParticle.Play();
            ApplyThrust();
        }
        else
        {          
            audioSource.Stop();
            rocketParticle.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.angularVelocity = Vector3.zero;

        float rotation = rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotation);
        }

        if (Input.GetKey(KeyCode.D))
        {
            
            transform.Rotate(Vector3.forward * -rotation);
        }

        rigidBody.freezeRotation = false;
    }

    private void RocketExplode()
    {
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
       
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;    
        }
            

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
    }

}
