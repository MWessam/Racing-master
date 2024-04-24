using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;
using DG.Tweening;
using Cinemachine;
using AmplifyMotion;
namespace PathCreation.Examples
{
    public class AIPathFollower : MonoBehaviour
    {
        
        public PathCreator pathCreator;
        private EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Stop;
        [HideInInspector]
        public float speed = 5;
        public float MaxSpeed = 48;
        private float acclearionRate = 10f;
        private float logicalSpeed => speed / 2.75f;
        
        [HideInInspector]
        public float distanceTravelled;

        public float startDistance = 0f;

        Vector3 EvaluatedPosition;
        Quaternion rot;
        Quaternion rot1;

        [Range(-1f, 1f)]
        public float deviation;
        private float tobogganWidth = 11;
        private float offsetFromGround = .035f;
        public bool onPath = true;
        [HideInInspector]
        public bool deviationModifAuthorization = true;
        private float speedMultiplicator = 1;
        public float absDeviationAcceleration;



        public float ejectionThresold = .9f;
        private float ejectionForce = 12f;

        [Header("Flying parameters")]


        private float flyingSpeed = 40;
        private float fallSpeed = .525f;

        [HideInInspector]
        public bool istransation;

        public TrailRenderer backLight1;
        public TrailRenderer backLight2;

   

        public ParticleSystem spark;
        public ParticleSystem dropspark;

        public ParticleSystem nos;
        public ParticleSystem dust;

        public ParticleSystem fire;

        [HideInInspector]
        public bool isStart;
        
        public bool isfinish;

        public bool canFly;
        [HideInInspector]
        public Transform Car;

        private float _lastDeviation;
        [HideInInspector]
        public float distanceCovered;

        DistanceCounter distance;

        public bool landBoost;

        public float finishDistanceTravelled;
        private bool _isDestroyed;

        private void Awake()
        {
            Car = transform.GetChild(0);
            distance = GetComponent<DistanceCounter>();
        }


        //private void OnEnable()
        //{
        //    pathCreator = GameManager.Instance.path;
        //}


        void Start()
        {
            backLight1.emitting = false;
            backLight2.emitting = false;
            MaxSpeed = Random.Range(100, 150);
            acclearionRate = PersistantPlayerDataBase.Instance.CurrentCar.Acceleration.CurrentStat * 0.9f;
            fire.gameObject.SetActive(false);

            isStart = false;

            canFly = true;

            if (pathCreator != null)
            {
                distanceTravelled = startDistance;
                EvaluatedPosition = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                rot = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

                rot.eulerAngles = Vector3.zero;
                transform.SetPositionAndRotation(EvaluatedPosition, rot);

            }

        }





        void LateUpdate()
        {
            RaycastHit hit;

            if (Input.GetMouseButtonDown(0))
            {
                isStart = true;
                backLight1.emitting = true;
                backLight2.emitting = true;
            }



            if (!isStart)
            {
                EvaluatedPosition = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                rot = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                rot1.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, 0);
                if (Mathf.Abs(deviation) > 0.48)
                {
                    rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y + deviation * (-10f), deviation * 35);
                    offsetFromGround = 0.18f;
                }
                else
                {
                    rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y + deviation * (-10f), deviation * 10);
                    offsetFromGround = 0.035f;
                }

                transform.SetPositionAndRotation(EvaluatedPosition, rot1);



                if (Physics.Raycast(EvaluatedPosition + Vector3.up + transform.right * deviation * tobogganWidth / 2f, Vector3.down, out hit, 5,
                  1 << 9))
                {
                    Vector3 EvaluatedPosition1;
                    EvaluatedPosition1 = hit.point + hit.normal * offsetFromGround;
                    Car.SetPositionAndRotation(EvaluatedPosition1, rot);


                }
            }

            if (pathCreator != null && isStart)
            {
                speed += acclearionRate * Time.deltaTime;
                if (speed > MaxSpeed) speed = MaxSpeed;
            }

            if (pathCreator != null && onPath && !istransation && isStart)
            {
                distanceCovered = (distanceTravelled - startDistance);
                float deviationMultiplicator = (1f - Mathf.Abs(deviation) / 6f);

                distanceTravelled += logicalSpeed * speedMultiplicator * deviationMultiplicator * Time.deltaTime;

                if (isfinish && distanceTravelled >= finishDistanceTravelled) distanceTravelled = finishDistanceTravelled;

                EvaluatedPosition = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                rot = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                rot1.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, 0);

                //rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y + deviation * (-10f), deviation * 45f * Mathf.Abs(deviation));
                rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, deviation * 45f * Mathf.Abs(deviation));

                offsetFromGround = .4f * Mathf.Abs(deviation);
                transform.SetPositionAndRotation(EvaluatedPosition, rot1);




                if (Physics.Raycast(EvaluatedPosition + Vector3.up + transform.right * deviation * tobogganWidth / 2f, Vector3.down, out hit, 5,
                  1 << 9))
                {
                    Vector3 EvaluatedPosition1;
                    EvaluatedPosition1 = hit.point + hit.normal * offsetFromGround;

                    Car.transform.SetPositionAndRotation(EvaluatedPosition1, rot);


                }



                if (Mathf.Abs(deviation) > 0.6f)
                {
                    if (!spark.isPlaying)
                    {
                        spark.Play();
                    }

                }
                else
                {
                    spark.Stop();
                }
            }

            else if (!onPath) //The character is flying
            {

                if (!istransation)
                {
                    fallSpeed += .1f * Time.deltaTime;
                    if (fallSpeed >= .525f) fallSpeed = .525f;
                    transform.position += Time.deltaTime* flyingSpeed * transform.forward - transform.up * fallSpeed;
                    if (Physics.Raycast(transform.position, Vector3.down, out hit, 2, 1 << 9))
                    {
                        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
                        distanceCovered = (distanceTravelled - startDistance);
                        Land();
                    }
                }
                else
                {
                    float upflyingSpeed = flyingSpeed / 2f;
                    transform.position += Time.deltaTime * upflyingSpeed * transform.forward;
                }

            }

            //The deviation acceleration is computed here, its used for collisions between characters
            absDeviationAcceleration = Mathf.Abs(_lastDeviation - deviation);

            _lastDeviation = deviation;

            if (isfinish)
            {

                deviation = Mathf.Lerp(deviation, 0, Time.deltaTime * 4.5f);
            }

            distance.distance = distanceTravelled;

        }



        public IEnumerator MakePlayerFly()
        {
            onPath = false;
            istransation = true;
            float t = 0;


            float currentYPos = transform.position.y;
            float upflyingSpeed = flyingSpeed / 1.5f;
            Vector3 childPosition = Car.position;
            Vector3 childMiddlePosition = Vector3.zero;
            Car.DOLocalRotate(Vector3.zero, .75f);

            while (t < 1)
            {
                t += Time.deltaTime / .4f;
                childPosition += Time.deltaTime * upflyingSpeed * transform.forward;
                Vector3 nextPosition = new Vector3(childPosition.x, currentYPos, childPosition.z) + Vector3.up * ejectionForce;
                transform.position = Vector3.Lerp(transform.position, nextPosition, t);
                Car.localPosition = Vector3.Lerp(Car.localPosition, childMiddlePosition, t);
               

               
                yield return Time.deltaTime;
            }

            istransation = false;
            nos.gameObject.SetActive(true);



        }


        public void StartFly()
        {

            //X rotation axis reset
            Vector3 newRot = transform.eulerAngles;
            newRot.x = 0;
            newRot.z = 0;
            transform.eulerAngles = newRot;
            backLight1.emitting = false;
            backLight2.emitting = false;
            dust.Stop();
            fire.gameObject.SetActive(true);

            onPath = false;
            istransation = true;


            Car.transform.DOMove(Car.transform.position + (Mathf.Sign(deviation) * transform.right * 2 + Vector3.up / 1.35f + Car.transform.forward * 6) * ejectionForce, 1.25f).OnComplete(() =>
            {
                transform.position = Car.transform.position;
                Car.transform.localPosition = Vector3.zero + new Vector3(0, 0, 0.74f);
                Car.transform.DOLocalRotate(Vector3.zero, .75f);
                istransation = false;
                //nos.gameObject.SetActive(true);
            });

            DOVirtual.DelayedCall(3f, () =>
            {

                fire.gameObject.SetActive(false);


            });

        }


        public void StartFlyRamp()
        {

            //X rotation axis reset
            Vector3 newRot = transform.eulerAngles;
            newRot.x = 0;
            newRot.z = 0;
            transform.eulerAngles = newRot;
            backLight1.emitting = false;
            backLight2.emitting = false;
            dust.Stop();
            fire.gameObject.SetActive(false);
            onPath = false;
            istransation = true;


            Car.transform.DOMove(Car.transform.position + ( Vector3.up / 1.35f + Car.transform.forward * 4) * ejectionForce, 1.25f).OnComplete(() =>
            {
                transform.position = Car.transform.position;
                Car.transform.localPosition = Vector3.zero + new Vector3(0, 0, 0.74f);
                Car.transform.DOLocalRotate(Vector3.zero, .75f);
                istransation = false;
            });
        }


        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }


        public void ApplyDeviationStop(float duration) { StartCoroutine(DeviationStop(duration)); }
        private IEnumerator DeviationStop(float duration)
        {
            deviationModifAuthorization = false;
            yield return new WaitForSeconds(duration);
            deviationModifAuthorization = true;

        }


        public void ApplyTempDeviation(float modification, float duration) { StartCoroutine(TempDeviation(modification, duration)); }
        private IEnumerator TempDeviation(float modification, float duration)
        {
            //deviationModifAuthorization = false;
            dropspark.Play();
            float timer = 0;

            //Using a while is usually not safe, but the lines here are full safe
            while (timer < duration)
            {
                float delta = Time.fixedDeltaTime * (modification / duration);
                deviation += delta;
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
              

            }
           // yield return new WaitForSeconds(2);
            //deviationModifAuthorization = true;
            dropspark.Stop();
        }

        public void ApplyTempMultiplicator(float multiplicator, float duration) { StartCoroutine(TempMultiplicator(multiplicator, duration)); }
        private IEnumerator TempMultiplicator(float multiplicator, float duration)
        {

            speedMultiplicator += (multiplicator - 1f);

            nos.gameObject.SetActive(true);
            dropspark.Play();


            yield return new WaitForSeconds(duration);

            speedMultiplicator -= (multiplicator - 1f);

            yield return new WaitForSeconds(duration + 0.4f);

            nos.gameObject.SetActive(false);
            dropspark.Stop();
        }

        private IEnumerator CountDown()
        {
            yield return new WaitForSeconds(4f);
            isStart = true;
            backLight1.emitting = true;
            backLight2.emitting = true;
        }

        public void Land()
        {
            fallSpeed = 0f;
            landBoost = true;
            Vector3 pathPos;
            pathPos = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            rot = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            rot1.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, 0);
            transform.DOMove(pathPos, 0.05f).SetEase(Ease.Linear);
            transform.DORotate(new Vector3(rot1.eulerAngles.x, rot1.eulerAngles.y, rot1.eulerAngles.z), 0.1f).SetEase(Ease.Linear);
            deviation = 0;
       
            istransation = true;
            nos.gameObject.SetActive(true);
            fire.gameObject.SetActive(false);
            DOVirtual.DelayedCall(1.5f, () =>
            {
                nos.gameObject.SetActive(false);
                dropspark.Stop();
                landBoost = false;
            });
           


            DOVirtual.DelayedCall(0.051f, () =>
            {
                onPath = true;
                istransation = false;
                backLight1.emitting = true;
                backLight2.emitting = true;
                dropspark.Play();

             

            });
        }
        
        public void Destroy()
        {
            if (_isDestroyed)
            {
                return;
            }

            StartCoroutine(DestroyCoroutine());
        }

        private IEnumerator DestroyCoroutine()
        {
            fire.gameObject.SetActive(true);
            fire.Play();
            _isDestroyed = true;
            foreach (var bumper in GetComponentsInChildren<Bumper>())
            {
                bumper.gameObject.SetActive(false);
            }

            var tempSpeed = MaxSpeed;
            var tempAccel = acclearionRate;
            acclearionRate = -5.0f;
            MaxSpeed = 0;
            yield return new WaitForSeconds(2.0f);
            Respawn(tempSpeed, tempAccel);
        }
        private void Respawn(float tempSpeed, float tempAccel)
        {
            foreach (var bumper in GetComponentsInChildren<Bumper>())
            {
                bumper.gameObject.SetActive(true);
            }
            fire.Stop();
            MaxSpeed = tempSpeed;
            acclearionRate = tempAccel;
            _isDestroyed = false;
            distanceTravelled = Mathf.Clamp(distanceTravelled + 20, 0, Mathf.Infinity);
        }
    }
}