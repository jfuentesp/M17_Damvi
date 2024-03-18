package Classes;

public class Sprite extends Component {

	private String m_Path;
	private String m_Description;
	
	public Sprite(String path, String description) {
		super("Sprite");
		this.m_Path = path;
		this.m_Description = description;
	}

	@Override
	public void update() {
		// TODO Auto-generated method stub
		
	}

}
