using AutoMapper.Internal;
 
namespace Demo.Repository;

 
public interface IMailService
{
    void SendEmail(MailRequestDTO mailRequest);
    
}
 

 