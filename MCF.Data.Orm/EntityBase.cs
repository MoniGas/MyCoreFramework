namespace MCF.Data.Orm
{
    [Serializable]
    public abstract class EntityBase
    {
        public abstract string GetTableName();

        public abstract string GetPkField();

        public abstract string GetIdenField();
    }
}
