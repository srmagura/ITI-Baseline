using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iti.Core.RequestTrace
{
    public interface IRequestTrace
    {
        void WriteTrace(string response);
        void WriteTrace(string request, string response);
    }
}
