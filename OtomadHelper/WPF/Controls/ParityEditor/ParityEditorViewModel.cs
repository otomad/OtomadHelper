using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtomadHelper.WPF.Controls;

public partial class ParityEditorViewModel<TArray> : ObservableObject<ParityEditor> where TArray: IList {
	[ObservableProperty]
	private uint column = 0;
	[ObservableProperty]
	private uint row = 0;
	[ObservableProperty]
	private TArray bits = default(TArray)!;

	public static implicit operator ParityEditorViewModel<TArray>(ParityEditorViewModel? viewModel) {
		if (viewModel is null) return null!;
		ValidateGenericType(viewModel.Bits);
		return (ParityEditorViewModel<TArray>)viewModel;
	}

	internal static void ValidateGenericType<TTest>(TTest test) {
		if (test is not bool[] or bool[,]) throw new ArgumentException($"Type of bits is not `bool[]` or `bool[,]`");
	}
}

public partial class ParityEditorViewModel : ParityEditorViewModel<IList> {
	public ParityEditorViewModel<bool[]>? As1D => (this.Bits is bool[]) ? this : null;
	public ParityEditorViewModel<bool[,]>? As2D => (this.Bits is bool[,]) ? this : null;
}
