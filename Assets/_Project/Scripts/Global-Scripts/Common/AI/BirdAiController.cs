using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.AI
{
    public class BirdAiController : MonoBehaviour
    {
        [BoxGroup("Spawn Settings")] public int idealNumberOfBirds;
        [BoxGroup("Spawn Settings")] public int maximumNumberOfBirds;
        [BoxGroup("Spawn Settings")] public Camera currentCamera;
        [BoxGroup("Spawn Settings")] public float unspawnDistance = 10.0f;
        [BoxGroup("Spawn Settings")] public bool highQuality = true;
        [BoxGroup("Spawn Settings")] public float birdScale = 1.0f;
        [BoxGroup("Ground Settings")] public bool collideWithObjects = true;
        [BoxGroup("Ground Settings")] public LayerMask groundLayer;
        [BoxGroup("Bird Prefabs")] public GameObject[] birdPrefabs;
        [BoxGroup("Bird Prefabs")] public GameObject featherEmitterPrefab;
        [BoxGroup("Targets")] public GameObject[] birdGroundTargets;
        [BoxGroup("Targets")] public GameObject[] birdPerchTargets;

        private List<GameObject> _validGroundTargets = new List<GameObject>();
        private List<GameObject> _validPerchTargets = new List<GameObject>();

        [Button("Refresh")]
        private void RefreshPerchTargets()
        {
            UpdatePerchTargets();
        }

        private bool _pause = false;
        private GameObject[] _myBirds;

        private int _activeBirds = 0;
        private int _birdIndex = 0;
        private GameObject[] _featherEmitters = new GameObject[3];

        /// <summary>
        /// Configure the component
        /// </summary>
        private void Start()
        {
            // Find the camera, if not set
            if (currentCamera == null)
            {
                currentCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            }

            if (idealNumberOfBirds >= maximumNumberOfBirds)
            {
                idealNumberOfBirds = maximumNumberOfBirds - 1;
            }

            UpdatePerchTargets();

            // Instantiate birds based on amounts and bird types
            _myBirds = new GameObject[maximumNumberOfBirds];
            GameObject bird;
            for (int i = 0; i < _myBirds.Length; i++)
            {
                bird = birdPrefabs[Random.Range(0, birdPrefabs.Length)];
    
                _myBirds[i] = Instantiate(bird, Vector3.zero, Quaternion.identity) as GameObject;
                _myBirds[i].transform.localScale = _myBirds[i].transform.localScale * birdScale;
                _myBirds[i].transform.parent = transform;
                _myBirds[i].SendMessage("SetController", this);
                _myBirds[i].SetActive(false);
            }

            // Find all the valid targets
            for (int i = 0; i < birdGroundTargets.Length; i++)
            {
                if (Vector3.Distance(birdGroundTargets[i].transform.position, currentCamera.transform.position) <
                    unspawnDistance)
                {
                    _validGroundTargets.Add(birdGroundTargets[i]);
                }
            }

            for (int i = 0; i < birdPerchTargets.Length; i++)
            {
                if (Vector3.Distance(birdPerchTargets[i].transform.position, currentCamera.transform.position) <
                    unspawnDistance)
                {
                    _validPerchTargets.Add(birdPerchTargets[i]);
                }
            }

            // Instantiate 3 feather emitters for killing the birds
            for (int i = 0; i < 3; i++)
            {
                _featherEmitters[i] = Instantiate(featherEmitterPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                _featherEmitters[i].transform.parent = transform;
                _featherEmitters[i].SetActive(false);
            }
        }

        /// <summary>
        /// Get all bird perch targets across scenes
        /// </summary>
        private void UpdatePerchTargets()
        {
            birdGroundTargets = GameObject.FindGameObjectsWithTag("BirdGroundTarget");
            birdPerchTargets = GameObject.FindGameObjectsWithTag("BirdPerchTarget");
        }

        /// <summary>
        /// Set up spawning on enable
        /// </summary>
        void OnEnable()
        {
            InvokeRepeating("UpdateBirds", 1, 1);
            StartCoroutine("UpdateTargets");
        }


        /// <summary>
        /// Cause all spawned birds to flee
        /// </summary>
        public void AllFlee()
        {
            if (!_pause)
            {
                for (int i = 0; i < _myBirds.Length; i++)
                {
                    if (_myBirds[i].activeSelf)
                    {
                        _myBirds[i].SendMessage("Flee");
                    }
                }
            }
        }

        /// <summary>
        /// Pause spawning birds
        /// </summary>
        public void Pause()
        {
            if (_pause)
            {
                AllUnPause();
            }
            else
            {
                AllPause();
            }
        }

        /// <summary>
        /// Pause spawning and pause all active birds
        /// </summary>
        public void AllPause()
        {
            _pause = true;
            for (int i = 0; i < _myBirds.Length; i++)
            {
                if (_myBirds[i].activeSelf)
                {
                    _myBirds[i].SendMessage("PauseBird");
                }
            }
        }

        /// <summary>
        /// Unpause all active birds and resume spawning
        /// </summary>
        public void AllUnPause()
        {
            _pause = false;
            for (int i = 0; i < _myBirds.Length; i++)
            {
                if (_myBirds[i].activeSelf)
                {
                    _myBirds[i].SendMessage("UnPauseBird");
                }
            }
        }

        /// <summary>
        /// Spawn the given number of birds
        /// </summary>
        /// <param name="amt"></param>
        public void SpawnAmount(int amt)
        {
            for (int i = 0; i <= amt; i++)
            {
                SpawnBird();
            }
        }

        /// <summary>
        /// Public method to change the spawn camera
        /// </summary>
        /// <param name="cam"></param>
        public void ChangeCamera(Camera cam)
        {
            currentCamera = cam;
        }
 
        /// <summary>
        /// Find a suitable landing point on given GameObject
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private Vector3 FindPointInGroundTarget(GameObject target)
        {
            //find a random point within the collider of a ground target that touches the ground
            Vector3 point;
            point.x = Random.Range(target.GetComponent<Collider>().bounds.max.x,
                target.GetComponent<Collider>().bounds.min.x);
            point.y = target.GetComponent<Collider>().bounds.max.y;
            point.z = Random.Range(target.GetComponent<Collider>().bounds.max.z,
                target.GetComponent<Collider>().bounds.min.z);
            //raycast down until it hits the ground
            RaycastHit hit;
            if (Physics.Raycast(point, -Vector3.up, out hit, target.GetComponent<Collider>().bounds.size.y,
                    groundLayer))
            {
                return hit.point;
            }

            return point;
        }

        /// <summary>
        /// Update birds in frame update
        /// </summary>
        private void UpdateBirds()
        {
            // This function is called once a second
            if (_activeBirds < idealNumberOfBirds && AreThereActiveTargets())
            {
                // If there are less than ideal birds active, spawn a bird
                SpawnBird();
            }
            else if (_activeBirds < maximumNumberOfBirds && Random.value < .05 && AreThereActiveTargets())
            {
                // If there are less than maximum birds active spawn a bird every 20 seconds
                SpawnBird();
            }

            // Check one bird every second to see if it should be unspawned
            if (_myBirds[_birdIndex].activeSelf && BirdOffCamera(_myBirds[_birdIndex].transform.position) &&
                Vector3.Distance(_myBirds[_birdIndex].transform.position, currentCamera.transform.position) >
                unspawnDistance)
            {
                // If the bird is off camera and at least unsapwnDistance units away lets unspawn
                Unspawn(_myBirds[_birdIndex]);
            }

            _birdIndex = _birdIndex == _myBirds.Length - 1 ? 0 : _birdIndex + 1;
        }

        /// <summary>
        /// Cycle through targets removing those outside of the unspawnDistance
        /// and add any new targets that come into range
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateTargets()
        {
            List<GameObject> gtRemove = new List<GameObject>();
            List<GameObject> ptRemove = new List<GameObject>();

            while (true)
            {
                gtRemove.Clear();
                ptRemove.Clear();
                //check targets to see if they are out of range
                for (int i = 0; i < _validGroundTargets.Count; i++)
                {
                    if (Vector3.Distance(_validGroundTargets[i].transform.position, currentCamera.transform.position) >
                        unspawnDistance)
                    {
                        gtRemove.Add(_validGroundTargets[i]);
                    }

                    yield return 0;
                }

                for (int i = 0; i < _validPerchTargets.Count; i++)
                {
                    if (Vector3.Distance(_validPerchTargets[i].transform.position, currentCamera.transform.position) >
                        unspawnDistance)
                    {
                        ptRemove.Add(_validPerchTargets[i]);
                    }

                    yield return 0;
                }

                // Remove any targets that have been found out of range
                foreach (GameObject entry in gtRemove)
                {
                    _validGroundTargets.Remove(entry);
                }

                foreach (GameObject entry in ptRemove)
                {
                    _validPerchTargets.Remove(entry);
                }

                yield return 0;
                // Now check for any new Targets
                Collider[] hits = Physics.OverlapSphere(currentCamera.transform.position, unspawnDistance);
                foreach (Collider hit in hits)
                {
                    if (hit.tag == "BirdGroundTarget" && !_validGroundTargets.Contains(hit.gameObject))
                    {
                        _validGroundTargets.Add(hit.gameObject);
                    }

                    if (hit.tag == "BirdPerchTarget" && !_validPerchTargets.Contains(hit.gameObject))
                    {
                        _validPerchTargets.Add(hit.gameObject);
                    }
                }

                yield return 0;
            }
        }

        /// <summary>
        /// Determine if bird is within range of the camera
        /// </summary>
        /// <param name="birdPos"></param>
        /// <returns></returns>
        private bool BirdOffCamera(Vector3 birdPos)
        {
            Vector3 screenPos = currentCamera.WorldToViewportPoint(birdPos);
            if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Unspawns the given bird
        /// </summary>
        /// <param name="bird"></param>
        private void Unspawn(GameObject bird)
        {
            bird.transform.position = Vector3.zero;
            bird.SetActive(false);
            _activeBirds--;
        }

        /// <summary>
        /// Spawns a bird
        /// </summary>
        void SpawnBird()
        {
            if (!_pause)
            {
                GameObject bird = null;
                int randomBirdIndex = Mathf.FloorToInt(Random.Range(0, _myBirds.Length));
                int loopCheck = 0;
                //find a random bird that is not active
                while (bird == null)
                {
                    if (_myBirds[randomBirdIndex].activeSelf == false)
                    {
                        bird = _myBirds[randomBirdIndex];
                    }

                    randomBirdIndex = randomBirdIndex + 1 >= _myBirds.Length ? 0 : randomBirdIndex + 1;
                    loopCheck++;
                    if (loopCheck >= _myBirds.Length)
                    {
                        //all birds are active
                        return;
                    }
                }

                //Find a point off camera to positon the bird and activate it
                bird.transform.position = FindPositionOffCamera();
                if (bird.transform.position == Vector3.zero)
                {
                    //couldnt find a suitable spawn point
                    return;
                }
                else
                {
                    bird.SetActive(true);
                    _activeBirds++;
                    BirdFindTarget(bird);
                }
            }
        }

        /// <summary>
        /// Check to see if there are active perch targets
        /// </summary>
        /// <returns></returns>
        private bool AreThereActiveTargets()
        {
            if (_validGroundTargets.Count > 0 || _validPerchTargets.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Find a suitable position off camera in which to spawn a new bird
        /// </summary>
        /// <returns></returns>
        private Vector3 FindPositionOffCamera()
        {
            RaycastHit hit;
            float dist = Random.Range(2, 10);
            Vector3 ray = -currentCamera.transform.forward;
            int loopCheck = 0;
            // Find a random ray pointing away from the cameras field of view
            ray += new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
            // Cycle through random rays until we find one that doesn't hit anything
            while (Physics.Raycast(currentCamera.transform.position, ray, out hit, dist))
            {
                dist = Random.Range(2, 10);
                loopCheck++;
                if (loopCheck > 35)
                {
                    // Can't find any good spawn points so lets cancel
                    return Vector3.zero;
                }
            }

            return currentCamera.transform.position + (ray * dist);
        }

        /// <summary>
        /// Find a suitable perch target for a bird
        /// </summary>
        /// <param name="bird"></param>
        void BirdFindTarget(GameObject bird)
        {
            // yield return new WaitForSeconds(1);
            GameObject target;
            if (_validGroundTargets.Count > 0 || _validPerchTargets.Count > 0)
            {
                // Pick a random target based on the number of available targets vs the area of ground targets
                // each perch target counts for .3 area, each ground target's area is calculated
                float gtArea = 0.0f;
                float ptArea = _validPerchTargets.Count * 0.3f;

                for (int i = 0; i < _validGroundTargets.Count; i++)
                {
                    gtArea += _validGroundTargets[i].GetComponent<Collider>().bounds.size.x *
                              _validGroundTargets[i].GetComponent<Collider>().bounds.size.z;
                }

                if (ptArea == 0.0f || Random.value < gtArea / (gtArea + ptArea))
                {
                    target = _validGroundTargets[Mathf.FloorToInt(Random.Range(0, _validGroundTargets.Count))];
                    bird.SendMessage("FlyToTarget", FindPointInGroundTarget(target));
                }
                else
                {
                    target = _validPerchTargets[Mathf.FloorToInt(Random.Range(0, _validPerchTargets.Count))];
                    bird.SendMessage("FlyToTarget", target.transform.position);
                }
            }
            else
            {
                bird.SendMessage("FlyToTarget",
                    currentCamera.transform.position + new Vector3(Random.Range(-100, 100), Random.Range(5, 10),
                        Random.Range(-100, 100)));
            }
        }

        /// <summary>
        /// Emit feather particles, for when the bird is killed
        /// </summary>
        /// <param name="pos"></param>
        private void FeatherEmit(Vector3 pos)
        {
            foreach (GameObject fEmit in _featherEmitters)
            {
                if (!fEmit.activeSelf)
                {
                    fEmit.transform.position = pos;
                    fEmit.SetActive(true);
                    StartCoroutine("DeactivateFeathers", fEmit);
                    break;
                }
            }
        }

        /// <summary>
        /// Deactivate the feather particles
        /// </summary>
        /// <param name="featherEmit"></param>
        /// <returns></returns>
        IEnumerator DeactivateFeathers(GameObject featherEmit)
        {
            yield return new WaitForSeconds(4.5f);
            featherEmit.SetActive(false);
        }
    }
}