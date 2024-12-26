using Demo.Dtos;
using Demo.Model;

namespace Demo.Repository;
public interface ISupplierRepository
{
     Task<SupplierDetailsDto> AddSupplierDetails(SupplierDetailsDto supplierDto);
     Task<List<SupplierDetails>> GetAllSuppliers();
     Task<SupplierDetails> GetSupplierByIdAsync(int supplierId);
     Task DeleteSupplierAsync(SupplierDetails supplier);
}