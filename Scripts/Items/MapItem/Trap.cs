﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WordJourney
{
	public class Trap : MapItem {

		public bool trapOff;

		protected override void Awake ()
		{
			base.Awake ();
			this.mapItemType = MapItemType.Trap;
		}

		public void OnTriggerEnter2D(Collider2D col){

			if (trapOff) {
				return;
			}

			GetComponent<BoxCollider2D> ().enabled = false;

			GameManager.Instance.soundManager.PlayClips (
				GameManager.Instance.dataCenter.allExploreAudioClips, 
				SoundDetailTypeName.Map, 
				mapItemName);

			col.GetComponent<BattlePlayerController> ().trapTriggered = this;

			Debug.Log ("触发陷阱");

		}

		public void OnSwitchOff(){

			this.trapOff = true;

			transform.GetComponent<SpriteRenderer> ().sprite = unlockedOrDestroyedSprite;

		}

	}
}
