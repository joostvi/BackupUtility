using GenericClassLibrary.Logging;
using System.Collections.Generic;
using System.Windows;
using ZCopy.Classes;
using GenericClassLibrary.Yaml;
using System.IO;

namespace ZCopyTemplateBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            List<FolderMap> folderMaps = new List<FolderMap>();
            string[] exclusive = { "exclusive1", "exclusive2" };
            FolderMap folderMap = new FolderMap("c:\\tmp", "d:\\targer", true, true, exclusive);
            folderMaps.Add(folderMap);
            Commands commands = new Commands(true, folderMaps, true, true, true, true, EnumLogLevel.None);
            string yaml = YamlSerializer.Serialize(commands, emitDefaults: true);

            FileStream fileStream = new FileStream(Source.Path + "\\test.txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(fileStream);
            writer.Write(yaml);
            writer.Flush();
            writer.Dispose();
            fileStream.Close();
           
        }
    }
}
