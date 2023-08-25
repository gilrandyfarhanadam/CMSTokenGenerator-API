namespace DatabaseConnection
{
    public interface ICheck
    {
        //
        public bool IsExist(string obj);
        public bool IsValid(string obj);
        public bool IsMatch(string[] obj);
    }    
}