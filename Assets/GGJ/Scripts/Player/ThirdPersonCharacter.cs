using GGJ.Scripts;
using GGJ.Scripts.ScriptableObjects;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(AudioSource))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		
		private float m_MoveSpeedMultiplier = 1f;
		private Rigidbody m_Rigidbody;
		private float m_ForwardAmount;

		private AudioSource _audioSource;
		private GameObject _speachBubble;
		private GameObject _currentSymbol;
		private SpriteRenderer _currentSymbolSpriteRenderer;
		private SingleNoteConfiguration[] _noteConfiguration;

		private void Start()
		{
			_audioSource = GetComponent<AudioSource>();
			_noteConfiguration = FindObjectOfType<GameManager>().GameSettings.AvailableNotes;
			_speachBubble = transform.Find("SpeakBubble").gameObject;
			_currentSymbol = _speachBubble.transform.Find("CurrentSymbol").gameObject;
			_currentSymbolSpriteRenderer = _currentSymbol.GetComponent<SpriteRenderer>();
			
            var gameManager = FindObjectOfType<GameManager>();
            m_MoveSpeedMultiplier = gameManager.GameSettings.PlayerSpeed;

            m_Rigidbody = GetComponent<Rigidbody>();
			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

			InputController
				.Instance
				.FireChanges.Subscribe(firePressed =>
			{
					_speachBubble.SetActive(true);
					for (var index = 0; index < _noteConfiguration.Length; index++)
					{
						var noteConfig = _noteConfiguration[index];
						if (_noteConfiguration[index].button.Equals(firePressed.ToString()))
						{
							_currentSymbolSpriteRenderer.sprite = noteConfig.Sprite;
							_currentSymbol.SetActive(true);
							_audioSource.PlayOneShot(noteConfig.Note);
						}
					}


			});
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

		public void ShowSpeechBubble()
		{
			_speachBubble.SetActive(true);
		}

		public void HideSpeechBubble()
		{
			_speachBubble.SetActive(false);
		}

		private void FixedUpdate()
		{
			if (_speachBubble.active && _audioSource.isPlaying == false)
			{
				_currentSymbol.SetActive(false);
			}
		}
	}
}
