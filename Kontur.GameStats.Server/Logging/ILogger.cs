using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public interface ILogger
    {
        void Log(MessageType messageType, string message);
    }
}
