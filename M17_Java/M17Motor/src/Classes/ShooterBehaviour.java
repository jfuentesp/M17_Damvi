package Classes;

public class ShooterBehaviour extends Component 
{
	private GameObject m_Owner;
	
	public ShooterBehaviour(GameObject owner) 
	{
		super("Shooter");
		this.m_Owner = owner;
	}

	@Override
	public void update() 
	{
		// TODO Auto-generated method stub
		
	}
}
