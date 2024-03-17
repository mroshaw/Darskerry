using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.AI
{
	public class BirdAiDemo : MonoBehaviour
	{
		[BoxGroup("Settings")] public BirdAiController BirdAiControl;
        [BoxGroup("Settings")] public Camera camera1;
        [BoxGroup("Settings")] public Camera camera2;

		private Camera _currentCamera;
        private bool _cameraDirections = true;
        private Ray _ray;
        private RaycastHit[] _hits;

		/// <summary>
		/// Set up demo component
		/// </summary>
		private void Start()
		{
			_currentCamera = Camera.main;
			BirdAiControl = GameObject.Find("_livingBirdsController").GetComponent<BirdAiController>();
			SpawnSomeBirds();
		}

		/// <summary>
		/// Check for key and spawn birds
		/// </summary>
		private void Update()
		{
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				camera1.transform.localEulerAngles += new Vector3(0.0f, 90.0f, 0.0f) * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				camera1.transform.localEulerAngles -= new Vector3(0.0f, 90.0f, 0.0f) * Time.deltaTime;
			}
			if (Input.GetMouseButtonDown(0))
			{
				_ray = _currentCamera.ScreenPointToRay(Input.mousePosition);
				_hits = Physics.RaycastAll(_ray);
				foreach (RaycastHit hit in _hits)
				{
					if (hit.collider.tag == "lb_bird")
					{
						hit.transform.SendMessage("KillBirdWithForce", _ray.direction * 500);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Editor GUI
		/// </summary>
		private void OnGUI()
		{
			if (GUI.Button(new Rect(10, 10, 150, 50), "Pause"))
				BirdAiControl.SendMessage("Pause");

			if (GUI.Button(new Rect(10, 70, 150, 30), "Scare All"))
				BirdAiControl.SendMessage("AllFlee");

			if (GUI.Button(new Rect(10, 110, 150, 50), "Change Camera"))
				ChangeCamera();

			if (GUI.Button(new Rect(10, 170, 150, 50), "Revive Birds"))
				BirdAiControl.BroadcastMessage("Revive");


			if (_cameraDirections)
			{
				GUI.Label(new Rect(170, 10, 1014, 20), "version 1.1");
				GUI.Label(new Rect(170, 30, 1014, 20), "USE ARROW KEYS TO PAN THE CAMERA");
				GUI.Label(new Rect(170, 50, 1014, 20), "Click a bird to kill it, you monster.");
			}
		}

		/// <summary>
		/// Async method to spawn birds
		/// </summary>
		/// <returns></returns>
		private IEnumerator SpawnSomeBirds()
		{
			yield return 2;
			BirdAiControl.SendMessage("SpawnAmount", 10);
		}

		/// <summary>
		/// Change the camera
		/// </summary>
		private void ChangeCamera()
		{
			if (camera2.gameObject.activeSelf)
			{
				camera1.gameObject.SetActive(true);
				camera2.gameObject.SetActive(false);
				BirdAiControl.SendMessage("ChangeCamera", camera1);
				_cameraDirections = true;
				_currentCamera = camera1;
			}
			else
			{
				camera1.gameObject.SetActive(false);
				camera2.gameObject.SetActive(true);
				BirdAiControl.SendMessage("ChangeCamera", camera2);
				_cameraDirections = false;
				_currentCamera = camera2;
			}
		}
	}
}