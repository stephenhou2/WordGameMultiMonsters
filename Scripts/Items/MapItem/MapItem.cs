﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordJourney
{
	// 地图物品类型枚举
	public enum MapItemType{
		Buck,
		Pot,
		TreasureBox,
		Tree,
		Stone,
		NormalTrapOn,
		NormalTrapOff,
		Switch,
		Door,
		MovableFloor,
		Transport,
		Billboard,
		FireTrap,
		Hole,
		MovableBox,
		LauncherTowardsUp,
		LauncherTowardsDown,
		LauncherTowardsLeft,
		LauncherTowardsRight,
		Plant,
		PressSwitch,
		Crystal,
		MapNPC,
		Docoration
	}

	public abstract class MapItem : MonoBehaviour {

		public string audioClipName;

		protected SpriteRenderer mapItemRenderer;

		protected CallBack animEndCallBack;

		public MapItemType mapItemType;

		protected BoxCollider2D bc2d;

		public bool isDroppable;

		protected virtual void Awake(){

			mapItemRenderer = GetComponent<SpriteRenderer> ();

			bc2d = GetComponent<BoxCollider2D> ();

			isDroppable = false;
		}

		public abstract void InitMapItem ();
		public abstract void AddToPool (InstancePool pool);

		public void SetSortingOrder(int order){
			mapItemRenderer.sortingOrder = order;
		}

	}


}
