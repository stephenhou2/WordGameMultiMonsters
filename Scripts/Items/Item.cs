﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {

	public string itemName;
	public string itemDescription;
	public string spriteName;

	public ItemType itemType;

	public int itemId;

	public int attackGain;//攻击力增益
	public int powerGain;//力量增益
	public int magicGain;//魔法增益
	public int critGain;//暴击增益
	public int amourGain;//护甲增益
	public int magicResistGain;//魔抗增益
	public int agilityGain;//闪避增益

//	public int strengthConsume;//气力消耗

	public int healthGain;//血量增益
	public int strengGain;//气力增益


	public override string ToString ()
	{
		return string.Format ("[Item]:" + itemName + "[\nItemDesc]:" + itemDescription);
	}


}
