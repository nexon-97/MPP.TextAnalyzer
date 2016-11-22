using System.ComponentModel;

namespace WpfClient.ViewModel
{
	internal class BaseViewModel : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged interface
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}
		#endregion
	}
}
