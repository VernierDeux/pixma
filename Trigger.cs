
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Event {

[AddComponentMenu("Trigger")]
public class Trigger : UniqueMonoBehaviour 
{
	public enum AreaKind
	{
		None,
		Rectangular,
		Circular
	}

	public AreaKind areaKind = AreaKind.None;

	

	public List<Action> onLoaded = new List<Action>(0);
	public List<Action> onPlayerEnter = new List<Action>(0);	
	public List<Action> onPlayerLeave = new List<Action>(0);	
	public List<Action> onPlayerStay = new List<Action>(0);		
	public List<Action> onNPCEnter = new List<Action>(0);		
	public List<Action> onNPCLeave = new List<Action>(0);		
	public List<Action> onNPCStay = new List<Action>(0);		/

	private bool performedFirstRun = false;



	public override void Awake()
	{
		autoSaveLoadEnabled = false;
		autoSaveDestroy = false;
		performedFirstRun = false;

		base.Awake();

		for (int i = 0; i < onLoaded.Count; i++) onLoaded[i].enabled = false;
		for (int i = 0; i < onPlayerEnter.Count; i++) onPlayerEnter[i].enabled = false;
		for (int i = 0; i < onPlayerLeave.Count; i++) onPlayerLeave[i].enabled = false;
		for (int i = 0; i < onPlayerStay.Count; i++) onPlayerStay[i].enabled = false;
		for (int i = 0; i < onNPCEnter.Count; i++) onNPCEnter[i].enabled = false;
		for (int i = 0; i < onNPCLeave.Count; i++) onNPCLeave[i].enabled = false;
		for (int i = 0; i < onNPCStay.Count; i++) onNPCStay[i].enabled = false;
	}

	void Start()
	{
		

		if (areaKind == AreaKind.Circular)
		{
			SphereCollider col = gameObject.AddComponent<SphereCollider>();
			col.isTrigger = true;
			col.radius = 1f; 
		}
		else if (areaKind == AreaKind.Rectangular)
		{
			BoxCollider col = gameObject.AddComponent<BoxCollider>();
			col.isTrigger = true;
			col.size = Vector3.one; 
		}
	}

	private void PerformFirstRun()
	{
		performedFirstRun = true;
		bool mustBeActive = false;

		if (areaKind != AreaKind.None)
		{
			
			if (onPlayerEnter.Count > 0) mustBeActive = true;
			else if (onPlayerLeave.Count > 0) mustBeActive = true;
			else if (onPlayerStay.Count > 0) mustBeActive = true;
			else if (onNPCEnter.Count > 0) mustBeActive = true;
			else if (onNPCLeave.Count > 0) mustBeActive = true;
			else if (onNPCStay.Count > 0) mustBeActive = true;
		}

		if (mustBeActive == false)
		{
			if (onLoaded.Count > 0)
			{	
				UniRPGGameController.ExecuteActions(onLoaded, gameObject, null, null, null, gameObject, false);
			}
			else
			{	
				gameObject.SetActive(false);
			}
		}
		else
		{	
			UniRPGGameController.ExecuteActions(onLoaded, gameObject, null, null, null, null, false);
		}
	}

	public override void Update()
	{
		if (performedFirstRun) return;

		if (IsLoading)
		{
			base.Update();
			return;
		}

		PerformFirstRun();
	}

	void OnTriggerEnter(Collider c)
	{
		if (onPlayerEnter.Count == 0 && onNPCEnter.Count == 0) return;
		Actor a = c.GetComponent<Actor>();
		if (a == null) return; 

		if (onPlayerEnter.Count > 0)
		{	
			if (a.ActorType == UniRPGGlobal.ActorType.Player) UniRPGGameController.ExecuteActions(onPlayerEnter, gameObject, a.gameObject, null, null, null, false);
		}
		if (onNPCEnter.Count > 0)
		{
			if (a.ActorType != UniRPGGlobal.ActorType.Player) UniRPGGameController.ExecuteActions(onNPCEnter, gameObject, a.gameObject, null, null, null, false);
		}
	}

	void OnTriggerExit(Collider c)
	{
		if (onPlayerLeave.Count == 0 && onNPCLeave.Count == 0) return;
		Actor a = c.GetComponent<Actor>();
		if (a == null) return; 

		if (onPlayerLeave.Count > 0)
		{	
			if (a.ActorType == UniRPGGlobal.ActorType.Player) UniRPGGameController.ExecuteActions(onPlayerLeave, gameObject, a.gameObject, null, null, null, false);
		}
		if (onNPCLeave.Count > 0)
		{	
			if (a.ActorType != UniRPGGlobal.ActorType.Player) UniRPGGameController.ExecuteActions(onNPCLeave, gameObject, a.gameObject, null, null, null, false);
		}
	}

	void OnTriggerStay(Collider c)
	{
		if (onPlayerStay.Count == 0 && onNPCStay.Count == 0) return;
		Actor a = c.GetComponent<Actor>();
		if (a == null) return; 

		if (onPlayerStay.Count > 0)
		{	
			if (a.ActorType == UniRPGGlobal.ActorType.Player) UniRPGGameController.ExecuteActions(onPlayerStay, gameObject, a.gameObject, null, null, null, false);
		}
		if (onNPCStay.Count > 0)
		{	
			if (a.ActorType != UniRPGGlobal.ActorType.Player) UniRPGGameController.ExecuteActions(onNPCStay, gameObject, a.gameObject, null, null, null, false);
		}
	}

	
} }