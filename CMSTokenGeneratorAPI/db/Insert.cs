namespace DatabaseConnection
{
    public interface IInsert
    {
        public bool Single(Object obj);

        public bool[] Many(Object[] obj);
    }
}