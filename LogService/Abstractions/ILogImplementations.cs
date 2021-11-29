using System;
using System.Collections.Generic;
using System.Text;

namespace LogService.Abstractions
{
    public interface ILogImplementations
    {
        public void InfoMessage(string message);

        public void WarningMessage(string message);

        public void ErrorMessage(string message);

        public void DebugMessage(string message);
    }
}
