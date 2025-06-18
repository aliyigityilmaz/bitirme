using System;
using UnityEngine;


    public class CombatCameraManager : MonoBehaviour
    {//singleton
        public static CombatCameraManager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
               // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        public Camera combatCamera;
        public Transform mainCameraTransform;
        public Transform cameraTransformHero1;
        public Transform cameraTransformHero2;
        public Transform cameraTransformHero3;
        public Transform cameraTransformHero4;

        
        
        public void SetCameraPosition(Transform targetTransform)
        {
            if (targetTransform != null)
            {
                combatCamera.transform.position = targetTransform.position;
                combatCamera.transform.rotation = targetTransform.rotation;
            }
            else
            {
                Debug.LogWarning("Target transform is null!");
            }
        }
        
        
        
        
        
    }
