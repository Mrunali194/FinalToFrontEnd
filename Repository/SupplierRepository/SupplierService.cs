using Demo.Database;
using Demo.Model;
using Microsoft.EntityFrameworkCore;

public class SupplierService:ISupplierRepository
{
     private readonly DatabaseContext context;

        public SupplierService(DatabaseContext _context)
        {
            context = _context;
        }
    
    public async Task<SupplierDetailsDto> AddSupplierDetails(SupplierDetailsDto supplierDto)
    {
        if (supplierDto == null)
        {
            throw new Exception("Supplier details is required.");
        }
        var existingSupplier = await context.SupplierDetails
            .FirstOrDefaultAsync(s => s.SupplierEmail == supplierDto.SupplierEmail);
        
        if (existingSupplier != null)
        {
            throw new Exception("A supplier with the given email already exists.");
        }

        //Map Supplier table to DTO
        var supplier = new SupplierDetails
        {
            SupplierEmail = supplierDto.SupplierEmail,
            SupplierName = supplierDto.SupplierName,
            Contact = supplierDto.Contact
        };

        
        await context.SupplierDetails.AddAsync(supplier);
        await context.SaveChangesAsync();

        return supplierDto;
    }

    public async Task<List<SupplierDetails>> GetAllSuppliers()
    {
        return await context.SupplierDetails.ToListAsync();
    }

    public async Task<SupplierDetails> GetSupplierByIdAsync(int supplierId)
    {
            return await context.SupplierDetails.FirstOrDefaultAsync(s => s.SupplierId == supplierId);
    }

    public async Task DeleteSupplierAsync(SupplierDetails supplier)
    {
            var drugs = await context.SupplierDetails.Where(s => s.SupplierId == supplier.SupplierId).ToListAsync();

            // // Set the SupplierId of related drugs to null
            // foreach (var drug in drugs)
            // {
            //     drug.SupplierId = null;
            // }
            context.SupplierDetails.Remove(supplier);
            await context.SaveChangesAsync();
    }
}


