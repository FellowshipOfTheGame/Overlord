using UnityEngine;
using System.Collections;

public class AudioAnimation : StateMachineBehaviour {

    public AudioClip[] clips;
    public float interval = 0.2f;
    public Vector2 pitchRange = new Vector2(0.8f, 1.2f);
    public Vector2 volumeRange = new Vector2(0.5f, 0.8f);

    private float time;
    private AudioSource audio;

     //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0f;
        audio = animator.gameObject.GetComponent<AudioSource>();
        if (interval < 0)
        {
            audio.pitch = Random.Range(pitchRange.x, pitchRange.y);
            audio.volume = Random.Range(volumeRange.x, volumeRange.y);
            audio.PlayOneShot(clips[Random.Range(0, clips.Length - 1)]);
        }
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (interval < 0)
            return;
        time += Time.deltaTime;
        if(time > interval)
        {
            time = 0f;
            audio.pitch = Random.Range(pitchRange.x, pitchRange.y);
            audio.volume = Random.Range(volumeRange.x, volumeRange.y);
            audio.PlayOneShot(clips[Random.Range(0, clips.Length - 1)]);
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
