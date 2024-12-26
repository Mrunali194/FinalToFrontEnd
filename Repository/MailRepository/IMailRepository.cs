

using Demo.Dtos;

namespace Demo.Repository;

 
public interface IMailService
{
    void SendEmail(MailRequestDTO mailRequest);
    
}
 

 