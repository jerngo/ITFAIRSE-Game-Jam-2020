using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TimeBody : MonoBehaviour
{

	bool isRewinding = false;

	bool isFastForward = false;

	public AudioSource statifSfx;

	[SerializeField]
	float recordTime = 5f;

	float recordForFF = 0;

	List<PointInTime> pointsInTime;

	List<PointInFastForward> pointsInFF;

	AudioSource music;
	public AudioSource emptyEnergysfx;
	public AudioSource cloningSfx;

	Rigidbody2D rb;

	bool isThisMainChara = true;

	bool firsttimeRecordingFF = true;

	LevelManager levelManager;
	// Use this for initialization
	void Start()
	{
		pointsInTime = new List<PointInTime>();
		pointsInFF = new List<PointInFastForward>();
		rb = GetComponent<Rigidbody2D>();
		levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
		music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
		recordTime = levelManager.timerCount;
		this.statifSfx.enabled = true;
		statifSfx.Stop();
		GameObject.FindGameObjectWithTag("PostPros").GetComponent<PostProcessVolume>().profile.TryGetSettings(out grainsSetting);
	}

	// Update is called once per frame

	Grain grainsSetting = null;

	void Update()
	{
		if (isThisMainChara == true) {
			if (Input.GetKeyDown(KeyCode.P)) {
				grainsSetting.intensity.value = 1.0f;
				if (firsttimeRecordingFF == true) {
					pointsInFF.Clear();
					firsttimeRecordingFF = false;
				}

				GameObject.FindGameObjectWithTag("MonitorControl").GetComponent<Monitor_Cont>().enableIcon();

				StartRewind();
			}

			if (Input.GetKeyUp(KeyCode.P)) {
				grainsSetting.intensity.value = 0.0f;
				GameObject.FindGameObjectWithTag("MonitorControl").GetComponent<Monitor_Cont>().disableIcon();

				StopRewind();
				firsttimeRecordingFF = true;

				
			}


			if (Input.GetKeyDown(KeyCode.L)) {
				StopRewind();
				GameObject.FindGameObjectWithTag("MonitorControl").GetComponent<Monitor_Cont>().disableIcon();
				grainsSetting.intensity.value = 0.0f;
				levelManager.isRealtime = true;
				//use static sfx for webGL only, else use pitch
				statifSfx.Stop();
				music.pitch = 1;
				//this.GetComponent<Robot_Cont>().setCantMove(true);
				if (Input.GetKeyDown(KeyCode.P) == false) { 
				
				

					int energyLeft = GameObject.FindGameObjectWithTag("MonitorControl").GetComponent<Monitor_Cont>().energyLeft;

					if (energyLeft > 0)
					{
						cloningSfx.Play();
						GameObject.FindGameObjectWithTag("MonitorControl").GetComponent<Monitor_Cont>().UpdateEnergy();
						this.GetComponent<Robot_Cont>().changeColor(0.5f);
						summonAnotherPlayer();
						StartFastForward();
					}
					else {
						emptyEnergysfx.Play();
					}
				
					}
				}

		}
		
	}

	void FixedUpdate()
	{
		if (isFastForward == true)
		{
			FastForward();
			this.GetComponent<Robot_Cont>().setCantMove(true);
		}
		else {
			if (isThisMainChara == true) {
				if (isRewinding)
				{
					levelManager.isRealtime = false;
					//use static sfx for webGL only, else use pitch
					if (statifSfx.isPlaying == false) {
						statifSfx.Play();
					}
					
					music.pitch = -1;
					this.GetComponent<Robot_Cont>().setCantMove(true);
					recordForFF += Time.deltaTime;
					RecordFastForward();
					Rewind();

				}

				else
				{
					recordForFF = 0;
					levelManager.isRealtime = true;
					//use static sfx for webGL only, else use pitch
					statifSfx.Stop();
					music.pitch = 1;
					this.GetComponent<Robot_Cont>().setCantMove(false);
					Record();
				}
			}
			
		}

		
			
	}

	float bodyMovement;
	float wheelMovement;


	void Rewind()
	{
		if (pointsInTime.Count > 0)
		{
			PointInTime pointInTime = pointsInTime[0];
			transform.position = pointInTime.position;
			transform.rotation = pointInTime.rotation;

			Vector3 tempScale = pointInTime.scale;
			if (tempScale.y != 0.62297f) {
				if (tempScale.x > 0)
				{
					tempScale = new Vector3(0.62297f, 0.62297f, 0.62297f);
				}
				else {

					tempScale = new Vector3(-0.62297f, 0.62297f, 0.62297f);
				}
				
			}


			//transform.localScale = pointInTime.scale;
			transform.localScale = tempScale;

			GetComponent<Robot_Cont>().animBody.SetFloat("Movement", pointInTime.axisInput);
			bodyMovement = pointInTime.axisInput;
			GetComponent<Robot_Cont>().animWheel.SetFloat("Movement", pointInTime.axisInput);

			pointsInTime.RemoveAt(0);
		}
		else
		{
			GameObject.FindGameObjectWithTag("MonitorControl").GetComponent<Monitor_Cont>().disableIcon();
			grainsSetting.intensity.value = 0.0f;
			StopRewind();
		}
		
	}

	void FastForward()
	{
		isThisMainChara = false;
		if (pointsInFF.Count > 0)
		{
			PointInFastForward pointInFastF = pointsInFF[0];
			transform.position = pointInFastF.position;
			transform.rotation = pointInFastF.rotation;
			//transform.localScale = pointInFastF.scale;

			Vector3 tempScale = pointInFastF.scale;
			if (tempScale.y != 0.62297f)
			{
				if (tempScale.x > 0)
				{
					tempScale = new Vector3(0.62297f, 0.62297f, 0.62297f);
				}
				else
				{

					tempScale = new Vector3(-0.62297f, 0.62297f, 0.62297f);
				}
			}


			//transform.localScale = pointInTime.scale;
			transform.localScale = tempScale;

			GetComponent<Robot_Cont>().animBody.SetFloat("Movement", pointInFastF.axisInput);
			GetComponent<Robot_Cont>().animWheel.SetFloat("Movement", pointInFastF.axisInput);
			pointsInFF.RemoveAt(0);
		}
		else
		{
			StopFastForward();
		}
		
	}

	void Record()
	{
		if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
		{
			pointsInTime.RemoveAt(pointsInTime.Count - 1);
		}

		pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, transform.localScale, Mathf.Abs(Input.GetAxis("Horizontal"))));
	}

	void RecordFastForward()
	{
		if (pointsInFF.Count > Mathf.Round(recordForFF / Time.fixedDeltaTime))
		{
			pointsInFF.RemoveAt(pointsInFF.Count - 1);
		}

		pointsInFF.Insert(0, new PointInFastForward(transform.position, transform.rotation, transform.localScale, bodyMovement));
	}

	public void StartRewind()
	{
		
		isRewinding = true;
		rb.isKinematic = true;
	}


	public void StartFastForward()
	{
		isFastForward = true;
		rb.isKinematic = true;
	}

	public void StopFastForward()
	{
		isFastForward = false;
		rb.isKinematic = false;
	}

	public void StopRewind()
	{
		isRewinding = false;
		rb.isKinematic = false;
	}

	public GameObject playerPrefab;
	void summonAnotherPlayer() {
		this.gameObject.name = "AlternatePlayer";
		this.GetComponent<Robot_Cont>().stopSFX();
		this.statifSfx.enabled = false;
		Vector3 offset = new Vector3(0, 0, 0);
		GameObject newPlayer = Instantiate(playerPrefab, transform.position - offset, Quaternion.identity);
		newPlayer.transform.parent = null;
		if (this.transform.localScale.x < 0)
		{
			newPlayer.transform.localScale = new Vector3(-0.62297f, 0.62297f, 0.62297f);
		}
		else {
			newPlayer.transform.localScale = new Vector3(0.62297f, 0.62297f, 0.62297f);
		}
		
		newPlayer.name = "MainPlayer";
	}
}
