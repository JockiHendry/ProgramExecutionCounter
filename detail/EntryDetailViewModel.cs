using System.Text;
using ProgramExecutionCounter.common;

namespace ProgramExecutionCounter.detail
{
    public class EntryDetailViewModel
    {
        public EntryDetailViewModel(CountEntry countEntry)
        {
            this.RegistryKey = countEntry.RegKey + "\\" + countEntry.Name;

            StringBuilder hexValue = new StringBuilder();
            int i = 0;
            foreach (byte b in countEntry.Value)
            {
                hexValue.AppendFormat("{0,2:X2} ", b);
                if (++i % 8 == 0) hexValue.AppendLine();
            }
            this.Value = hexValue.ToString();
        }

        public string RegistryKey { get; set; }

        public string Value { get; set; }
    }
}
