package Classes;

import java.util.ArrayList;

import Motor.GameLoop;

public class ShooterBehaviour extends Component 
{
	private GameObject m_Owner;
	private GameLoop m_GameLoop;
	private GameObject m_Target;
	private int m_Damage;
	
	public ShooterBehaviour(GameObject owner, int damage) 
	{
		super("Shooter");
		this.m_Owner = owner;
		this.m_GameLoop = GameLoop.getInstance();
		this.m_Damage = damage;
	}

	@Override
	public void update() 
	{
		Shoot(m_GameLoop.getGameObjects());
	}
	
	private void Shoot(ArrayList<GameObject> targetlist)
	{
		System.out.println(m_Owner.getName() + ": Preparado para disparar.");
		for (GameObject object : targetlist) 
		{
			if(object.hasComponent(DummyBehaviour.class))
			{
				m_Target = object;
				System.out.println("El tirador " + m_Owner.getName() + " ha encontrado un nuevo objetivo: " + object.getName() + "."
						+ " Le dispara.");
				if(m_Target.getComponent(Health.class).getCurrentHealth() > 0)
				{
					m_Target.getComponent(DummyBehaviour.class).RecieveShot(m_Damage);
					Reload();
					return;
				}
			}
		}
	}
	
	private void Reload()
	{
		System.out.println(m_Owner.getName() + " está recargando...");
	}
}
