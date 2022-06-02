using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using Arhiv;
using Path = System.IO.Path;

namespace DocumentsApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
        public MainWindow()
        {
           InitializeComponent();
        }
		private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				OpenFileDialog dlg = new OpenFileDialog() 
				{ 
					Filter = "Text files (*.txt)|*.txt|BtB files (*.btb)|*.btb"
				};
				if (dlg.ShowDialog() == true)
				{
					if (Path.GetExtension(dlg.FileName) == ".btb")
					{
						Arhivator open = new Arhivator(dlg.FileName, rtbEditor);
						open.Load();
					}
					else
					{
						FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
						TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
						range.Load(fileStream, DataFormats.Rtf);
					}
				}
			}
			catch (ArhivException ex)
            {
				MessageBox.Show(ex.Message);
			}
		}

		private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog()
			{
				Filter = "Text files (*.txt)|*.txt"
			};
			if (dlg.ShowDialog() == true)
			{
				FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
				TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
				range.Save(fileStream, DataFormats.Rtf);
			}
		}
		private void Zip_Executed(object sender, ExecutedRoutedEventArgs e )
        {

			try
			{
				SaveFileDialog dlg = new SaveFileDialog()
				{
					Filter = "BtB files (*.btb)|*.btb"
				};
				if (dlg.ShowDialog() == true)
				{
					Arhivator arhiv = new Arhivator(rtbEditor, dlg.FileName);
					arhiv.Save();
					arhiv = null;
				}
			}
			catch (ArhivException ex) {MessageBox.Show(ex.Message);}
			catch (ArgumentException ex) { MessageBox.Show(ex.Message); }
			
		}
	}
}
