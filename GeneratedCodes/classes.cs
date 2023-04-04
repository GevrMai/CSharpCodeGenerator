class Class1
{
	public Class1( Class2 objClass2)
	{
		this.objClass2 = objClass2;

	}
	~Class1(){}

	public Class2 class2_A
	{
		set{objClass2 = value;}
		get{ return objClass2; }
	}
	private Class2 objClass2;
}
class Class2
{
	public Class2()
	{

	}
	~Class2(){}

}
