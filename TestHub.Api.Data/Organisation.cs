namespace TestHub.Api.Data
{
    public class Organisation : DataObjectBase
    {
        public string Name { get; set; }
        public string Projects { get; set; }

        public string Coverage { get; set; }
        public OrgSummary Summary { get; set; }
    }
}
