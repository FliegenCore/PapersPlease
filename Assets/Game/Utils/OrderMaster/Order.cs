[System.AttributeUsage(System.AttributeTargets.Class)]

public class Order : System.Attribute
{
    private int m_Id;

    public int Id => m_Id;

    public Order(int id)
    {
        m_Id = id;
    }
}
