using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramExecutionCounter.common;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows;

namespace ProgramExecutionCounter.main
{
    public class MainWindowViewModel
    {
        private List<SourceType> sourceTypes;
        private ObservableCollection<CountEntry> countEntries;

        public MainWindowViewModel()
        {
            this.sourceTypes = new List<SourceType>()
            {
                new SourceType("Program", @"Software\Microsoft\Windows\CurrentVersion\Explorer\UserAssist\{CEBFF5CD-ACE2-4F4F-9178-9926F41749EA}\Count"),
                new SourceType("Shortcut", @"Software\Microsoft\Windows\CurrentVersion\Explorer\UserAssist\{F4E57C4B-2036-45F0-A9AB-443BCFE33D9F}\Count")
            };
            this.SelectedSourceType = sourceTypes[0];
            this.countEntries = new ObservableCollection<CountEntry>();
            this.SearchCommand = new DelegateCommand(OnSearch);            
        }

        public List<SourceType> SourceTypes 
        {
            get { return sourceTypes; }
        }

        public SourceType SelectedSourceType { get; set; }

        public string NameFilter { get; set; }
        
        public ObservableCollection<CountEntry> CountEntries
        {
            get { return countEntries; }
        }

        public ICommand SearchCommand { get; private set; }        

        private void OnSearch()
        {            
            countEntries.Clear();
                        
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(SelectedSourceType.Key);
            foreach (string valueName in reg.GetValueNames())
            {                
                CountEntry entry = new CountEntry();
                entry.Name = valueName;

                // filter by name
                if (!string.IsNullOrEmpty(NameFilter))
                {
                    if (!entry.DecodedName.ToUpper().Contains(NameFilter.ToUpper())) continue;
                }

                entry.Value = (byte[]) reg.GetValue(valueName);
                entry.RegKey = reg.ToString();
                entry.PropertyChanged += (o, e) =>
                {
                    CountEntry countEntry = (CountEntry)o;
                    if (e.PropertyName == "ExecutionCount")
                    {
                        try
                        {                            
                            byte[] newCount = BitConverter.GetBytes(countEntry.ExecutionCount);
                            newCount.CopyTo(countEntry.Value, 4);
                            Registry.SetValue(entry.RegKey, entry.Name, entry.Value);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error Updating Registry: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else if (e.PropertyName == "DeleteCommand")
                    {
                        CountEntries.Remove(countEntry);
                    }
                };
                countEntries.Add(entry);
            }            
        }        
    }    
}
