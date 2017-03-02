using UnityEngine;
using System.Collections;

public class DestroyAfterAnimation : MonoBehaviour {

    public float delay;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }
}
