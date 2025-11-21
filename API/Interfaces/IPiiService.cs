using System.Threading.Tasks;
using QTip.Api.Database.DTOs;

namespace QTip.Api.Interfaces;
public interface IPiiService
{
    Task<ProcessingResult> ProcessPiiAsync(string rawInput);
    Task<int> GetPiiCountAsync();
}