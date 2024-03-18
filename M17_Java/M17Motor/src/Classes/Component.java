package Classes;

public abstract class Component {
	private String m_Name = "Component";
	
	public Component(String name) {
		this.m_Name = name;
	}
	
	public abstract void update();
}
