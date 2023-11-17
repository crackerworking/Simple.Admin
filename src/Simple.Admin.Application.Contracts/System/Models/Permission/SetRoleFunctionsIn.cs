namespace Simple.Admin.Application.Contracts.System.Models.Permission
{
    public class SetRoleFunctionsIn
    {
        //long id, IList<long> funcIds
        public long id { get; set; }
        public IList<long> funcIds { get; set; }
    }
}