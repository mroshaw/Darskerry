using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace DaftAppleGames.Common.AI
{
    public class BirdAi : MonoBehaviour
    {
        private enum birdBehaviors
        {
            sing,
            preen,
            ruffle,
            peck,
            hopForward,
            hopBackward,
            hopLeft,
            hopRight,
        }

        [BoxGroup("Parent Controller")] public BirdAiController AiController;

        [BoxGroup("Audio Settings")] public AudioClip song1;
        [BoxGroup("Audio Settings")] public AudioClip song2;
        [BoxGroup("Audio Settings")] public AudioClip flyAway1;
        [BoxGroup("Audio Settings")] public AudioClip flyAway2;

        [BoxGroup("Behaviour Settings")]  public bool fleeCrows = true;

        private Animator _anim;


        private bool _paused = false;
        private bool _idle = true;
        private bool _flying = false;
        private bool _landing = false;
        private bool _perched = false;
        private bool _onGround = true;
        private bool _dead = false;
        private BoxCollider _birdCollider;
        private Vector3 _bColCenter;
        private Vector3 _bColSize;
        private SphereCollider _solidCollider;
        private float _distanceToTarget = 0.0f;
        private float _agitationLevel = .5f;
        private float _originalAnimSpeed = 1.0f;
        private Vector3 _originalVelocity = Vector3.zero;

        //hash variables for the animation states and animation properties
        private int _idleAnimationHash;
        private int _singAnimationHash;
        private int _ruffleAnimationHash;
        private int _preenAnimationHash;
        private int _peckAnimationHash;
        private int _hopForwardAnimationHash;
        private int _hopBackwardAnimationHash;
        private int _hopLeftAnimationHash;
        private int _hopRightAnimationHash;
        private int _worriedAnimationHash;
        private int _landingAnimationHash;
        private int _flyAnimationHash;
        private int _hopIntHash;

        private int _flyingBoolHash;

        //int perchedBoolHash;
        private int _peckBoolHash;
        private int _ruffleBoolHash;

        private int _preenBoolHash;

        //int worriedBoolHash;
        private int _landingBoolHash;
        private int _singTriggerHash;
        private int _flyingDirectionHash;
        private int _dieTriggerHash;

        /// <summary>
        /// Init component on enable
        /// </summary>
        private void OnEnable()
        {
            _birdCollider = gameObject.GetComponent<BoxCollider>();
            _bColCenter = _birdCollider.center;
            _bColSize = _birdCollider.size;
            _solidCollider = gameObject.GetComponent<SphereCollider>();
            _anim = gameObject.GetComponent<Animator>();

            _idleAnimationHash = Animator.StringToHash("Base Layer.Idle");
            //singAnimationHash = Animator.StringToHash ("Base Layer.sing");
            //ruffleAnimationHash = Animator.StringToHash ("Base Layer.ruffle");
            //preenAnimationHash = Animator.StringToHash ("Base Layer.preen");
            //peckAnimationHash = Animator.StringToHash ("Base Layer.peck");
            //hopForwardAnimationHash = Animator.StringToHash ("Base Layer.hopForward");
            //hopBackwardAnimationHash = Animator.StringToHash ("Base Layer.hopBack");
            //hopLeftAnimationHash = Animator.StringToHash ("Base Layer.hopLeft");
            //hopRightAnimationHash = Animator.StringToHash ("Base Layer.hopRight");
            //worriedAnimationHash = Animator.StringToHash ("Base Layer.worried");
            //landingAnimationHash = Animator.StringToHash ("Base Layer.landing");
            _flyAnimationHash = Animator.StringToHash("Base Layer.fly");
            _hopIntHash = Animator.StringToHash("hop");
            _flyingBoolHash = Animator.StringToHash("flying");
            //perchedBoolHash = Animator.StringToHash("perched");
            _peckBoolHash = Animator.StringToHash("peck");
            _ruffleBoolHash = Animator.StringToHash("ruffle");
            _preenBoolHash = Animator.StringToHash("preen");
            //worriedBoolHash = Animator.StringToHash("worried");
            _landingBoolHash = Animator.StringToHash("landing");
            _singTriggerHash = Animator.StringToHash("sing");
            _flyingDirectionHash = Animator.StringToHash("flyingDirectionX");
            _dieTriggerHash = Animator.StringToHash("die");
            _anim.SetFloat("IdleAgitated", _agitationLevel);
            if (_dead)
            {
                Revive();
            }
        }

        /// <summary>
        /// Pauses the bird
        /// </summary>
        private void PauseBird()
        {
            if (!_dead)
            {
                _originalAnimSpeed = _anim.speed;
                _anim.speed = 0;
                if (!GetComponent<Rigidbody>().isKinematic)
                {
                    _originalVelocity = GetComponent<Rigidbody>().velocity;
                }

                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<AudioSource>().Stop();
                _paused = true;
            }
        }

        /// <summary>
        /// Unpauses the bird
        /// </summary>
        private void UnPauseBird()
        {
            if (!_dead)
            {
                _anim.speed = _originalAnimSpeed;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().velocity = _originalVelocity;
                _paused = false;
            }
        }

        /// <summary>
        /// Sends the bird flying to the given position
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private IEnumerator FlyToTarget(Vector3 target)
        {
            if (Random.value < .5)
            {
                GetComponent<AudioSource>().PlayOneShot(flyAway1, .1f);
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(flyAway2, .1f);
            }

            _flying = true;
            _landing = false;
            _onGround = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().drag = 0.5f;
            _anim.applyRootMotion = false;
            _anim.SetBool(_flyingBoolHash, true);
            _anim.SetBool(_landingBoolHash, false);

            // Wait to apply velocity until the bird is entering the flying animation
            while (_anim.GetCurrentAnimatorStateInfo(0).nameHash != _flyAnimationHash)
            {
                yield return 0;
            }

            // Birds fly up and away from their perch for 1 second before orienting to the next target
            GetComponent<Rigidbody>().AddForce((transform.forward * 50.0f * AiController.birdScale) +
                                               (transform.up * 100.0f * AiController.birdScale));
            float t = 0.0f;
            while (t < 1.0f)
            {
                if (!_paused)
                {
                    t += Time.deltaTime;
                    if (t > .2f && !_solidCollider.enabled && AiController.collideWithObjects)
                    {
                        _solidCollider.enabled = true;
                    }
                }

                yield return 0;
            }

            // Start to rotate toward target
            Vector3 vectorDirectionToTarget = (target - transform.position).normalized;
            Quaternion finalRotation = Quaternion.identity;
            Quaternion startingRotation = transform.rotation;
            _distanceToTarget = Vector3.Distance(transform.position, target);
            Vector3 forwardStraight; //the forward vector on the xz plane
            RaycastHit hit;
            Vector3 tempTarget = target;
            t = 0.0f;

            // If the target is directly above the bird the bird needs to fly out before going up
            // this should stop them from taking off like a rocket upwards
            if (vectorDirectionToTarget.y > .5f)
            {
                tempTarget = transform.position +
                             (new Vector3(transform.forward.x, .5f, transform.forward.z) * _distanceToTarget);

                while (vectorDirectionToTarget.y > .5f)
                {
                    //Debug.DrawLine (tempTarget,tempTarget+Vector3.up,Color.red);
                    vectorDirectionToTarget = (tempTarget - transform.position).normalized;
                    finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
                    transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t);
                    _anim.SetFloat(_flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
                    t += Time.deltaTime * 0.5f;
                    GetComponent<Rigidbody>()
                        .AddForce(transform.forward * 70.0f * AiController.birdScale * Time.deltaTime);

                    //Debug.DrawRay (transform.position,transform.forward,Color.green);

                    vectorDirectionToTarget =
                        (target - transform.position)
                        .normalized; //reset the variable to reflect the actual target and not the temptarget

                    if (Physics.Raycast(transform.position, -Vector3.up, out hit, 0.15f * AiController.birdScale) &&
                        GetComponent<Rigidbody>().velocity.y < 0)
                    {
                        // If the bird is going to collide with the ground zero out vertical velocity
                        if (!hit.collider.isTrigger)
                        {
                            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f,
                                GetComponent<Rigidbody>().velocity.z);
                        }
                    }

                    if (Physics.Raycast(transform.position, Vector3.up, out hit, 0.15f * AiController.birdScale) &&
                        GetComponent<Rigidbody>().velocity.y > 0)
                    {
                        // If the bird is going to collide with something overhead zero out vertical velocity
                        if (!hit.collider.isTrigger)
                        {
                            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f,
                                GetComponent<Rigidbody>().velocity.z);
                        }
                    }

                    // Check for collisions with non trigger colliders and abort flight if necessary
                    if (AiController.collideWithObjects)
                    {
                        forwardStraight = transform.forward;
                        forwardStraight.y = 0.0f;
                        //Debug.DrawRay (transform.position+(transform.forward*.1f),forwardStraight*.75f,Color.green);
                        if (Physics.Raycast(transform.position + (transform.forward * .15f * AiController.birdScale),
                                forwardStraight, out hit, .75f * AiController.birdScale))
                        {
                            if (!hit.collider.isTrigger)
                            {
                                AbortFlyToTarget();
                            }
                        }
                    }

                    yield return null;
                }
            }

            finalRotation = Quaternion.identity;
            startingRotation = transform.rotation;
            _distanceToTarget = Vector3.Distance(transform.position, target);

            // Rotate the bird toward the target over time
            while (transform.rotation != finalRotation || _distanceToTarget >= 1.5f)
            {
                if (!_paused)
                {
                    _distanceToTarget = Vector3.Distance(transform.position, target);
                    vectorDirectionToTarget = (target - transform.position).normalized;
                    if (vectorDirectionToTarget == Vector3.zero)
                    {
                        vectorDirectionToTarget = new Vector3(0.0001f, 0.00001f, 0.00001f);
                    }

                    finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
                    transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t);
                    _anim.SetFloat(_flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
                    t += Time.deltaTime * 0.5f;
                    GetComponent<Rigidbody>()
                        .AddForce(transform.forward * 70.0f * AiController.birdScale * Time.deltaTime);
                    if (Physics.Raycast(transform.position, -Vector3.up, out hit, 0.15f * AiController.birdScale) &&
                        GetComponent<Rigidbody>().velocity.y < 0)
                    {
                        // If the bird is going to collide with the ground zero out vertical velocity
                        if (!hit.collider.isTrigger)
                        {
                            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f,
                                GetComponent<Rigidbody>().velocity.z);
                        }
                    }

                    if (Physics.Raycast(transform.position, Vector3.up, out hit, 0.15f * AiController.birdScale) &&
                        GetComponent<Rigidbody>().velocity.y > 0)
                    {
                        // If the bird is going to collide with something overhead zero out vertical velocity
                        if (!hit.collider.isTrigger)
                        {
                            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f,
                                GetComponent<Rigidbody>().velocity.z);
                        }
                    }

                    // Check for collisions with non trigger colliders and abort flight if necessary
                    if (AiController.collideWithObjects)
                    {
                        forwardStraight = transform.forward;
                        forwardStraight.y = 0.0f;
                        //Debug.DrawRay (transform.position+(transform.forward*.1f),forwardStraight*.75f,Color.green);
                        if (Physics.Raycast(transform.position + (transform.forward * .15f * AiController.birdScale),
                                forwardStraight, out hit, .75f * AiController.birdScale))
                        {
                            if (!hit.collider.isTrigger)
                            {
                                AbortFlyToTarget();
                            }
                        }
                    }
                }

                yield return 0;
            }

            // Keep the bird pointing at the target and move toward it
            float flyingForce = 50.0f * AiController.birdScale;
            while (true)
            {
                if (!_paused)
                {
                    // Do a raycast to see if the bird is going to hit the ground
                    if (Physics.Raycast(transform.position, -Vector3.up, 0.15f * AiController.birdScale) &&
                        GetComponent<Rigidbody>().velocity.y < 0)
                    {
                        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f,
                            GetComponent<Rigidbody>().velocity.z);
                    }

                    if (Physics.Raycast(transform.position, Vector3.up, out hit, 0.15f * AiController.birdScale) &&
                        GetComponent<Rigidbody>().velocity.y > 0)
                    {
                        // If the bird is going to collide with something overhead zero out vertical velocity
                        if (!hit.collider.isTrigger)
                        {
                            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f,
                                GetComponent<Rigidbody>().velocity.z);
                        }
                    }

                    // Check for collisions with non trigger colliders and abort flight if necessary
                    if (AiController.collideWithObjects)
                    {
                        forwardStraight = transform.forward;
                        forwardStraight.y = 0.0f;
                        // Debug.DrawRay (transform.position+(transform.forward*.1f),forwardStraight*.75f,Color.green);
                        if (Physics.Raycast(transform.position + (transform.forward * .15f * AiController.birdScale),
                                forwardStraight, out hit, .75f * AiController.birdScale))
                        {
                            if (!hit.collider.isTrigger)
                            {
                                AbortFlyToTarget();
                            }
                        }
                    }

                    vectorDirectionToTarget = (target - transform.position).normalized;
                    finalRotation = Quaternion.LookRotation(vectorDirectionToTarget);
                    _anim.SetFloat(_flyingDirectionHash, FindBankingAngle(transform.forward, vectorDirectionToTarget));
                    transform.rotation = finalRotation;
                    GetComponent<Rigidbody>().AddForce(transform.forward * flyingForce * Time.deltaTime);
                    _distanceToTarget = Vector3.Distance(transform.position, target);
                    if (_distanceToTarget <= 1.5f * AiController.birdScale)
                    {
                        _solidCollider.enabled = false;
                        if (_distanceToTarget < 0.5f * AiController.birdScale)
                        {
                            break;
                        }
                        else
                        {
                            GetComponent<Rigidbody>().drag = 2.0f;
                            flyingForce = 50.0f * AiController.birdScale;
                        }
                    }
                    else if (_distanceToTarget <= 5.0f * AiController.birdScale)
                    {
                        GetComponent<Rigidbody>().drag = 1.0f;
                        flyingForce = 50.0f * AiController.birdScale;
                    }
                }

                yield return 0;
            }

            _anim.SetFloat(_flyingDirectionHash, 0);
            // Initiate the landing for the bird to finally reach the target
            Vector3 vel = Vector3.zero;
            _flying = false;
            _landing = true;
            _solidCollider.enabled = false;
            _anim.SetBool(_landingBoolHash, true);
            _anim.SetBool(_flyingBoolHash, false);
            t = 0.0f;
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            // Tell any birds that are in the way to move their butts
            Collider[] hitColliders = Physics.OverlapSphere(target, 0.05f * AiController.birdScale);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag == "Bird" && hitColliders[i].transform != transform)
                {
                    hitColliders[i].SendMessage("FlyAway");
                }
            }

            // This while loop will reorient the rotation to vertical and translate the bird exactly to the target
            startingRotation = transform.rotation;
            transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
            finalRotation = transform.rotation;
            transform.rotation = startingRotation;
            while (_distanceToTarget > 0.05f * AiController.birdScale)
            {
                if (!_paused)
                {
                    transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t * 4.0f);
                    transform.position = Vector3.SmoothDamp(transform.position, target, ref vel, 0.5f);
                    t += Time.deltaTime;
                    _distanceToTarget = Vector3.Distance(transform.position, target);
                    if (t > 2.0f)
                    {
                        break; // Failsafe to stop birds from getting stuck
                    }
                }

                yield return 0;
            }

            GetComponent<Rigidbody>().drag = .5f;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            _anim.SetBool(_landingBoolHash, false);
            _landing = false;
            transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
            transform.position = target;
            _anim.applyRootMotion = true;
            _onGround = true;
        }

        /// <summary>
        /// Sets a variable between -1 and 1 to control the left and right banking animation
        /// </summary>
        /// <param name="birdForward"></param>
        /// <param name="dirToTarget"></param>
        /// <returns></returns>
        private float FindBankingAngle(Vector3 birdForward, Vector3 dirToTarget)
        {
            Vector3 cr = Vector3.Cross(birdForward, dirToTarget);
            float ang = Vector3.Dot(cr, Vector3.up);
            return ang;
        }

        /// <summary>
        /// Set appropriate ground behaviour / state for the bird
        /// </summary>
        private void OnGroundBehaviors()
        {
            _idle = _anim.GetCurrentAnimatorStateInfo(0).nameHash == _idleAnimationHash;
            if (!GetComponent<Rigidbody>().isKinematic)
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }

            if (_idle)
            {
                // The bird is in the idle animation, lets randomly choose a behavior every 3 seconds
                if (Random.value < Time.deltaTime * .33)
                {
                    // Bird will display a behavior
                    // in the perched state the bird can only sing, preen, or ruffle
                    float rand = Random.value;
                    if (rand < .3)
                    {
                        DisplayBehavior(birdBehaviors.sing);
                    }
                    else if (rand < .5)
                    {
                        DisplayBehavior(birdBehaviors.peck);
                    }
                    else if (rand < .6)
                    {
                        DisplayBehavior(birdBehaviors.preen);
                    }
                    else if (!_perched && rand < .7)
                    {
                        DisplayBehavior(birdBehaviors.ruffle);
                    }
                    else if (!_perched && rand < .85)
                    {
                        DisplayBehavior(birdBehaviors.hopForward);
                    }
                    else if (!_perched && rand < .9)
                    {
                        DisplayBehavior(birdBehaviors.hopLeft);
                    }
                    else if (!_perched && rand < .95)
                    {
                        DisplayBehavior(birdBehaviors.hopRight);
                    }
                    else if (!_perched && rand <= 1)
                    {
                        DisplayBehavior(birdBehaviors.hopBackward);
                    }
                    else
                    {
                        DisplayBehavior(birdBehaviors.sing);
                    }

                    // Lets alter the agitation level of the bird so it uses a different mix of idle animation next time
                    _anim.SetFloat("IdleAgitated", Random.value);
                }

                // Birds should fly to a new target about every 10 seconds
                if (Random.value < Time.deltaTime * .1)
                {
                    FlyAway();
                }
            }
        }

        /// <summary>
        /// Apply a behaviour state to the bird
        /// </summary>
        /// <param name="behavior"></param>
        private void DisplayBehavior(birdBehaviors behavior)
        {
            _idle = false;
            switch (behavior)
            {
                case birdBehaviors.sing:
                    _anim.SetTrigger(_singTriggerHash);
                    break;
                case birdBehaviors.ruffle:
                    _anim.SetTrigger(_ruffleBoolHash);
                    break;
                case birdBehaviors.preen:
                    _anim.SetTrigger(_preenBoolHash);
                    break;
                case birdBehaviors.peck:
                    _anim.SetTrigger(_peckBoolHash);
                    break;
                case birdBehaviors.hopForward:
                    _anim.SetInteger(_hopIntHash, 1);
                    break;
                case birdBehaviors.hopLeft:
                    _anim.SetInteger(_hopIntHash, -2);
                    break;
                case birdBehaviors.hopRight:
                    _anim.SetInteger(_hopIntHash, 2);
                    break;
                case birdBehaviors.hopBackward:
                    _anim.SetInteger(_hopIntHash, -1);
                    break;
            }
        }

        /// <summary>
        /// Handle collision between birds
        /// </summary>
        /// <param name="col"></param>
        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Bird")
            {
                FlyAway();
            }
        }

        /// <summary>
        /// Handling flying away from target
        /// </summary>
        /// <param name="col"></param>
        private void OnTriggerExit(Collider col)
        {
            //if bird has hopped out of the target area lets fly
            if (_onGround && (col.tag == "BirdGroundTarget" || col.tag == "BirdPerchTarget"))
            {
                FlyAway();
            }
        }

        /// <summary>
        /// Abort flying
        /// </summary>
        private void AbortFlyToTarget()
        {
            StopCoroutine("FlyToTarget");
            _solidCollider.enabled = false;
            _anim.SetBool(_landingBoolHash, false);
            _anim.SetFloat(_flyingDirectionHash, 0);
            transform.localEulerAngles = new Vector3(
                0.0f,
                transform.localEulerAngles.y,
                0.0f);
            FlyAway();
        }

        /// <summary>
        /// Fly away behaviour
        /// </summary>
        private void FlyAway()
        {
            if (!_dead)
            {
                StopCoroutine("FlyToTarget");
                _anim.SetBool(_landingBoolHash, false);
                AiController.SendMessage("BirdFindTarget", gameObject);
            }
        }

        /// <summary>
        /// Flee behaviour
        /// </summary>
        private void Flee()
        {
            if (!_dead)
            {
                StopCoroutine("FlyToTarget");
                GetComponent<AudioSource>().Stop();
                _anim.Play(_flyAnimationHash);
                Vector3 farAwayTarget = transform.position;
                farAwayTarget += new Vector3(Random.Range(-100, 100) * AiController.birdScale, 10 * AiController.birdScale,
                    Random.Range(-100, 100) * AiController.birdScale);
                StartCoroutine("FlyToTarget", farAwayTarget);
            }
        }

        /// <summary>
        /// Handle behaviour when crow is close
        /// </summary>
        private void CrowIsClose()
        {
            if (fleeCrows && !_dead)
            {
                Flee();
            }
        }

        /// <summary>
        /// Public method to kill bird
        /// </summary>
        public void KillBird()
        {
            if (!_dead)
            {
                AiController.SendMessage("FeatherEmit", transform.position);
                _anim.SetTrigger(_dieTriggerHash);
                _anim.applyRootMotion = false;
                _dead = true;
                _onGround = false;
                _flying = false;
                _landing = false;
                _idle = false;
                _perched = false;
                AbortFlyToTarget();
                StopAllCoroutines();
                GetComponent<Collider>().isTrigger = false;
                _birdCollider.center = new Vector3(0.0f, 0.0f, 0.0f);
                _birdCollider.size = new Vector3(0.1f, 0.01f, 0.1f) * AiController.birdScale;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().useGravity = true;
            }
        }

        /// <summary>
        /// Public method to kill bird with force
        /// </summary>
        /// <param name="force"></param>
        public void KillBirdWithForce(Vector3 force)
        {
            if (!_dead)
            {
                AiController.SendMessage("FeatherEmit", transform.position);
                _anim.SetTrigger(_dieTriggerHash);
                _anim.applyRootMotion = false;
                _dead = true;
                _onGround = false;
                _flying = false;
                _landing = false;
                _idle = false;
                _perched = false;
                AbortFlyToTarget();
                StopAllCoroutines();
                GetComponent<Collider>().isTrigger = false;
                _birdCollider.center = new Vector3(0.0f, 0.0f, 0.0f);
                _birdCollider.size = new Vector3(0.1f, 0.01f, 0.1f) * AiController.birdScale;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(force);
            }
        }

        /// <summary>
        /// Revive a dead bird
        /// </summary>
        private void Revive()
        {
            if (_dead)
            {
                _birdCollider.center = _bColCenter;
                _birdCollider.size = _bColSize;
                GetComponent<Collider>().isTrigger = true;
                _dead = false;
                _onGround = false;
                _flying = false;
                _landing = false;
                _idle = true;
                _perched = false;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().useGravity = false;
                _anim.Play(_idleAnimationHash);
                AiController.SendMessage("BirdFindTarget", gameObject);
            }
        }

        /// <summary>
        /// Update the Animator Controller on the bird
        /// </summary>
        /// <param name="cont"></param>
        private void SetController(BirdAiController cont)
        {
            AiController = cont;
        }

        /// <summary>
        /// Set the hop animation tag hash
        /// </summary>
        void ResetHopInt()
        {
            _anim.SetInteger(_hopIntHash, 0);
        }

        /// <summary>
        /// Resets the flying / landing status
        /// </summary>
        private void ResetFlyingLandingVariables()
        {
            if (_flying || _landing)
            {
                _flying = false;
                _landing = false;
            }
        }

        /// <summary>
        /// Plays a song
        /// </summary>
        private void PlaySong()
        {
            if (!_dead)
            {
                if (Random.value < .5)
                {
                    GetComponent<AudioSource>().PlayOneShot(song1, 1);
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(song2, 1);
                }
            }
        }

        /// <summary>
        /// Update behaviours on update
        /// </summary>
        private void Update()
        {
            if (_onGround && !_paused && !_dead)
            {
                OnGroundBehaviors();
            }
        }
    }
}