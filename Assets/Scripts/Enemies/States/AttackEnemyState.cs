using System;
using GeneralBehaviours.HealthBehaviours;
using Time = UnityEngine.Time;

namespace Enemies.States
{
	public class AttackEnemyState : EnemyState
	{
		private float _secondsSinceLastAttack = 0f;

		public event EventHandler AttackingFinished; 
        
		public AttackEnemyState(Enemy enemy) : base(enemy) { }

		public override string DisplayName => "Attacking";
		public override string DisplayStatus { get; protected set; }
		
		public HealthComponent Target { get; set; }
		public float DamagePerHit { get; set; }
		public float SecondsBetweenAttacks { get; set; }
		
		public override void Begin()
		{
			// They should be able to attack as soon as they reach the target
			_secondsSinceLastAttack = SecondsBetweenAttacks;
		}

		public override void Update()
		{
			if (!Target)
			{
				AttackingFinished?.Invoke(this, EventArgs.Empty);
				return;
			}
			
			if (_secondsSinceLastAttack < SecondsBetweenAttacks)
			{
				_secondsSinceLastAttack += Time.deltaTime;
				return;	
			}

			// Not setting attack timer back to zero.
			// Depending on the timing, we could lose a lot of attacks over time
			_secondsSinceLastAttack -= SecondsBetweenAttacks;
			Target.Health.CurrentHealth -= DamagePerHit;
			
			if (Target.Health.IsAtMinHealth)
			{
				AttackingFinished?.Invoke(this, EventArgs.Empty);
			}
		}

		public override void End() { }
	}
}