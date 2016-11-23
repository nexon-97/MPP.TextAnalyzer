using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using System.Diagnostics;
using Analyzer;
using WpfClient.Model;
using WpfClient.Commands;

namespace WpfClient.ViewModel
{
	internal class ListItem : BaseViewModel
	{
		private string path;

		public string Path
		{
			get
			{
				return path;
			}
			set
			{
				path = value;

				RaisePropertyChanged(nameof(Path));
			}
		}

		public ButtonCommand Command { get; set; }
	}

	internal class AppViewModel : BaseViewModel
	{
		#region Fields
		private ObservableCollection<ListItem> resultFilesList;
		private string directoryText;
		private string filterText;
		private Visibility errorMessageVisibility;
		private string errorMessage;
		#endregion

		#region Properties
		public ObservableCollection<ListItem> ResultFilesList
		{
			get
			{
				return resultFilesList;
			}
			set
			{
				resultFilesList = value;

				RaisePropertyChanged(nameof(ResultFilesList));
			}
		}

		public string FilterText
		{
			get
			{
				return filterText;
			}
			set
			{
				filterText = value;

				RaisePropertyChanged(nameof(FilterText));
			}
		}

		public Visibility ErrorMessageVisibility
		{
			get
			{
				return errorMessageVisibility;
			}
			set
			{
				errorMessageVisibility = value;

				RaisePropertyChanged(nameof(ErrorMessageVisibility));
			}
		}

		public string ErrorMessage
		{
			get
			{
				return errorMessage;
			}
			set
			{
				errorMessage = value;

				RaisePropertyChanged(nameof(ErrorMessage));
			}
		}

		public string DirectoryText
		{
			get
			{
				return directoryText;
			}
			set
			{
				directoryText = value;

				RaisePropertyChanged(nameof(DirectoryText));
			}
		}

		public ButtonCommand SearchCommand { get; private set; }
		#endregion

		public AppViewModel()
		{
			ResultFilesList = new ObservableCollection<ListItem>();

			SearchCommand = new ButtonCommand(OnStartSearch);

			ShowError(false, string.Empty);
		}

		private void OnStartSearch(object param)
		{
			if (!Directory.Exists(DirectoryText))
			{
				ShowError(true, string.Format("Directory {0} doesn't exists!", DirectoryText));
				return;
			}

			if (!string.IsNullOrEmpty(FilterText))
			{
				ExpressionBuilder builder = new ExpressionBuilder();
				var filterExpression = builder.Build(FilterText);

				if (filterExpression != null)
				{
					ShowError(false, string.Empty);
					ResultFilesList.Clear();

					Filter filter = new Filter(filterExpression.Compile());

					FileToWordsParser parser = new FileToWordsParser();
					ProcessDirectory(DirectoryText, parser, filter);
				}
				else
				{
					ShowError(true, "Invalid filter expression!");
				}
			}
			else
			{
				ShowError(true, "Empty filter!");
			}
		}

		private void ProcessDirectory(string path, FileToWordsParser parser, Filter filter)
		{
			try
			{
				var files = Directory.EnumerateFiles(path);
				foreach (var file in files)
				{
					var input = parser.Parse(file);

					if (filter.Verify(input))
					{
						ResultFilesList.Add(new ListItem { Path = file, Command = new ButtonCommand(OnOpenResultFile) });
					}
				}	

				var subdirectories = Directory.GetDirectories(path);
				foreach (var subdir in subdirectories)
				{
					ProcessDirectory(subdir, parser, filter);
				}
			}
			catch (Exception)
			{

			}
		}

		private void ShowError(bool show, string message)
		{
			ErrorMessageVisibility = show ? Visibility.Visible : Visibility.Collapsed;
			ErrorMessage = message;
		}

		private void OnOpenResultFile(object param)
		{
			string path = param as string;
			if (path != null)
			{
				Process.Start(path);
			}
		}
	}
}
