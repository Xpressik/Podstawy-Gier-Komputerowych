using UnityEngine;
using System.Collections;

namespace Assets
{
    public class AnimController : MonoBehaviour
    {

        Animator anim;
        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void PlayIdle()
        {
            anim.Play("idle", -1, 0f);
        }

        public void PlayWalk()
        {
            anim.Play("walk", -1, 0f);
        }
    }
}