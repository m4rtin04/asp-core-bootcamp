namespace Sección_23___Solid_Principles.ServiceContracts
{
    public interface IFinnhubCompanyProfileService
    {
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    }
}
