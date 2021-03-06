﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WordJourney
{

	using UnityEngine.UI;
	using UnityEditor;

	using System.Linq;
	using System.Text;
	using CE.iPhone.PList;
	using System.IO;
	using System.Data;

	using UnityEngine.EventSystems;

	public class EditHelper {
		
		[MenuItem("EditHelper/GameLevelDatasHelper")]
		public static void GameLevelDatasHelper(){
			GameLevelDataHandler gldh = new GameLevelDataHandler ();
			gldh.LoadGameDatas ();
			gldh.SaveGameDatas ();
		}

		[MenuItem("EditHelper/TempHelper")]
		public static void TempHelper(){

			Transform charactersPlane = TransformManager.FindTransform ("CharactersPlane");

			int AInASCII = (int)('A');

			for (int i = 0; i < charactersPlane.childCount; i++) {

				Transform child = charactersPlane.GetChild (i);

				string character = ((char)(i + AInASCII)).ToString ();

				child.Find ("Text").GetComponent<Text> ().text = character;

				child.Find ("CharacterTint").Find("Text").GetComponent<Text>().text = character;

				child.name = "CharacterButton_" + character;

			}


		}

		[MenuItem("EditHelper/GeneratePlayerJson")]
		public static void GeneratePlayerJsonData(){

			Player p = TransformManager.FindTransform ("Player").GetComponent<Player> ();

//			p.allEquipedEquipments = new Equipment[6];

			p.physicalHurtScaler = 1.0f;
			p.magicalHurtScaler = 1.0f;
			p.critHurtScaler = 1.5f;

			PlayerData pd = new PlayerData (p);

			string originalPlayerDataPath = Path.Combine (CommonData.originDataPath, "OriginalPlayerData.json");

			DataHandler.SaveInstanceDataToFile<PlayerData> (pd, originalPlayerDataPath);


		}




		// 将物品csv数据转化为json文件并存储直接使用这个方法
		private static void ConvertItemToJson(){

				

			ItemModel[] allItem = ItemsToJson ();

			AllItemsJson aij = new AllItemsJson ();

			aij.Items = allItem;

			string allItemsJsonStr = JsonUtility.ToJson (aij);

			File.WriteAllText (CommonData.persistDataPath + "/AllItemsJson.txt", allItemsJsonStr);

		}

		static private ItemModel[] ItemsToJson(){

			string csv = DataHandler.LoadDataString ("itemsData.csv");

			string[] dataArray = csv.Split (new string[]{ "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

			List <ItemModel> allItem = new List<ItemModel> ();

			for (int i = 1; i < dataArray.Length; i++) {

				string[] itemStr = dataArray [i].Split (',');
					
				ItemModel itemModel = new ItemModel ();

				itemModel.itemId = System.Convert.ToInt32 (itemStr [0]);
				itemModel.itemName = itemStr[1];
				itemModel.itemGeneralDescription = itemStr[2];
				itemModel.spriteName = itemStr[3];
				itemModel.itemType = (ItemType)System.Convert.ToInt16(itemStr[4]);
				itemModel.itemNameInEnglish = itemStr[5];
				itemModel.attackGain = System.Convert.ToInt16(itemStr[6]);
//				itemModel.attackSpeedGain = System.Convert.ToInt16(itemStr[7]);
				itemModel.armorGain = System.Convert.ToInt16(itemStr[8]);
				itemModel.magicResistGain = System.Convert.ToInt16(itemStr[9]);
				itemModel.critGain = System.Convert.ToInt16(itemStr[10]);
				itemModel.dodgeGain = System.Convert.ToInt16(itemStr[11]);
				itemModel.healthGain = System.Convert.ToInt16(itemStr[12]);
				itemModel.manaGain = System.Convert.ToInt16 (itemStr [13]);
//				itemModel.equipmentType = (EquipmentType)System.Convert.ToInt16 (itemStr [14]);

				allItem.Add (itemModel);

			}

			ItemModel[] ItemArray = new ItemModel[allItem.Count];

			allItem.CopyTo(ItemArray);


			return ItemArray;

		}

		public class AllItemsJson{

			public ItemModel[] Items;

		}

	}
		
}