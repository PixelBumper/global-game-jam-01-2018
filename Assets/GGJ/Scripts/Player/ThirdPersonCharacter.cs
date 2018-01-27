using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		private float m_MoveSpeedMultiplier = 1f;
		private Rigidbody m_Rigidbody;
		private float m_ForwardAmount;

		private void Start()
		{
            var gameManager = FindObjectOfType<GameManager>();
            m_MoveSpeedMultiplier = gameManager.GameSettings.PlayerSpeed;

            m_Rigidbody = GetComponent<Rigidbody>();
			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}

		public void Move(Vector3 move)
		{
			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
            m_Rigidbody.velocity = move * m_MoveSpeedMultiplier;
		}
    }
}
