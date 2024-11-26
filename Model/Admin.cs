using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Model;

public class Admin {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AdminId {get; set;}
    public string? Username {get; set;}
    public string? Password {get; set;}

}