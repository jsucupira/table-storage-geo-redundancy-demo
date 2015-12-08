namespace Model.Transaction
{
    public class RedundancyModel
    {
        //if this value is null in the table means that it was added in the primary region
        public string ReferenceId { get; set; }
    }
}