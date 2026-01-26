
namespace StudentManagement.Domain.Helper;

public class LocalDate 
{
    public static DateTime GetLocalDate()
    {
        DateTime utcNow = DateTime.UtcNow;
        return utcNow.ToLocalTime();
    }
}
