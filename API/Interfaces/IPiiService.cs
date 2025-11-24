using System.Threading.Tasks;
using QTip.Api.Models;

namespace QTip.Api.Interfaces;
public interface IPiiService
{
    Task<ProcessingResult> ProcessPiiAsync(string rawInput);
    Task<PiiStats> GetPiiCountAsync();
    Task<PiiStats> GetPiiCountAsync(ClassificationTypes type);
}