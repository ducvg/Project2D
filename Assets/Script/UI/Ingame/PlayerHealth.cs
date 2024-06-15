//==============================================================
// HealthSystem
// HealthSystem.Instance.TakeDamage (float Damage);
// HealthSystem.Instance.HealDamage (float Heal);
// HealthSystem.Instance.UseMana (float Mana);
// HealthSystem.Instance.RestoreMana (float Mana);
// Attach to the Hero.
//==============================================================

using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth: MonoBehaviour
{
	public static PlayerHealth Instance;

	public Image currentHealthBar;
	public Text healthText;
	public float hitPoint = 100f;
	public float maxHitPoint = 100f;



	//==============================================================
	// Awake
	//==============================================================
	void Awake()
	{
		Instance = this;
	}
	
	//==============================================================
	// Awake
	//==============================================================
  	void Start()
	{
		UpdateGraphics();
	}

	//==============================================================
	// Update
	//==============================================================
	void Update ()
	{

	}

	//==============================================================
	// Regenerate Health & Mana
	//==============================================================
	private void Regen()
	{

	}

	//==============================================================
	// Health Logic
	//==============================================================
	private void UpdateHealthBar()
	{
		float ratio = hitPoint / maxHitPoint;
		currentHealthBar.rectTransform.localPosition = new Vector3(currentHealthBar.rectTransform.rect.width * ratio - currentHealthBar.rectTransform.rect.width, 0, 0);
		healthText.text = hitPoint.ToString ("0") + "/" + maxHitPoint.ToString ("0");
	}

	
	public void TakeDamage(float Damage)
	{
		hitPoint -= Damage;
		if (hitPoint < 1)
			hitPoint = 0;

		UpdateGraphics();
	}

	public void HealDamage(float Heal)
	{
		hitPoint += Heal;
		if (hitPoint > maxHitPoint) 
			hitPoint = maxHitPoint;

		UpdateGraphics();
	}
	public void SetMaxHealth(float max)
	{
		maxHitPoint += (int)(maxHitPoint * max / 100);

		UpdateGraphics();
	}

	

	//==============================================================
	// Update all Bars & Globes UI graphics
	//==============================================================
	private void UpdateGraphics()
	{
		UpdateHealthBar();
	}

	//==============================================================
	// Coroutine Player Hurts
	//==============================================================
	//IEnumerator PlayerHurts()
	//{
	//	// Player gets hurt. Do stuff.. play anim, sound..

	//	PopupText.Instance.Popup("Ouch!", 1f, 1f); // Demo stuff!

	//	if (hitPoint < 1) // Health is Zero!!
	//	{
	//		yield return StartCoroutine(PlayerDied()); // Hero is Dead
	//	}

	//	else
	//		yield return null;
	//}

	////==============================================================
	//// Hero is dead
	////==============================================================
	//IEnumerator PlayerDied()
	//{
	//	// Player is dead. Do stuff.. play anim, sound..
	//	PopupText.Instance.Popup("You have died!", 1f, 1f); // Demo stuff!

	//	yield return null;
	//}
}
