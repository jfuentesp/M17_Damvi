package Classes;

public class BulletBehaviour extends Component
{
	private GameObject m_Owner;
	
	public BulletBehaviour(GameObject owner) {
		super("Bullet");
		this.m_Owner = owner;
	}

	@Override
	public void update() {
		// TODO Auto-generated method stub
		
	}

}