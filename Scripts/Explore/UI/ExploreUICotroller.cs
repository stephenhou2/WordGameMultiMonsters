﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace WordJourney
{
	public class ExploreUICotroller : MonoBehaviour {

		public Transform tintHUD;

//		public Transform mask;

		/**********  battlePlane UI *************/
		public Transform battlePlane;

		/**********  battlePlane UI *************/

//		public NPCUIController npcUIController;

		private InstancePool materialCardPool;


		public Transform formulaDetailPlane;

		public Transform materialCardContainer;
		private Transform materialCardModel;


		private InstancePool choiceButtonPool;
		private Transform choiceButtonModel;
		private Transform goodsModel;
		private InstancePool goodsPool;

//		private Dialog[] dialogs;
//		private Choice[] choices;

//		private List<Item> itemsToPickUp = new List<Item>();
		private Item itemToPickUp;




		public void SetUpExploreCanvas(){

			Initialize ();

		}

			
		private void Initialize(){

			Transform poolContainerOfExploreCanvas = TransformManager.FindOrCreateTransform (CommonData.poolContainerName + "/PoolContainerOfExploreCanvas");
			Transform modelContainerOfExploreScene = TransformManager.FindOrCreateTransform (CommonData.instanceContainerName + "/ModelContainerOfExploreScene");

			choiceButtonPool = InstancePool.GetOrCreateInstancePool ("ChoiceButtonPool",poolContainerOfExploreCanvas.name);
//			rewardButtonPool = InstancePool.GetOrCreateInstancePool ("RewardButtonPool",poolContainerOfExploreCanvas.name);
			materialCardPool = InstancePool.GetOrCreateInstancePool ("MaterialCardPool", poolContainerOfExploreCanvas.name);
			goodsPool = InstancePool.GetOrCreateInstancePool ("GoodsPool", poolContainerOfExploreCanvas.name);

			choiceButtonModel = TransformManager.FindTransform ("ChoiceButtonModel");
//			rewardButtonModel = TransformManager.FindTransform ("RewardButtonModel");
			materialCardModel = TransformManager.FindTransform ("MaterialCardModel");
			goodsModel = TransformManager.FindTransform ("GoodsModel");

			choiceButtonModel.SetParent (modelContainerOfExploreScene);
//			rewardButtonModel.SetParent (modelContainerOfExploreScene);
			materialCardModel.SetParent (modelContainerOfExploreScene);
			goodsModel.SetParent (modelContainerOfExploreScene);

			if (!GameManager.Instance.UIManager.UIDic.ContainsKey ("BagCanvas")) {

				GameManager.Instance.UIManager.SetUpCanvasWith (CommonData.bagCanvasBundleName, "BagCanvas", () => {
					Transform bagCanvas = TransformManager.FindTransform ("BagCanvas");
					bagCanvas.GetComponent<BagViewController> ().SetUpBagView (false);
				}, true,true);
			}

		}


		public void ShowFightPlane(){
			battlePlane.gameObject.SetActive (true);
		}

		public void HideFightPlane(){
			battlePlane.gameObject.SetActive (false);
		}


		public void SetUpTintHUD(string tint){
//			HideMask ();
			tintHUD.gameObject.SetActive (true);
			Text tintText = tintHUD.GetComponentInChildren<Text> ();
			tintText.color = Color.black;
			tintText.text = tint;

			StartCoroutine ("PlayTintTextAnim", tintText);

		}

		private IEnumerator PlayTintTextAnim(Text tintText){

			yield return new WaitForSeconds (1f);

			tintText.text = string.Empty;
			tintHUD.gameObject.SetActive (false);

		}

		public void EnterNPC(NPC npc,int currentLevelIndex){

			GetComponent<NPCUIController>().SetupNpcPlane (npc, currentLevelIndex,choiceButtonPool,choiceButtonModel,goodsPool,goodsModel);

		}




		public void SetUpRewardFormulaPlane(Formula formula){

			ItemModel equipment = GameManager.Instance.gameDataCenter.allItemModels.Find (delegate(ItemModel obj) {
				return obj.itemId == formula.itemOrSkillId;
			});

			Transform formulaDetailContainer = formulaDetailPlane.Find ("FormulaDetailContainer");

			Text equipmentName = formulaDetailContainer.Find ("EquipmentName").GetComponent<Text> ();
			Image equipmentIcon = formulaDetailContainer.Find ("EquipmentIcon").GetComponent<Image> ();
			Text equipmentDescription = formulaDetailContainer.Find ("EquipmentDescription").GetComponent<Text> ();

			equipmentName.text = equipment.itemName;
			equipmentDescription.text = equipment.itemDescription;

			Sprite s = GameManager.Instance.gameDataCenter.allItemSprites.Find (delegate(Sprite obj) {
				return obj.name == equipment.spriteName;
			});

			equipmentIcon.sprite = s;

			materialCardPool.AddChildInstancesToPool (materialCardContainer);

			for (int i = 0; i < equipment.materials.Count; i++) {
				
				Transform materialCard = materialCardPool.GetInstance<Transform> (materialCardModel.gameObject, materialCardContainer);

				Material material = equipment.materials [i];

				Image materialIcon = materialCard.Find ("MaterialIcon").GetComponent<Image> ();
				Text materialName = materialCard.Find ("MaterialName").GetComponent<Text> ();

				materialName.text = material.itemName;

				Sprite materialSprite = GameManager.Instance.gameDataCenter.allMaterialSprites.Find (delegate(Sprite obj) {
					return obj.name == material.spriteName;
				});

				materialIcon.sprite = materialSprite;


			}

			Button confirmButton = formulaDetailContainer.Find ("ConfirmButton").GetComponent<Button> ();

			confirmButton.onClick.RemoveAllListeners ();

			confirmButton.onClick.AddListener (delegate() {
				AddFormulaAndQuitFormulaDetailPlane (formula);
			});

			formulaDetailPlane.gameObject.SetActive (true);
		}

		private void AddFormulaAndQuitFormulaDetailPlane(Formula formula){

			Player.mainPlayer.AddItem (formula);

			formulaDetailPlane.gameObject.SetActive (false);

		}


		public void SetUpWorkBenchPlane(){

			GameManager.Instance.UIManager.SetUpCanvasWith (CommonData.workBenchCanvasBundleName, "WorkbenchCanvas", () => {
				TransformManager.FindTransform ("WorkbenchCanvas").GetComponent<WorkBenchViewController> ().SetUpWorkBenchView ();
			}, false, true);

		}

		public void SetUpLearnPlane(){
			GameManager.Instance.UIManager.SetUpCanvasWith (CommonData.learnCanvasBundleName, "LearnCanvas", () => {
				TransformManager.FindTransform("LearnCanvas").GetComponent<LearnViewController>().SetUpLearnView(false);

			}, false, true);
		}


//		public void HideFightPlane(){
//			GetComponent<BattlePlayerUIController> ().QuitFight ();
//			GetComponent<BattleMonsterUIController>().QuitFight ();
//		}

		public void QuitFight(){
			GetComponent<BattlePlayerUIController> ().QuitFight ();
			GetComponent<BattleMonsterUIController>().QuitFight ();
			HideFightPlane ();
		}


		public void EnterNextLevel(){
			TransformManager.FindTransform ("ExploreManager").GetComponent<ExploreManager> ().EnterNextLevel ();
		}

		public void QuitExplore(){

			TransformManager.DestroyTransform(transform);

			DestroyInstances ();

			Resources.UnloadUnusedAssets ();

			System.GC.Collect ();

			GameManager.Instance.UIManager.SetUpCanvasWith (CommonData.homeCanvasBundleName, "HomeCanvas", () => {

				TransformManager.FindTransform("HomeCanvas").GetComponent<HomeViewController> ().SetUpHomeView ();

			});

		}


		#warning 测试用，退出按钮直接退出探索界面
		public void QuitExploreWithTP(){
			TransformManager.FindTransform ("ExploreManager").GetComponent<ExploreManager> ().QuitExploreScene ();
		}

		private void DestroyInstances(){
			GameManager.Instance.UIManager.DestroryCanvasWith (CommonData.exploreSceneBundleName, "ExploreCanvas", "PoolContainerOfExploreScene", "ModelContainerOfExploreScene");
		}


	}
}
