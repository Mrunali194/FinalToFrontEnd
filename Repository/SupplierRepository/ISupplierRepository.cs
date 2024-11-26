using Demo.Model;

public interface ISupplierRepository
{
     Task<SupplierDetailsDto> AddSupplierDetails(SupplierDetailsDto supplierDto);
     Task<List<SupplierDetails>> GetAllSuppliers();
     Task<SupplierDetails> GetSupplierByIdAsync(int supplierId);
     Task DeleteSupplierAsync(SupplierDetails supplier);
}