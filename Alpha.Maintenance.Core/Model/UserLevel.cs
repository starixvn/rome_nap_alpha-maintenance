namespace Alpha.Maintenance.Core.Model
{
    public class UserLevel
    {
        public UserLevel(byte id, string name)
        {
            Id = id;
            Name = name;
        }

        public byte Id { get;}

        public string Name { get;}
    }
}