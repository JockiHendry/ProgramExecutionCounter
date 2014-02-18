using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using ProgramExecutionCounter.detail;

namespace ProgramExecutionCounter.common
{
    public class CountEntry : INotifyPropertyChanged
    {
        private static Dictionary<char, char> lookupTable = new Dictionary<char, char>() 
        {
            {'A', 'N'}, {'a', 'n'},
            {'B', 'O'}, {'b', 'o'},
            {'C', 'P'}, {'c', 'p'},
            {'D', 'Q'}, {'d', 'q'},
            {'E', 'R'}, {'e', 'r'},
            {'F', 'S'}, {'f', 's'},
            {'G', 'T'}, {'g', 't'},
            {'H', 'U'}, {'h', 'u'},
            {'I', 'V'}, {'i', 'v'},
            {'J', 'W'}, {'j', 'w'},
            {'K', 'X'}, {'k', 'x'},
            {'L', 'Y'}, {'l', 'y'},
            {'M', 'Z'}, {'m', 'z'},
            {'N', 'A'}, {'n', 'a'},
            {'O', 'B'}, {'o', 'b'},
            {'P', 'C'}, {'p', 'c'},
            {'Q', 'D'}, {'q', 'd'},
            {'R', 'E'}, {'r', 'e'},
            {'S', 'F'}, {'s', 'f'},
            {'T', 'G'}, {'t', 'g'},
            {'U', 'H'}, {'u', 'h'},
            {'V', 'I'}, {'v', 'i'},
            {'W', 'J'}, {'w', 'j'},
            {'X', 'K'}, {'x', 'k'},
            {'Y', 'L'}, {'y', 'l'},
            {'Z', 'M'}, {'z', 'm'},
        };

        private static Dictionary<string, string> folderGUID = new Dictionary<string, string>()
        {
            {"{D20BEEC4-5CA8-4905-AE3B-BF251EA09B53}", "{Network}"},
            {"{0AC0837C-BBF8-452A-850D-79D08E667CA7}", "{Computer}"},
            {"{4D9F7874-4E0C-4904-967B-40B0D20C3E4B}", "{Internet}"},
            {"{82A74AEB-AEB4-465C-A014-D097EE346D63}", "{ControlPanel}"},
            {"{76FC4E2D-D6AD-4519-A663-37BD56068185}", "{Printers}"},
            {"{43668BF8-C14E-49B2-97C9-747784D784B7}", "{SyncSetup}"},
            {"{4bfefb45-347d-4006-a5be-ac0cb0567192}", "{Conflict}"},
            {"{289a9a43-be44-4057-a41b-587a76d7e7f9}", "{SyncResults}"},
            {"{B7534046-3ECB-4C18-BE4E-64CD4CB7D6AC}", "{RecycleBin}"},
            {"{6F0CD92B-2E97-45D1-88FF-B0D186B8DEDD}", "{Connections}"},
            {"{FD228CB7-AE11-4AE3-864C-16F3910AB8FE}", "{Fonts}"},
            {"{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", "{Desktop}"},
            {"{B97D20BB-F46A-4C97-BA10-5E3608430854}", "{Startup}"},
            {"{A77F5D77-2E2B-44C3-A6A2-ABA601054A51}", "{Programs}"},
            {"{625B53C3-AB48-4EC1-BA1F-A1EF4146FC19}", "{StartMenu}"},
            {"{AE50C081-EBD2-438A-8655-8A092E34987A}", "{Recent}"},
            {"{8983036C-27C0-404B-8F08-102D10DCFD74}", "{SendTo}"},
            {"{FDD39AD0-238F-46AF-ADB4-6C85480369C7}", "{Documents}"},
            {"{1777F761-68AD-4D8A-87BD-30B759FA33DD}", "{Favorites}"},
            {"{C5ABBF53-E17F-4121-8900-86626FC2C973}", "{NetHood}"},
            {"{9274BD8D-CFD1-41C3-B35E-B13F55A758F4}", "{PrintHood}"},
            {"{A63293E8-664E-48DB-A079-DF759E0509F7}", "{Templates}"},
            {"{82A5EA35-D9CD-47C5-9629-E15D2F714E6E}", "{CommonStartup}"},
            {"{0139D44E-6AFE-49F2-8690-3DAFCAE6FFB8}", "{CommonPrograms}"},
            {"{A4115719-D62E-491D-AA7C-E74B8BE3B067}", "{CommonStartMenu}"},
            {"{C4AA340D-F20F-4863-AFEF-F87EF2E6BA25}", "{PublicDesktop}"},
            {"{62AB5D82-FDC1-4DC3-A9DD-070D1D495D97}", "{ProgramData}"},
            {"{B94237E7-57AC-4347-9151-B08C6C32D1F7}", "{CommonTemplates}"},
            {"{ED4824AF-DCE4-45A8-81E2-FC7965083634}", "{PublicDocuments}"},
            {"{3EB685DB-65F9-4CF6-A03A-E3EF65729F3D}", "{RoamingAppData}"},
            {"{F1B32785-6FBA-4FCF-9D55-7B8E7F157091}", "{LocalAppData}"},
            {"{A520A1A4-1780-4FF6-BD18-167343C5AF16}", "{LocalAppDataLow}"},
            {"{352481E8-33BE-4251-BA85-6007CAEDCF9D}", "{InternetCache}"},
            {"{2B0F765D-C0E9-4171-908E-08A611B84FF6}", "{Cookies}"},
            {"{D9DC8A3B-B784-432E-A781-5A1130A75963}", "{History}"},
            {"{1AC14E77-02E7-4E5D-B744-2EB1AE5198B7}", "{System}"},
            {"{D65231B0-B2F1-4857-A4CE-A8E7C6EA7D27}", "{SystemX86}"},
            {"{F38BF404-1D43-42F2-9305-67DE0B28FC23}", "{Windows}"},
            {"{5E6C858F-0E22-4760-9AFE-EA3317B67173}", "{Profile}"},
            {"{33E28130-4E1E-4676-835A-98395C3BC3BB}", "{Pictures}"},
            {"{7C5A40EF-A0FB-4BFC-874A-C0F2E0B9FA8E}", "{ProgramFilesX86}"},
            {"{DE974D24-D9C6-4D3E-BF91-F4455120B917}", "{ProgramFilesCommonX86}"},
            {"{6D809377-6AF0-444b-8957-A3773F02200E}", "{ProgramFilesX64}"},
            {"{6365D5A7-0F0D-45e5-87F6-0DA56B6A4F7D}", "{ProgramFilesCommonX64}"},
            {"{905e63b6-c1bf-494e-b29c-65b732d3d21a}", "{ProgramFiles}"},
            {"{F7F1ED05-9F6D-47A2-AAAE-29D317C6F066}", "{ProgramFilesCommon}"},
            {"{5cd7aee2-2219-4a67-b85d-6c9ce15660cb}", "{UserProgramFiles}"},
            {"{bcbd3057-ca5c-4622-b42d-bc56db0ae516}", "{UserProgramFilesCommon}"},
            {"{724EF170-A42D-4FEF-9F26-B60E846FBA4F}", "{AdminTools}"},
            {"{D0384E7D-BAC3-4797-8F14-CBA229B392B5}", "{CommonAdminTools}"},
            {"{4BD8D571-6D19-48D3-BE97-422220080E43}", "{Music}"},
            {"{18989B1D-99B5-455B-841C-AB7C74E4DDFC}", "{Videos}"},
            {"{C870044B-F49E-4126-A9C3-B52A1FF411E8}", "{Ringtones}"},
            {"{B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5}", "{PublicPictures}"},
            {"{3214FAB5-9757-4298-BB61-92A9DEAA44FF}", "{PublicMusic}"},
            {"{2400183A-6185-49FB-A2D8-4A392A602BA3}", "{PublicVideos}"},
            {"{E555AB60-153B-4D17-9F04-A5FE99FC15EC}", "{PublicRingtones}"},
            {"{8AD10C31-2ADB-4296-A8F7-E4701232C972}", "{ResourceDir}"},
            {"{2A00375E-224C-49DE-B8D1-440DF7EF3DDC}", "{LocalizedResourcesDir}"},
            {"{C1BAE2D0-10DF-4334-BEDD-7AA20B227A9D}", "{CommonOEMLinks}"},
            {"{9E52AB10-F80D-49DF-ACB8-4330F5687855}", "{CDBurning}"},
            {"{0762D272-C50A-4BB0-A382-697DCD729B80}", "{UserProfiles}"},
            {"{DE92C1C7-837F-4F69-A3BB-86E631204A23}", "{Playlists}"},
            {"{15CA69B3-30EE-49C1-ACE1-6B5EC372AFB5}", "{SamplePlaylists}"},
            {"{B250C668-F57D-4EE1-A63C-290EE7D1AA1F}", "{SampleMusic}"},
            {"{C4900540-2379-4C75-844B-64E6FAF8716B}", "{SamplePictures}"},
            {"{859EAD94-2E85-48AD-A71A-0969CB56A6CD}", "{SampleVideos}"},
            {"{69D2CF90-FC33-4FB7-9A0C-EBB0F0FCB43C}", "{PhotoAlbums}"},
            {"{DFDF76A2-C82A-4D63-906A-5644AC457385}", "{Public}"},
            {"{df7266ac-9274-4867-8d55-3bd661de872d}", "{ChangeRemovePrograms}"},
            {"{a305ce99-f527-492b-8b1a-7e76fa98d6e4}", "{AppUpdates}"},
            {"{de61d971-5ebc-4f02-a3a9-6c82895e5c04}", "{AddNewPrograms}"},
            {"{374DE290-123F-4565-9164-39C4925E467B}", "{Downloads}"},
            {"{3D644C9B-1FB8-4f30-9B45-F670235F79C0}", "{PublicDownloads}"},
            {"{7d1d3a04-debb-4115-95cf-2f29da2920da}", "{SavedSearches}"},
            {"{52a4f021-7b75-48a9-9f6b-4b87a210bc8f}", "{QuickLaunch}"},
            {"{56784854-C6CB-462b-8169-88E350ACB882}", "{Contacts}"},        
        };

        private byte[] value;
        private string name;
        private int executionCount;
        private string regKey;
        private ICommand deleteCommand;
        private ICommand detailCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        public CountEntry()
        {
            this.deleteCommand = new DelegateCommand(OnDelete);
            this.detailCommand = new DelegateCommand(OnDetail);
        }

        public byte[] Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
                executionCount = BitConverter.ToInt32(value, 4);
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string RegKey
        {
            get { return regKey; }
            set { regKey = value; }
        }

        public String DecodedName
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return "";
                }
                else
                {
                    string result = new string(Name.ToCharArray().Select(c =>
                    {
                        return lookupTable.ContainsKey(c) ? lookupTable[c] : c;
                    }).ToArray());
                    foreach (var f in folderGUID)
                    {
                        if (result.Contains(f.Key)) result = result.Replace(f.Key, f.Value);
                    }
                    return result;
                }
            }
        }

        public int ExecutionCount
        {
            get { return executionCount; }
            set
            {
                executionCount = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ExecutionCount"));
            }
        }

        public ICommand DeleteCommand
        {
            get { return deleteCommand; }
        }

        public ICommand DetailCommand
        {
            get { return detailCommand; }
        }

        public void OnDelete()
        {
            if (MessageBox.Show("Do you really want delete this entry?", "Delete Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey.Replace(@"HKEY_CURRENT_USER\", ""), true);
                if (key != null)
                {
                    try
                    {
                        key.DeleteValue(Name);
                        PropertyChanged(this, new PropertyChangedEventArgs("DeleteCommand"));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Deleting Registry: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }

        public void OnDetail()
        {
            EntryDetail detailForm = new EntryDetail(this);
            detailForm.ShowDialog();
        }

    }

}
