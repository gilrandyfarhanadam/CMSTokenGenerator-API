namespace DatabaseConnection
{
    public interface IDelete 
    {
        public bool Single(string obj);
        
        public bool[] Bulk(string[] obj);
    }
}