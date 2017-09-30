using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
		[SerializeField] private int m_PlayerNumber;
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
		[SerializeField] private float m_JumpSpeed;
		[SerializeField] private float m_RotationSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        //[SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
		private bool m_attract;
		private bool m_repulse;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

		//Strings to differentiate player input
		private string input_A;
		private string input_R1;
		private string input_L1;
		private string input_LSTICKH;
		private string input_LSTICKV;
		private string input_RSTICKH;
		private string input_RSTICKV;

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			//m_MouseLook.Init(transform , m_Camera.transform);
			//Initialize Player Number
			if (m_PlayerNumber == 1) {
				input_A = "P1-A";
				input_R1 = "P1-R1";
				input_L1 = "P1-L1";
				input_LSTICKH = "P1-LStickHorizontal";
				input_LSTICKV = "P1-LStickVertical";
				input_RSTICKH = "P1-RStickHorizontal";
				input_RSTICKV = "P1-RStickVertical";
			} else if (m_PlayerNumber == 2) {
				input_A = "P2-A";
				input_R1 = "P2-R1";
				input_L1 = "P2-L1";
				input_LSTICKH = "P2-LStickHorizontal";
				input_LSTICKV = "P2-LStickVertical";
				input_RSTICKH = "P2-RStickHorizontal";
				input_RSTICKV = "P2-RStickVertical";
			}
        }


        // Update is called once per frame
        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {

				m_Jump = CrossPlatformInputManager.GetButtonDown(input_A);
            }
			if (!m_attract)
			{
				m_attract = CrossPlatformInputManager.GetButtonDown(input_R1);
			}
			if (!m_repulse)
			{
				m_repulse = CrossPlatformInputManager.GetButtonDown(input_L1);
			}

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                //StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
				}
				if (m_attract)
				{
					//PlayJumpSound();
					m_attract = false;
					Debug.Log (input_R1);
					//m_Jumping = true;
				}
				if (m_repulse)
				{
					//PlayJumpSound();
					m_repulse = false;
					Debug.Log (input_L1);
					//m_Jumping = true;
				}
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            //m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            /*if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;*/
        }


        private void GetInput(out float speed)
        {
            // Read input
			//float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
			//float vertical = CrossPlatformInputManager.GetAxis("Vertical");
			float horizontal = CrossPlatformInputManager.GetAxis(input_LSTICKH);
			float vertical = CrossPlatformInputManager.GetAxis(input_LSTICKV);

            /*bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            //m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif*/
            // set the desired speed to be walking or running
			speed = m_WalkSpeed;//m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            /*if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }*/
        }

		/*public static float P1RHorizontal(){
			float r = 0.0f;
			r += Input.GetAxis ("P1-RStickHorizontal");
			return Mathf.Clamp (r, -1.0f, 1.0f);
		}
		public static float P1RVertical(){
			float r = 0.0f;
			r += Input.GetAxis ("P1-RStickVertical");
			return Mathf.Clamp (r, -1.0f, 1.0f);
		}*/
        private void RotateView()
        {
			float new_x = m_CharacterController.transform.eulerAngles.x + Input.GetAxis (input_RSTICKV) * Time.deltaTime*m_RotationSpeed;//P1RVertical ();//;
			//Debug.Log (new_x);
			if (new_x > 60 && new_x < 180){
				new_x = 60;
			}
			if (new_x < -60 || (new_x < 300 && new_x > 180)) {
				new_x = -60;
			}
			float new_y = m_CharacterController.transform.eulerAngles.y + Input.GetAxis (input_RSTICKH)*Time.deltaTime*m_RotationSpeed;//P1RHorizontal ();//
			m_CharacterController.transform.eulerAngles = new Vector3 (new_x, new_y, m_CharacterController.transform.eulerAngles.z);
            //m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
