using Test.Inventory.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Inventory.API.Interfaces
{
    public interface ILog
    {
        Task Log(LogTypeEnum logType, string additionalInfo, Exception ex = null);
    }
}
