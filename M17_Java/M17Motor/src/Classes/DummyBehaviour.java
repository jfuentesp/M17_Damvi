package Classes;

public class DummyBehaviour extends Component 
{
	private GameObject m_Owner;
	
	public DummyBehaviour(GameObject owner) 
	{
		super("Dummy");
		this.m_Owner = owner;
	}

	@Override
	public void update() 
	{
		// TODO Auto-generated method stub
		
	}
	
}