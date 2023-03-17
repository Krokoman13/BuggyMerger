using UnityEngine;

namespace FPSepController
{
	public class SimplePOVCam : MonoBehaviour
	{
		[SerializeField] PlayerInput pInput = null;

		[Header("Input Indexes")]
		[SerializeField, Tooltip("The input axis that'll move the camera horizontally.")]
		int inputAxis_CamX = 2;
		[SerializeField, Tooltip("The input axis that'll move the camera vertically")]
		int inputAxis_CamY = 3;

		[Header("Camera Rotation Values")]
		[SerializeField, Tooltip("Speed at which the mouse will rotate the camera.")]
		float sensitivity = 2f;		
		[SerializeField, Range(0, 90), Tooltip("Clamps the camera as to not have it wrap after looking straight up/down.")]
		float yRotationLimit = 75f;
		
		Vector2 rot = Vector2.zero;     //Cache current rotation.
		Transform t = null;

		void Update()
		{
			DoRotation();	
		}

		void DoRotation()
        {
			Vector2 inputs = GetInputs();	//Get Inputs

			//Apply Rotations based on input * sensitivity
			rot.x += inputs.x * sensitivity;
			rot.y += inputs.y * sensitivity;
			rot.y = Mathf.Clamp(rot.y, -yRotationLimit, yRotationLimit);	//Clamp Vertical look to prevent wrapping.

			//Create Quaternions out of the rotation outcomes.
			Quaternion xQuat = Quaternion.AngleAxis(rot.x, Vector3.up);
			Quaternion yQuat = Quaternion.AngleAxis(rot.y, Vector3.left);

			//Combine these for the final rotation.
			t.localRotation = xQuat * yQuat;
		}

		Vector2 GetInputs()
		{
			return new Vector2(pInput.GetInputAxisValue(inputAxis_CamX), pInput.GetInputAxisValue(inputAxis_CamY));
		}

        void Awake()
        {
			t = this.transform;    
        }
    }
}
