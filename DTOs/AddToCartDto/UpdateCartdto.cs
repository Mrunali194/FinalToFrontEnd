namespace Demo.Dtos;
public class UpdateCartItemDto
{
    public int UserId { get; set; }
    public int DrugId { get; set; }
    public int NewQuantity { get; set; }
}