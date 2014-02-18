using System.Windows;
using ProgramExecutionCounter.common;

namespace ProgramExecutionCounter.detail
{
    /// <summary>
    /// Interaction logic for EntryDetail.xaml
    /// </summary>
    public partial class EntryDetail : Window
    {
        public EntryDetail(CountEntry countEntry)
        {
            InitializeComponent();
            DataContext = new EntryDetailViewModel(countEntry);
        }
    }
}
