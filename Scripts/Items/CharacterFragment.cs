﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordJourney
{
	public class CharacterFragment : Item {

		public CharacterFragment(char character){

			itemName = string.Format ("字母碎片-{0}", character.ToString());
			itemNameInEnglish = character.ToString();
			spriteName = "character_fragment";
		}


		public override string GetItemBasePropertiesString ()
		{
			return itemName;
		}

		public override string GetItemTypeString ()
		{
			return "字母碎片";
		}

		public override string ToString ()
		{
			return string.Format ("[CharacterFragment]");
		}
	}
}