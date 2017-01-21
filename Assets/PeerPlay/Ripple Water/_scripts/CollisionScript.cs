using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour {
	private int waveNumber;
	public float distanceX, distanceZ;
	public float[] waveAmplitude;
	public float magnitudeDivider;
	public Vector2[] impactPos;
	public float[] distance;
	public float speedWaveSpread;

	Mesh mesh;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
	}
	
	// Update is called once per frame
	void Update () {
	
		for (int i=0; i<8; i++){

			waveAmplitude[i] = GetComponent<Renderer>().material.GetFloat("_WaveAmplitude" + (i+1));
			if (waveAmplitude[i] > 0)

			{
				distance[i] += speedWaveSpread;
				GetComponent<Renderer>().material.SetFloat("_Distance" + (i+1), distance[i]);
				GetComponent<Renderer>().material.SetFloat("_WaveAmplitude" + (i+1), waveAmplitude[i] * 0.98f);
			}
			if (waveAmplitude[i] < 0.01)
			{
				GetComponent<Renderer>().material.SetFloat("_WaveAmplitude" + (i+1), 0);
				distance[i] = 0;
			}

		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			// testing
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
			RaycastHit hit;
			int layerMask = 1 << LayerMask.NameToLayer("Ground");

			if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
			{
				waveNumber++;
				if (waveNumber == 9)
				{
					waveNumber = 1;
				}
				waveAmplitude[waveNumber - 1] = 0;
				distance[waveNumber - 1] = 0;

				distanceX = this.transform.position.x - hit.point.x;
				distanceZ = this.transform.position.z - hit.point.z;
				impactPos[waveNumber - 1].x = hit.point.x;
				impactPos[waveNumber - 1].y = hit.point.z;

				GetComponent<Renderer>().material.SetFloat("_xImpact" + waveNumber, hit.point.x);
				GetComponent<Renderer>().material.SetFloat("_zImpact" + waveNumber, hit.point.z);

				GetComponent<Renderer>().material.SetFloat("_OffsetX" + waveNumber, distanceX / mesh.bounds.size.x * 2.5f);
				GetComponent<Renderer>().material.SetFloat("_OffsetZ" + waveNumber, distanceZ / mesh.bounds.size.z * 2.5f);

				GetComponent<Renderer>().material.SetFloat("_WaveAmplitude" + waveNumber, /*col.rigidbody.velocity.magnitude*/30.0f * magnitudeDivider);
			}
		}
	}

	void OnCollisionEnter(Collision col){
		if (col.rigidbody)
		{
			waveNumber++;
			if (waveNumber == 9){
				waveNumber = 1;
			}
			waveAmplitude[waveNumber-1] = 0;
			distance[waveNumber-1] = 0;

			distanceX = this.transform.position.x - col.gameObject.transform.position.x;
			distanceZ = this.transform.position.z - col.gameObject.transform.position.z;
			impactPos[waveNumber - 1].x = col.transform.position.x;
			impactPos[waveNumber - 1].y = col.transform.position.z;

			GetComponent<Renderer>().material.SetFloat("_xImpact" + waveNumber, col.transform.position.x);
			GetComponent<Renderer>().material.SetFloat("_zImpact" + waveNumber, col.transform.position.z);

			GetComponent<Renderer>().material.SetFloat("_OffsetX" + waveNumber, distanceX / mesh.bounds.size.x * 2.5f);
			GetComponent<Renderer>().material.SetFloat("_OffsetZ" + waveNumber, distanceZ / mesh.bounds.size.z * 2.5f);

			GetComponent<Renderer>().material.SetFloat("_WaveAmplitude" + waveNumber, col.rigidbody.velocity.magnitude * magnitudeDivider);

		}
	}
}
