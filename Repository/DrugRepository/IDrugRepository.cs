using Demo.Dtos;
using Demo.Model;
namespace Demo.Repository;

public interface IDrugRepository
{
    Task<string> AddDrugDetails(DrugDetailsDto drugDto);
    Task DeleteDrugsAsync(List<DrugDetails> drugs);
    Task<List<DrugDetails>> GetDrugsToUpdateAsync();
    
    Task<DrugDetails> GetDrugByIdAsync(int drugId);
    IQueryable<DrugDetails> GetAllDrugs();
    Task UpdateDrugDetails(DrugDetails drug);
    Task<List<DrugDetails>> SearchDrugsByNameAsync(string searchTerm);

    
}