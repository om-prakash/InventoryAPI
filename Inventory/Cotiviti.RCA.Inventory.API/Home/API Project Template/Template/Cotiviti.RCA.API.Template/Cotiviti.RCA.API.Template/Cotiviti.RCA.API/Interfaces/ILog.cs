using $safeprojectname$.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace $safeprojectname$.Interfaces
{
    public interface ILog
    {
        Task Log(LogTypeEnum logType, string additionalInfo, Exception ex = null);
    }
}
