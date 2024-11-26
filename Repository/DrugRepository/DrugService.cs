using Demo.Database;
using Demo.Dtos;
using Demo.Model;
using Demo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

public class DrugService : IDrugRepository
{
        private readonly DatabaseContext context;

        public DrugService(DatabaseContext _context)
        {
            context = _context;
        }

    
    public async Task<DrugDetailsDto> AddDrugDetails(DrugDetailsDto drugDto)
    {
       if (drugDto == null)
        {
            throw new ArgumentNullException(nameof(drugDto));
        }
         var supplier = await context.SupplierDetails.FindAsync(drugDto.SupplierId); 

        if (supplier == null)
        {
            throw new Exception("Supplier not found.");
        }

        //Mapping
        var drug = new DrugDetails
        {
                DrugName = drugDto.DrugName,
                Quantity = drugDto.Quantity,
                ExpiryDate = drugDto.ExpiryDate,
                Price = drugDto.Price,
                SupplierId=drugDto.SupplierId
        };

        context.DrugDetails.Add(drug);
        await context.SaveChangesAsync();

        return drugDto;
    }
    public async Task<List<DrugDetails>> GetDrugsToUpdateAsync()
    {
        var expiredOrOutOfStockDrugs = await context.DrugDetails
                                                 .Where(d => d.ExpiryDate < DateTime.Now || d.Quantity == 0)
                                                 .ToListAsync();

        return expiredOrOutOfStockDrugs;
    }
    
    public async Task DeleteDrugsAsync(List<DrugDetails> drugs)
    {
        if (drugs != null && drugs.Any())
        {
            context.DrugDetails.RemoveRange(drugs);
            await context.SaveChangesAsync();
        }
    }

    public async Task<List<DrugDetails>> SearchDrugsByNameAsync(string searchDrug)
    {
            return await context.DrugDetails
                                 .Where(d => EF.Functions.Like(d.DrugName, $"%{searchDrug}%"))
                                 .ToListAsync();
    }

    public async Task UpdateDrugDetails(DrugDetails drug)
    {
            context.DrugDetails.Update(drug);
            await context.SaveChangesAsync();
    }

    public IQueryable<DrugDetails> GetAllDrugs()
    {
            return context.DrugDetails;
    }

    
    public async Task<DrugDetails> GetDrugByIdAsync(int drugId)
    {
        return await context.DrugDetails.FirstOrDefaultAsync(d => d.DrugId == drugId);
    }


}
    
      



    

