using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace PathCreation.Examples
{

    public class Bumper : MonoBehaviour
    {
        enum BumperType
        {
            LeftBumper,
            RightBumper,
            BackBumper
        }

        [SerializeField] private BumperType type;
        // public PathFollower movementsComponent;
        public AIPathFollower movementsComponent;

        //Making an editor script to show only the necessary parameters depending on the bumper type would have been better
        [Header("Back bumper parameters")]

        [SerializeField] private float backBumpSpeedMultiplicator = 2;
        [SerializeField] private float backBumpDuration = 1;

        [Header("Side bumper parameters")]

        [SerializeField] private float sideBumpTime = 2f;
        [SerializeField] private float bumpDeviation = 0.4f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player") || other.transform.CompareTag("Ai"))
            {

                switch (type)
                {
                    case BumperType.BackBumper:

                        if (other.transform.CompareTag("Player"))
                        {
                            if(!other.gameObject.GetComponentInParent<PathFollower>().isfinish && !movementsComponent.isfinish && !other.gameObject.GetComponentInParent<PathFollower>().landBoost)
                            {
                                movementsComponent.speed = movementsComponent.MaxSpeed;
                                //movementsComponent.speed = other.gameObject.GetComponentInParent<PathFollower>().MaxSpeed;
                                //movementsComponent.speed = movementsComponent.MaxSpeed;
                                other.gameObject.GetComponentInParent<PathFollower>().speed -= 4.5f;
                                movementsComponent.ApplyTempMultiplicator(backBumpSpeedMultiplicator, backBumpDuration);

                                HapticManager.Instance.PlayHaptic(HapticType.MediumImpact);
                            }

                            else if(!other.gameObject.GetComponentInParent<PathFollower>().isfinish && !movementsComponent.isfinish && other.gameObject.GetComponentInParent<PathFollower>().landBoost)
                            {
                                movementsComponent.StartFly();

                                HapticManager.Instance.PlayHaptic(HapticType.MediumImpact);
                            }

                          
                        }

                        else
                        {
                            if (!other.gameObject.GetComponentInParent<AIPathFollower>().isfinish && !movementsComponent.isfinish && !other.gameObject.GetComponentInParent<AIPathFollower>().landBoost)
                            {
                                movementsComponent.speed = movementsComponent.MaxSpeed;
                                //movementsComponent.speed = movementsComponent.MaxSpeed;
                                other.gameObject.GetComponentInParent<AIPathFollower>().speed -= 4.5f;
                                movementsComponent.ApplyTempMultiplicator(backBumpSpeedMultiplicator, backBumpDuration);
                            }

                            else if (!other.gameObject.GetComponentInParent<AIPathFollower>().isfinish && !movementsComponent.isfinish && other.gameObject.GetComponentInParent<AIPathFollower>().landBoost)
                            {
                                movementsComponent.StartFly();

                            }


                        }

                       
                        break;

                    case BumperType.LeftBumper:

                        if (other.transform.CompareTag("Player"))
                        {
                            PathFollower otherCar = other.gameObject.GetComponentInParent<PathFollower>();

                          


                            if(!otherCar.landBoost)
                            {
                                SideBumper(otherCar);
                            }
                            else if(otherCar.distanceTravelled>movementsComponent.distanceTravelled && otherCar.landBoost)
                            {
                                SideBumper(otherCar);
                            }
                            else if(otherCar.distanceTravelled < movementsComponent.distanceTravelled && otherCar.landBoost)
                            {
                                movementsComponent.StartFly();
                            }
                           

                            otherCar.dropspark.Play();

                            DOVirtual.DelayedCall(0.1f, () =>
                            {

                                otherCar.dropspark.Stop();

                            });
                        }

                        else
                        {
                            AIPathFollower otherCar = other.gameObject.GetComponentInParent<AIPathFollower>();

                            SideBumper(otherCar);

                            otherCar.dropspark.Play();

                            DOVirtual.DelayedCall(0.1f, () =>
                            {

                                otherCar.dropspark.Stop();

                            });
                        }





                        break;

                    case BumperType.RightBumper:
                        if (other.transform.CompareTag("Player"))
                        {
                            PathFollower otherCar = other.gameObject.GetComponentInParent<PathFollower>();
                            SideBumper(otherCar);

                            otherCar.dropspark.Play();

                            DOVirtual.DelayedCall(0.1f, () =>
                            {

                                otherCar.dropspark.Stop();

                            });
                        }

                        else
                        {
                            AIPathFollower otherCar = other.gameObject.GetComponentInParent<AIPathFollower>();

                            SideBumper(otherCar);

                            otherCar.dropspark.Play();

                            DOVirtual.DelayedCall(0.1f, () =>
                            {

                                otherCar.dropspark.Stop();

                            });
                        }
                        break;
                }
            }


        }




        //Side bumpers moves the slowest character of the collision out of the bounds of the other
        void SideBumper(PathFollower other)
        {
            //The function is called on both actors of the collision, only the slowest is affected
            if (other.absDeviationAcceleration > movementsComponent.absDeviationAcceleration  && !other.isfinish)
            {
                movementsComponent.ApplyTempDeviation((type == BumperType.LeftBumper ? 1f : -1f) * (bumpDeviation + 0.0005f) * other.absDeviationAcceleration * 25, sideBumpTime);
                // other.ApplyDeviationStop(sideBumpTime);
                HapticManager.Instance.PlayHaptic(HapticType.MediumImpact);
            }
        }

        void SideBumper(AIPathFollower other)
        {
            //The function is called on both actors of the collision, only the slowest is affected
            if (other.absDeviationAcceleration > movementsComponent.absDeviationAcceleration && !other.isfinish)
            {
                movementsComponent.ApplyTempDeviation((type == BumperType.LeftBumper ? 1f : -1f) * (bumpDeviation + 0.0005f) * other.absDeviationAcceleration * 25, sideBumpTime);
                // other.ApplyDeviationStop(sideBumpTime);
            }
        }
    }
}