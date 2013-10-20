namespace Application.Core.UI.ViewModels
{
	public class ModelViewModelBase<TModel> : ViewModelBase where TModel : new()
	{
		public TModel Model
		{
			get; set;
		}

		public ModelViewModelBase()
		{
		}

		public ModelViewModelBase(TModel model)
		{
			Model = model;
		}
	}
}
