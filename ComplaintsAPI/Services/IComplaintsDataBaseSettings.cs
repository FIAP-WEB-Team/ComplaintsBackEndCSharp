namespace ComplaintsAPI.Services
{
    public interface IComplaintsDataBaseSettings
    {
      string CollectionName { get; set; }
      string ConnectionString { get; set; }
      string DatabaseName { get; set; }
    }
}
