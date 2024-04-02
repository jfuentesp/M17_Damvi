package Classes;

public class Health extends Component {

	private GameObject m_Owner;
	private int m_MaxHealth;
	private int m_CurrentHealth;
	private boolean m_isAlive;
	
	public Health(GameObject owner, int maxHealth) {
		super("Health");
		this.m_MaxHealth = maxHealth;
		this.m_CurrentHealth = m_MaxHealth;
		this.m_Owner = owner;
	}

	@Override
	public void update() {
		// TODO Auto-generated method stub
		
	}

	public int getMaxHealth() {
		return m_MaxHealth;
	}

	public void setMaxHealth(int maxHealth) {
		this.m_MaxHealth = maxHealth;
	}

	public int getCurrentHealth() {
		return m_CurrentHealth;
	}

	public void setCurrentHealth(int currentHealth) {
		this.m_CurrentHealth = currentHealth;
	}
	
	public void receiveDamage(int damage)
	{
		this.m_CurrentHealth -= damage;
		if(this.m_CurrentHealth <= 0)
		{
			this.m_CurrentHealth = 0;
			m_Owner.destroy();
		}
		System.out.println(m_Owner.getName() + " recibe " + damage + " puntos de daño y pasa a tener " + m_CurrentHealth + " puntos de vida.");
	}

}