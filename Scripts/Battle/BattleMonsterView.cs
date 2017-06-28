﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMonsterView : BattleAgentView {


	public void SetUpMonsterView(Monster monster){

		Debug.Log (monster.agentIconName);
		// 加载怪物头像图片
		ResourceManager.Instance.LoadAssetWithName ("battle/monster_icons", () => {
			Debug.Log(ResourceManager.Instance.sprites.Count);
			agentIcon.sprite = ResourceManager.Instance.sprites [0];
		}, true, monster.agentIconName);

	}



}
