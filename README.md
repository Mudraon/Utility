# GenericSerializer

[Serializable]
public class Person
{
    public string Name;
    public string LastName;
}

public void Run()
{
    Person lPerson = new Person()
    {
        Name = "Foo",
        LastName = "Bar"
    };
    // save
    new GenericSerializer<Person>().XmlSerialize("person.xml", lPerson);
    // load
    lPerson = new GenericSerializer<Person>().XmlDeserialize("person.xml");
}
